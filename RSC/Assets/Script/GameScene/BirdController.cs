using UnityEngine;

public class BirdController : MonoBehaviour
{
    public string BirdColor { get; private set; }
    private Transform target;
    private int hp;
    private float attackDistance = 1.5f; // 플레이어에 도달하는 거리 임계값
    public float SpawnTime { get; private set; } // 새가 생성된 시간 기록

    [SerializeField] private ParticleSystem destructionParticle; // 파괴 시 파티클
    [SerializeField] private AudioClip damageSound; // 데미지 사운드
    private AudioSource audioSource; // AudioSource 컴포넌트

    public void Initialize(string color, Transform target, int hp, float spawnTime)
    {
        BirdColor = color;
        this.target = target;
        this.hp = hp;
        SpawnTime = spawnTime; // 생성 시간 초기화
    }

    private void Start()
    {
        // AudioSource 컴포넌트를 가져오거나 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // 자동 재생 비활성화
    }

    private void Update()
    {
        MoveTowardsTarget();

        // 플레이어에 가까이 도달했는지 확인
        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            GameSceneController gameController = FindObjectOfType<GameSceneController>();
            gameController.TakeDamage(1); // 플레이어 HP를 1 감소
            Destroy(gameObject); // 새 제거
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime;
        }
    }

    public void TakeDamage()
    {
        hp--;
        Debug.Log($"Bird {BirdColor} took damage. HP remaining: {hp}");
        if (hp <= 0)
        {
            GameSceneController gameController = FindObjectOfType<GameSceneController>();
            gameController.DisplayActionTextBasedOnTime(this); // 시간에 따른 ActionText 표시

            PlayDamageSound(); // 데미지 사운드 재생
            TriggerDestructionEffect(); // 파괴 효과 실행
            Destroy(gameObject);
        }
    }

    private void TriggerDestructionEffect()
    {
        if (destructionParticle != null)
        {
            ParticleSystem particleInstance = Instantiate(destructionParticle, transform.position, Quaternion.identity);
            particleInstance.Play();

            // 파티클이 끝난 뒤 자동으로 제거
            Destroy(particleInstance.gameObject, particleInstance.main.duration + particleInstance.main.startLifetime.constantMax);
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    public int GetCurrentHP()
    {
        return hp;
    }
}
