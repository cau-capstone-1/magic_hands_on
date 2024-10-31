using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private List<GameObject> popups; // 팝업 오브젝트 리스트
    private Dictionary<string, CanvasGroup> popupCanvasGroups = new Dictionary<string, CanvasGroup>();
    private float fadeDuration = 0.5f; // 페이드 인/아웃 시간

    private void Start()
    {
        // 모든 팝업의 CanvasGroup 설정 및 비활성화
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

    // 특정 팝업을 여는 메서드 (페이드 인)
    public void ShowPopup(string popupName)
    {
        // 다른 모든 팝업은 닫고, 특정 팝업만 페이드 인
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

    // 모든 팝업을 닫는 메서드 (페이드 아웃)
    public void CloseAllPopups()
    {
        foreach (var popup in popups)
        {
            StartCoroutine(FadeOut(popupCanvasGroups[popup.name]));
        }
    }

    // 페이드 인 코루틴
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

    // 페이드 아웃 코루틴
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
