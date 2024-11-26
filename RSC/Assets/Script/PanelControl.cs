using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new(); // �г� ����Ʈ
    private int currentPanelIndex = 0; // ���� Ȱ��ȭ�� �г� �ε���
    public float slideDuration = 0.5f; // �����̵� �ִϸ��̼� �ð�

    private void Start()
    {
        // ���� �� ��� �г� ����� 0�� �гθ� Ȱ��ȭ
        HideAllPanels();
        if (panels.Count > 0)
        {
            panels[0].SetActive(true);
            panels[0].transform.localPosition = Vector3.zero;
        }
    }

    public void ShowPanel(string name) {
        int index = panels.FindIndex(panel => panel.name == name);

        Debug.Log("ShowPanel: " + name + index);


        if (index < 0 || index >= panels.Count || index == currentPanelIndex)
            return;

        // ���� �г��� �����̵� �ƿ��ϰ� �� �г��� �����̵� ��
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

        // �����̵� ���� ����
        Vector3 fromTargetPosition;
        Vector3 toStartPosition;

        if (toIndex > fromIndex) // �Ʒ����� ���� �����̵�
        {
            fromTargetPosition = new Vector3(0, Screen.height, 0); // ���� �����
            toStartPosition = new Vector3(0, -Screen.height, 0);   // �Ʒ����� ����
        }
        else // ������ �Ʒ��� �����̵�
        {
            fromTargetPosition = new Vector3(0, -Screen.height, 0); // �Ʒ��� �����
            toStartPosition = new Vector3(0, Screen.height, 0);     // ������ ����
        }

        // �� �г� Ȱ��ȭ �� ���� ��ġ ����
        toPanel.SetActive(true);
        toPanel.transform.localPosition = toStartPosition;

        float elapsedTime = 0;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            t = Mathf.SmoothStep(0, 1, t); // �ε巯�� ��¡ ȿ��

            // �����̵� �̵�
            fromPanel.transform.localPosition = Vector3.Lerp(Vector3.zero, fromTargetPosition, t);
            toPanel.transform.localPosition = Vector3.Lerp(toStartPosition, Vector3.zero, t);

            yield return null;
        }

        // �ִϸ��̼��� ���� �� ���� �г� ��Ȱ��ȭ �� ��ġ �ʱ�ȭ
        fromPanel.SetActive(false);
        fromPanel.transform.localPosition = Vector3.zero;
    }
}
