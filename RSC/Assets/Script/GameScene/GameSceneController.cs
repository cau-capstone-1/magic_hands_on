using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    private int stageNumber;
    private int difficulty;
    private int tutorialStep = 0;

    [SerializeField] private GameObject character; // Ʃ�丮�� ĳ���� ������Ʈ
    [SerializeField] private Text tutorialText; // Ʃ�丮�� �ȳ� �ؽ�Ʈ
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private Button confirmButton;

    private void Start()
    {
        // GameManager���� ���������� ���̵� ���� ��������
        stageNumber = GameManager.Instance.currentStageNumber;
        difficulty = GameManager.Instance.currentDifficulty;

        // Ʃ�丮�� ����
        StartTutorial();
    }

    private void StartTutorial()
    {
        tutorialStep = 0;
        tutorialText.text = "Ʃ�丮�� ����: �ҳడ ������ ���� ���� �޾Ƴ����ؿ䡦!";
        character.SetActive(true);
        confirmButton.onClick.AddListener(NextTutorialStep);
        StartCoroutine(CharacterIntroAnimation());
    }

    private IEnumerator CharacterIntroAnimation()
    {
        // ĳ���Ͱ� �ؿ��� ���� �ö���� �ִϸ��̼�
        Vector3 startPosition = character.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 2; // ���ϴ� ��ġ�� �̵�
        float elapsedTime = 0;
        float duration = 1.0f;

        while (elapsedTime < duration)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;
    }

    private void NextTutorialStep()
    {
        tutorialStep++;

        switch (tutorialStep)
        {
            case 1:
                tutorialText.text = "�������� O ��ư�� �޼��� O ��ư�� ����������!";
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);
                break;

            case 2:
                tutorialText.text = "���ʿ��� ���� ��Ÿ�����! ���� �Ӹ� �� �ɺ��� ���� �޼տ��� ��ư�� ����������!";
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
                break;

            case 3:
                tutorialText.text = "���ϼ̾��! �����ʿ��� ���� ��Ÿ���ϴ�. �̹��� ���������� ���纸����!";
                break;

            case 4:
                EndTutorial();
                break;
        }
    }

    private void EndTutorial()
    {
        tutorialText.text = "";
        confirmButton.gameObject.SetActive(false);
        character.SetActive(false);
        Debug.Log("Ʃ�丮���� �������ϴ�. ���� ������ �����մϴ�.");

        // ���� ���� �ʱ�ȭ
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
