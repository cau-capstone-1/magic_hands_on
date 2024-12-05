using UnityEngine;

public class TopBarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject userNameObj = GameObject.Find("UserName");
        userNameObj.GetComponent<TMPro.TextMeshProUGUI>().text =
            GameData.instance.playerName + "님 환영합니다!";
    }

    // Update is called once per frame
    void Update() { }
}
