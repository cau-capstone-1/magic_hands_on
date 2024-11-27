using UnityEngine;

public class PlayCountValueBinding : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameObject currentObject = this.gameObject;
        currentObject.GetComponent<TMPro.TextMeshProUGUI>().text = GameData.instance.playCount.ToString() + '회';
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
