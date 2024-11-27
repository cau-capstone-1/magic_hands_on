using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MissionScript : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_FontAsset font;

    private Mission[] missions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mission healItemTwiceMission = new Mission(
            this.transform,
            "회복 아이템 2회 사용",
            () => GameData.instance.healUsageCount >= 2,
            font
        );
        Mission tenBirdKillMission = new Mission(
            this.transform,
            "검은색 새 10마리 죽이기",
            () => GameData.instance.blackBirdUsageCount >= 10,
            font
        );
        Mission twoGamePlayMission = new Mission(
            this.transform,
            "게임 2회 플레이하기",
            () => GameData.instance.playCount >= 2,
            font
        );

        healItemTwiceMission.position = new Vector2(0, 0);
        tenBirdKillMission.position = new Vector2(0, -160);
        twoGamePlayMission.position = new Vector2(0, -320);

        missions = new Mission[] { healItemTwiceMission, tenBirdKillMission, twoGamePlayMission };
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < missions.Length; i++)
        {
            missions[i].OnUpdate();
        }
    }
}

class Mission
{
    public GameObject gameObject;

    public Vector2 position
    {
        get { return gameObject.GetComponent<RectTransform>().anchoredPosition; }
        set { gameObject.GetComponent<RectTransform>().anchoredPosition = value; }
    }

    public TMPro.TextMeshProUGUI textComponent
    {
        get { return gameObject.GetComponent<TMPro.TextMeshProUGUI>(); }
    }

    public Boolean isComplete
    {
        get { return this.completeCondition(); }
    }

    private Func<Boolean> completeCondition;

    public Mission(
        Transform parent,
        string text,
        Func<Boolean> completeCondition,
        TMPro.TMP_FontAsset font = null
    )
    {
        this.completeCondition = completeCondition;
        this.gameObject = new GameObject(text);
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.parent = parent;
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1200, 200);

        TMPro.TextMeshProUGUI textComponent = gameObject.AddComponent<TMPro.TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 110;
        textComponent.color = new Color(0.3254901961f, 0.3254901961f, 0.3254901961f);
        textComponent.font = font;
    }

    public void OnUpdate()
    {
        if (isComplete)
        {
            textComponent.color = new Color(0.3254901961f, 0.3254901961f, 0.3254901961f, 0.5f);
            textComponent.text = "<s>" + textComponent.text + "</s>";
        }
    }
}
