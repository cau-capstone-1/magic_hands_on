using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TitleAnimationController : MonoBehaviour
{
    public RectTransform titleText; // Title_Text ������Ʈ
    public CanvasGroup buttonPlay; // Button_Play ������Ʈ�� CanvasGroup

    public float titleAnimationDuration = 1.0f; // Title_Text �ִϸ��̼� �ð�
    public float buttonFadeInDuration = 1.0f; // Button_Play ���̵��� �ð�
    public Vector3 titleStartPos; // Title_Text ���� ��ġ

    private Vector3 titleEndPos; // Title_Text ���� ��ġ
    private Button playButton;
    public Transform leftPenguinRect;
    public Transform rightPenguinRect;
    public SpriteRenderer leftPenguinRenderer;
    public SpriteRenderer rightPenguinRenderer;

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
        float initialLeftX = leftPenguinRect.position.x;
        float initialRightX = rightPenguinRect.position.x;
        
        while (elapsedTime < buttonFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / buttonFadeInDuration;
            buttonPlay.alpha = Mathf.Lerp(0, 1, t);

            leftPenguinRenderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            rightPenguinRenderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));

            leftPenguinRect.position = new Vector3(initialLeftX - Mathf.Lerp(0, 1, t), leftPenguinRect.position.y, leftPenguinRect.position.z);
            rightPenguinRect.position = new Vector3(initialRightX + Mathf.Lerp(0, 1, t), rightPenguinRect.position.y, rightPenguinRect.position.z);
            
            yield return null;
        }

        // �ִϸ��̼��� ���� �� ��ư Ȱ��ȭ
        playButton.interactable = true;
    }
}
