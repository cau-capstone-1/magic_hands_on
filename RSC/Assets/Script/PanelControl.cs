using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new(); // 패널 리스트
    private int currentPanelIndex = 0; // 현재 활성화된 패널 인덱스
    public float slideDuration = 0.5f; // 슬라이드 애니메이션 시간

    private void Start()
    {
        // 시작 시 모든 패널 숨기고 0번 패널만 활성화
        HideAllPanels();
        if (panels.Count > 0)
        {
            panels[0].SetActive(true);
            panels[0].transform.localPosition = Vector3.zero;
        }
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Count || index == currentPanelIndex)
            return;

        // 현재 패널을 슬라이드 아웃하고 새 패널을 슬라이드 인
        StartCoroutine(SlidePanel(currentPanelIndex, index));
        currentPanelIndex = index;
    }

    private void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }

    private IEnumerator SlidePanel(int fromIndex, int toIndex)
    {
        GameObject fromPanel = panels[fromIndex];
        GameObject toPanel = panels[toIndex];

        // 슬라이드 방향 설정
        Vector3 fromTargetPosition;
        Vector3 toStartPosition;

        if (toIndex > fromIndex) // 아래에서 위로 슬라이드
        {
            fromTargetPosition = new Vector3(0, Screen.height, 0); // 위로 사라짐
            toStartPosition = new Vector3(0, -Screen.height, 0);   // 아래에서 등장
        }
        else // 위에서 아래로 슬라이드
        {
            fromTargetPosition = new Vector3(0, -Screen.height, 0); // 아래로 사라짐
            toStartPosition = new Vector3(0, Screen.height, 0);     // 위에서 등장
        }

        // 새 패널 활성화 및 시작 위치 설정
        toPanel.SetActive(true);
        toPanel.transform.localPosition = toStartPosition;

        float elapsedTime = 0;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            t = Mathf.SmoothStep(0, 1, t); // 부드러운 이징 효과

            // 슬라이드 이동
            fromPanel.transform.localPosition = Vector3.Lerp(Vector3.zero, fromTargetPosition, t);
            toPanel.transform.localPosition = Vector3.Lerp(toStartPosition, Vector3.zero, t);

            yield return null;
        }

        // 애니메이션이 끝난 후 이전 패널 비활성화 및 위치 초기화
        fromPanel.SetActive(false);
        fromPanel.transform.localPosition = Vector3.zero;
    }
}
