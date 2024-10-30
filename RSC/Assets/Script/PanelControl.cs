using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new(); // �г� ����Ʈ

    // Ư�� �г��� �����ִ� �޼���
    public void ShowPanel(int index)
    {
        HideAllPanels(); // ��� �г� �����
        if (index >= 0 && index < panels.Count)
        {
            panels[index].SetActive(true); // ������ �гθ� Ȱ��ȭ
        }
        else
        {
            Debug.LogWarning("�߸��� �г� �ε����Դϴ�.");
        }
    }

    // ��� �г��� ����� �޼���
    private void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
