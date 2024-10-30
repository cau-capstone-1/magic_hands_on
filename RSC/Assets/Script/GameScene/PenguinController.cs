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
        // 특정 키 입력 감지 (예: 스페이스바)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAttackAnimation();
        }
    }

    public void PlayAttackAnimation()
    {
        // Attack 트리거 활성화하여 애니메이션 재생
        animator.SetTrigger("Attack");
    }
}
