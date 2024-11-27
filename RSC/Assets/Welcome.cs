using UnityEngine;

public class Welcome : MonoBehaviour
{
    private Animation animation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        animation = GetComponent<Animation>();
        GameObject textObj = GameObject.Find("NameText");
        textObj.GetComponent<TMPro.TextMeshProUGUI>().text = "환영해요, " + GameData.instance.playerName + "님!";
    }

    // Update is called once per frame
    private void Update() {
        if (!animation.isPlaying) {
            onAnimationEnd();
        }
    }

    private void onAnimationEnd() {
        GameObject targetObject = GameObject.Find("Panels");
        PanelControl panelControl = targetObject.GetComponent<PanelControl>();
        panelControl.ShowPanel("Home");
    }
}
