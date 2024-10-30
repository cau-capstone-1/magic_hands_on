using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public int stageNumber; // �������� ��ȣ
    public int difficulty;  // �������� ���̵� (1: ����, 2: �߰�, 3: �����)

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnStageSelected); // �������� ���� �� �̺�Ʈ �߰�
    }

    private void OnStageSelected()
    {
        // ���� �Ŵ����� ���� �������� ������ ����
        GameManager.Instance.SelectStage(stageNumber, difficulty);
    }
}
