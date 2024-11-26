using UnityEngine;

public class PlayButton : MonoBehaviour {
    private PanelControl panelControl;

    void Start() {
        GameObject targetObject = GameObject.Find("Panels");
        panelControl = targetObject.GetComponent<PanelControl>();
    }

    public void HandleClick() {
        // 플레이어 이름이 없을 경우
        if (string.IsNullOrEmpty(GameData.instance.playerName)) {
            // 회원 정보 입력 패널
            panelControl.ShowPanel("Register");
            Debug.Log("Register");
        } else {
            panelControl.ShowPanel("Home");
        }
    }
}
