using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private int stageNumber;
    private int difficulty;

    private void Start()
    {
        // GameManager에서 스테이지와 난이도 정보 가져오기
        stageNumber = GameManager.Instance.currentStageNumber;
        difficulty = GameManager.Instance.currentDifficulty;

        InitializeStage();
    }

    private void InitializeStage()
    {
        // 난이도에 따른 게임 요소 조정 예시
        switch (difficulty)
        {
            case 1:
                // 쉬운 난이도 설정 (예: 적의 수와 속도를 낮게 설정)
                break;
            case 2:
                // 중간 난이도 설정
                break;
            case 3:
                // 어려운 난이도 설정
                break;
        }

        Debug.Log($"스테이지 {stageNumber} 시작 - 난이도: {difficulty}");
    }
}
