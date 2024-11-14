using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Slider timerSlider; // Slider 컴포넌트 참조
    private float gameDuration = 120f; // 게임 시간 (초)
    private float elapsedTime = 0f;    // 경과 시간

    private void Start()
    {
        timerSlider.minValue = 0;
        timerSlider.maxValue = gameDuration;
        timerSlider.value = gameDuration; // 타이머가 시작될 때 최대 값으로 설정
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
        timerSlider.value = remainingTime; // 슬라이더 값을 남은 시간으로 설정
    }

    // 타이머를 초기화하는 메서드
    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerSlider.value = gameDuration; // 초기 상태로 슬라이더 값을 최대 값으로 설정
    }
}
