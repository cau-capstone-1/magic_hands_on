using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Slider timerSlider; // Slider ������Ʈ ����
    private float gameDuration = 120f; // ���� �ð� (��)
    private float elapsedTime = 0f;    // ��� �ð�

    private void Start()
    {
        timerSlider.minValue = 0;
        timerSlider.maxValue = gameDuration;
        timerSlider.value = gameDuration; // Ÿ�̸Ӱ� ���۵� �� �ִ� ������ ����
    }

    private void Update()
    {
        if (elapsedTime < gameDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateSlider();
        }
    }

    private void UpdateSlider()
    {
        float remainingTime = gameDuration - elapsedTime;
        timerSlider.value = remainingTime; // �����̴� ���� ���� �ð����� ����
    }

    // Ÿ�̸Ӹ� �ʱ�ȭ�ϴ� �޼���
    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerSlider.value = gameDuration; // �ʱ� ���·� �����̴� ���� �ִ� ������ ����
    }
}
