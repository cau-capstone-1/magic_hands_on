using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentStageNumber { get; private set; }
    public int currentDifficulty { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬에서도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 스테이지 선택 메서드
    public void SelectStage(int stageNumber, int difficulty)
    {
        currentStageNumber = stageNumber;
        currentDifficulty = difficulty;

        // 게임 씬으로 이동
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        // 게임 씬 로드 (예: "GameScene"으로 씬 이름을 변경)
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
