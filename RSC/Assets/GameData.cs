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
