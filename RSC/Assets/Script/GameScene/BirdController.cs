using UnityEngine;

public class BirdController : MonoBehaviour
{
    public string BirdColor { get; private set; }
    private Transform target;
    private int hp;
    private float attackDistance = 1.5f; // �÷��̾ �����ϴ� �Ÿ� �Ӱ谪
    public float SpawnTime { get; private set; } // ���� ������ �ð� ���

    [SerializeField]
    private ParticleSystem damageParticle; // �ı� �� ��ƼŬ

    [SerializeField]
    private ParticleSystem destructionParticle; // �ı� �� ��ƼŬ

    [SerializeField]
    private AudioClip damageSound; // ������ ����
    private AudioSource audioSource; // AudioSource ������Ʈ

    private GameSceneController gameSceneController;

    public void Initialize(
        GameSceneController gameSceneController,
        string color,
        Transform target,
        int hp,
        float spawnTime
    )
    {
        this.gameSceneController = gameSceneController;
        BirdColor = color;
        this.target = target;
        this.hp = hp;
        SpawnTime = spawnTime; // ���� �ð� �ʱ�ȭ
    }

    private void Start()
    {
        // AudioSource ������Ʈ�� �������ų� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
    }

    private void Update()
    {
        MoveTowardsTarget();

        // �÷��̾ ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            this.gameSceneController.TakeDamage(1); // �÷��̾� HP�� 1 ����
            Destroy(gameObject); // �� ����
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
            gameController.DisplayActionTextBasedOnTime(this); // �ð��� ���� ActionText ǥ��

            PlayDamageSound(); // ������ ���� ���
            TriggerDestructionEffect(); // �ı� ȿ�� ����
            Destroy(gameObject);

            if (BirdColor == "Black")
            {
                GameData.instance.blackBirdUsageCount += 1;
            }

            // 적 처치 시 100점 추가
            this.gameSceneController.score += 100;

            float elapsedTime = Time.time - this.SpawnTime;

            // 빨리 처치하면 추가 점수
            if (elapsedTime < 2f)
            {
                this.gameSceneController.score += 50;
            }
            else if (elapsedTime < 4f)
            {
                this.gameSceneController.score += 25;
            }
        }
        else
        {
            TriggerDamageEffect();
        }
    }

    private void TriggerDestructionEffect()
    {
        if (destructionParticle != null)
        {
            ParticleSystem particleInstance = Instantiate(
                destructionParticle,
                transform.position,
                Quaternion.identity
            );
            particleInstance.Play();

            // ��ƼŬ�� ���� �� �ڵ����� ����
            Destroy(
                particleInstance.gameObject,
                particleInstance.main.duration + particleInstance.main.startLifetime.constantMax
            );
        }
    }

    private void TriggerDamageEffect()
    {
        if (damageParticle != null)
        {
            ParticleSystem particleInstance = Instantiate(
                damageParticle,
                transform.position,
                Quaternion.identity
            );

            // ũ�� ����
            particleInstance.transform.localScale *= 1.6f;

            particleInstance.Play();

            // ��ƼŬ�� ���� �� �ڵ����� ����
            Destroy(
                particleInstance.gameObject,
                particleInstance.main.duration + particleInstance.main.startLifetime.constantMax
            );
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
