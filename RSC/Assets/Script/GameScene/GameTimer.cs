using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private RectTransform fillRectTransform;
    private float gameDuration = 120f; // 게임 시간 (초)
    private float elapsedTime = 0f;    // 경과 시간

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
        float progress = 1 - (elapsedTime / gameDuration); // 남은 시간 비율 계산
        fillRectTransform.offsetMax = new Vector2(-440 + progress * 800, fillRectTransform.offsetMax.y);
    }

    // 타이머를 초기화하는 메서드
    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateFill(); // 초기 상태로 업데이트
    }
}
