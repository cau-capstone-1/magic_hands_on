#nullable enable
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public string? playerName
    {
        get { return PlayerPrefs.GetString("name"); }
        set { PlayerPrefs.SetString("name", value); }
    }

    public int playCount
    {
        get
        {
            if (PlayerPrefs.HasKey("playCount") == false)
            {
                return 0;
            }

            return PlayerPrefs.GetInt("playCount");
        }
        set { PlayerPrefs.SetInt("playCount", value); }
    }

    public int score
    {
        get
        {
            if (PlayerPrefs.HasKey("score") == false)
            {
                return 0;
            }

            return PlayerPrefs.GetInt("score");
        }
        set { PlayerPrefs.SetInt("score", value); }
    }

    public string? startedAt
    {
        get { return PlayerPrefs.GetString("startedAt"); }
        set { PlayerPrefs.SetString("startedAt", value); }
    }

    public int healUsageCount
    {
        get
        {
            if (PlayerPrefs.HasKey("healUsageCount") == false)
            {
                return 0;
            }

            return PlayerPrefs.GetInt("healUsageCount");
        }
        set { PlayerPrefs.SetInt("healUsageCount", value); }
    }

    public int blackBirdUsageCount
    {
        get
        {
            if (PlayerPrefs.HasKey("blackBirdUsageCount") == false)
            {
                return 0;
            }

            return PlayerPrefs.GetInt("blackBirdUsageCount");
        }
        set { PlayerPrefs.SetInt("blackBirdUsageCount", value); }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() { }
}
