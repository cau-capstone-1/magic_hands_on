using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleAnimationController : MonoBehaviour
{
    public RectTransform titleText; // Title_Text 오브젝트
    public CanvasGroup buttonPlay; // Button_Play 오브젝트의 CanvasGroup

    public float titleAnimationDuration = 1.0f; // Title_Text 애니메이션 시간
    public float buttonFadeInDuration = 1.0f; // Button_Play 페이드인 시간
    public Vector3 titleStartPos; // Title_Text 시작 위치

    private Vector3 titleEndPos; // Title_Text 최종 위치
    private Button playButton;

    private void Awake()
    {
        // Title_Text와 Button_Play 오브젝트의 초기 설정
        titleEndPos = titleText.anchoredPosition;
        titleText.anchoredPosition = titleStartPos;

        buttonPlay.alpha = 0;
        playButton = buttonPlay.GetComponent<Button>();
        playButton.interactable = false; // 초기에는 버튼 비활성화
    }

    private void OnEnable()
    {
        StartCoroutine(PlayTitleAnimation());
    }

    private IEnumerator PlayTitleAnimation()
    {
        // Title_Text의 위치 애니메이션
        float elapsedTime = 0;
        while (elapsedTime < titleAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / titleAnimationDuration;
            t = Mathf.SmoothStep(0, 1, t); // 부드러운 이징 효과
            titleText.anchoredPosition = Vector3.Lerp(titleStartPos, titleEndPos, t);
            yield return null;
        }

        // Button_Play의 페이드인 애니메이션
        elapsedTime = 0;
        while (elapsedTime < buttonFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / buttonFadeInDuration;
            buttonPlay.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        // 애니메이션이 끝난 후 버튼 활성화
        playButton.interactable = true;
    }
}
