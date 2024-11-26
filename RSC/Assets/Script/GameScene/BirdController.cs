using UnityEngine;

public class BirdController : MonoBehaviour
{
    public string BirdColor { get; private set; }
    private Transform target;
    private int hp;
    private float attackDistance = 1.5f; // �÷��̾ �����ϴ� �Ÿ� �Ӱ谪
    public float SpawnTime { get; private set; } // ���� ������ �ð� ���

    [SerializeField] private ParticleSystem destructionParticle; // �ı� �� ��ƼŬ

    public void Initialize(string color, Transform target, int hp, float spawnTime)
    {
        BirdColor = color;
        this.target = target;
        this.hp = hp;
        SpawnTime = spawnTime; // ���� �ð� �ʱ�ȭ
    }

    private void Update()
    {
        MoveTowardsTarget();

        // �÷��̾ ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            GameSceneController gameController = FindObjectOfType<GameSceneController>();
            gameController.TakeDamage(1); // �÷��̾� HP�� 1 ����
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
        TriggerDestructionEffect(); // �ı� ȿ�� ����
        if (hp <= 0)
        {
            GameSceneController gameController = FindObjectOfType<GameSceneController>();
            gameController.DisplayActionTextBasedOnTime(this); // �ð��� ���� ActionText ǥ��

            Destroy(gameObject);
        }
    }

    private void TriggerDestructionEffect()
    {
        if (destructionParticle != null)
        {
            ParticleSystem particleInstance = Instantiate(destructionParticle, transform.position, Quaternion.identity);
            particleInstance.Play();

            // ��ƼŬ�� ���� �� �ڵ����� ����
            Destroy(particleInstance.gameObject, particleInstance.main.duration + particleInstance.main.startLifetime.constantMax);
        }
    }

    public int GetCurrentHP()
    {
        return hp;
    }
}
