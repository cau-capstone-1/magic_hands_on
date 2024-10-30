using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private int stageNumber;
    private int difficulty;

    private void Start()
    {
        // GameManager���� ���������� ���̵� ���� ��������
        stageNumber = GameManager.Instance.currentStageNumber;
        difficulty = GameManager.Instance.currentDifficulty;

        InitializeStage();
    }

    private void InitializeStage()
    {
        // ���̵��� ���� ���� ��� ���� ����
        switch (difficulty)
        {
            case 1:
                // ���� ���̵� ���� (��: ���� ���� �ӵ��� ���� ����)
                break;
            case 2:
                // �߰� ���̵� ����
                break;
            case 3:
                // ����� ���̵� ����
                break;
        }

        Debug.Log($"�������� {stageNumber} ���� - ���̵�: {difficulty}");
    }
}
