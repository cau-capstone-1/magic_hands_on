using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private RectTransform fillRectTransform;
    private float gameDuration = 120f; // ���� �ð� (��)
    private float elapsedTime = 0f;    // ��� �ð�

    private void Update()
    {
        if (elapsedTime < gameDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateFill();
        }
    }

    private void UpdateFill()
    {
        float progress = 1 - (elapsedTime / gameDuration); // ���� �ð� ���� ���
        fillRectTransform.offsetMax = new Vector2(-440 + progress * 800, fillRectTransform.offsetMax.y);
    }

    // Ÿ�̸Ӹ� �ʱ�ȭ�ϴ� �޼���
    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateFill(); // �ʱ� ���·� ������Ʈ
    }
}
