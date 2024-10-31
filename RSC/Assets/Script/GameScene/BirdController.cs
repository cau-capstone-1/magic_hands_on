using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public string BirdColor { get; private set; }
    private Transform target;
    private int hp;

    public void Initialize(string color, Transform target, int hp)
    {
        BirdColor = color;
        this.target = target;
        this.hp = hp;
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime; // 새가 천천히 다가감
        }
    }

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject); // HP가 0이 되면 새 제거
        }
    }
}
