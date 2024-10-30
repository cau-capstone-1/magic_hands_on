using UnityEngine;

public class PenguinController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Ư�� Ű �Է� ���� (��: �����̽���)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAttackAnimation();
        }
    }

    public void PlayAttackAnimation()
    {
        // Attack Ʈ���� Ȱ��ȭ�Ͽ� �ִϸ��̼� ���
        animator.SetTrigger("Attack");
    }
}
