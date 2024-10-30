using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new(); // 패널 리스트

    // 특정 패널을 보여주는 메서드
    public void ShowPanel(int index)
    {
        HideAllPanels(); // 모든 패널 숨기기
        if (index >= 0 && index < panels.Count)
        {
            panels[index].SetActive(true); // 선택한 패널만 활성화
        }
        else
        {
            Debug.LogWarning("잘못된 패널 인덱스입니다.");
        }
    }

    // 모든 패널을 숨기는 메서드
    private void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
