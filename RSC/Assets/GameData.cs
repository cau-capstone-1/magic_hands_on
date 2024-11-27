#nullable enable
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public string? playerName {
        get {
            return PlayerPrefs.GetString("name");
        }
        set {
            PlayerPrefs.SetString("name", value);
        }
    }

    public int playCount {
        get {
            if (PlayerPrefs.HasKey("playCount") == false) {
                return 0;
            }
            
            return PlayerPrefs.GetInt("playCount");
        }
        set {
            PlayerPrefs.SetInt("playCount", value);
        }
    }

    public string? startedAt {
        get {
            return PlayerPrefs.GetString("startedAt");
        }
        set {
            PlayerPrefs.SetString("startedAt", value);
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {

    }
}
