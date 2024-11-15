using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private List<GameObject> popups; // �˾� ������Ʈ ����Ʈ
    private Dictionary<string, CanvasGroup> popupCanvasGroups = new Dictionary<string, CanvasGroup>();
    private float fadeDuration = 0.5f; // ���̵� ��/�ƿ� �ð�

    private void Start()
    {
        // ��� �˾��� CanvasGroup ���� �� ��Ȱ��ȭ
        foreach (var popup in popups)
        {
            CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = popup.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            popupCanvasGroups.Add(popup.name, canvasGroup);
            popup.SetActive(false);
        }
    }

    // Ư�� �˾��� ���� �޼��� (���̵� ��)
    public void ShowPopup(string popupName)
    {
        foreach (var popup in popups)
        {
            if (popup.name == popupName)
            {
                StartCoroutine(FadeIn(popupCanvasGroups[popupName]));
            }
            else
            {
                StartCoroutine(FadeOut(popupCanvasGroups[popup.name]));
            }
        }
    }

    // ��� �˾��� �ݴ� �޼��� (���̵� �ƿ�)
    public void CloseAllPopups()
    {
        foreach (var popup in popups)
        {
            StartCoroutine(FadeOut(popupCanvasGroups[popup.name]));
        }
    }

    // ���̵� �� �ڷ�ƾ
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // ���̵� �ƿ� �ڷ�ƾ
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }
}
