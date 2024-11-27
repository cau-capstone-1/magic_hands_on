using UnityEngine;

public class FirstStartedAtValueBinding : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject currentObject = this.gameObject;
        currentObject.GetComponent<TMPro.TextMeshProUGUI>().text = GameData.instance.startedAt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
