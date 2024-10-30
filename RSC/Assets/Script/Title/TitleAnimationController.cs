using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleAnimationController : MonoBehaviour
{
    public RectTransform titleText; // Title_Text ������Ʈ
    public CanvasGroup buttonPlay; // Button_Play ������Ʈ�� CanvasGroup

    public float titleAnimationDuration = 1.0f; // Title_Text �ִϸ��̼� �ð�
    public float buttonFadeInDuration = 1.0f; // Button_Play ���̵��� �ð�
    public Vector3 titleStartPos; // Title_Text ���� ��ġ

    private Vector3 titleEndPos; // Title_Text ���� ��ġ
    private Button playButton;

    private void Awake()
    {
        // Title_Text�� Button_Play ������Ʈ�� �ʱ� ����
        titleEndPos = titleText.anchoredPosition;
        titleText.anchoredPosition = titleStartPos;

        buttonPlay.alpha = 0;
        playButton = buttonPlay.GetComponent<Button>();
        playButton.interactable = false; // �ʱ⿡�� ��ư ��Ȱ��ȭ
    }

    private void OnEnable()
    {
        StartCoroutine(PlayTitleAnimation());
    }

    private IEnumerator PlayTitleAnimation()
    {
        // Title_Text�� ��ġ �ִϸ��̼�
        float elapsedTime = 0;
        while (elapsedTime < titleAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / titleAnimationDuration;
            t = Mathf.SmoothStep(0, 1, t); // �ε巯�� ��¡ ȿ��
            titleText.anchoredPosition = Vector3.Lerp(titleStartPos, titleEndPos, t);
            yield return null;
        }

        // Button_Play�� ���̵��� �ִϸ��̼�
        elapsedTime = 0;
        while (elapsedTime < buttonFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / buttonFadeInDuration;
            buttonPlay.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        // �ִϸ��̼��� ���� �� ��ư Ȱ��ȭ
        playButton.interactable = true;
    }
}
