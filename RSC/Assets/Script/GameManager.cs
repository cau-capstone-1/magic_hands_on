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
            DontDestroyOnLoad(gameObject); // �ٸ� �������� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �������� ���� �޼���
    public void SelectStage(int stageNumber, int difficulty)
    {
        currentStageNumber = stageNumber;
        currentDifficulty = difficulty;

        // ���� ������ �̵�
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        // ���� �� �ε� (��: "GameScene"���� �� �̸��� ����)
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
