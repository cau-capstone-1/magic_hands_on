using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    private int stageNumber;
    private int difficulty;
    private int tutorialStep = 0;

    [SerializeField] private GameObject character; // 튜토리얼 캐릭터 오브젝트
    [SerializeField] private Text tutorialText; // 튜토리얼 안내 텍스트
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private Button confirmButton;

    private void Start()
    {
        // GameManager에서 스테이지와 난이도 정보 가져오기
        stageNumber = GameManager.Instance.currentStageNumber;
        difficulty = GameManager.Instance.currentDifficulty;

        // 튜토리얼 시작
        StartTutorial();
    }

    private void StartTutorial()
    {
        tutorialStep = 0;
        tutorialText.text = "튜토리얼 시작: 소녀가 마법의 구를 훔쳐 달아나려해요…!";
        character.SetActive(true);
        confirmButton.onClick.AddListener(NextTutorialStep);
        StartCoroutine(CharacterIntroAnimation());
    }

    private IEnumerator CharacterIntroAnimation()
    {
        // 캐릭터가 밑에서 위로 올라오는 애니메이션
        Vector3 startPosition = character.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 2; // 원하는 위치로 이동
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
                tutorialText.text = "오른손의 O 버튼과 왼손의 O 버튼을 눌러보세요!";
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);
                break;

            case 2:
                tutorialText.text = "왼쪽에서 적이 나타났어요! 적의 머리 위 심볼에 맞춰 왼손에서 버튼을 눌러보세요!";
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
                break;

            case 3:
                tutorialText.text = "잘하셨어요! 오른쪽에도 적이 나타납니다. 이번엔 오른손으로 맞춰보세요!";
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
        Debug.Log("튜토리얼이 끝났습니다. 이제 게임을 시작합니다.");

        // 실제 게임 초기화
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
