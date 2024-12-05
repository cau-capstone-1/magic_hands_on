#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct GamePlayStat
{
    public int score;
    public string startedAt;

    public GamePlayStat(int score, DateTime startedAt)
    {
        this.score = score;
        this.startedAt = startedAt.ToString();
    }

    public object Clone()
    {
        return new GamePlayStat(this.score, DateTime.Parse(this.startedAt));
    }
}

[Serializable]
public class GamePlayStats
{
    public List<GamePlayStat> stats;
}

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public string? playerName
    {
        get { return PlayerPrefs.GetString("name"); }
        set { PlayerPrefs.SetString("name", value); }
    }

    private List<GamePlayStat> _stats;

    public List<GamePlayStat> stats
    {
        get
        {
            if (_stats == null)
            {
                if (PlayerPrefs.HasKey("stats"))
                {
                    string json = PlayerPrefs.GetString("stats");
                    _stats = JsonUtility.FromJson<GamePlayStats>(json).stats;
                }
                else
                {
                    _stats = new List<GamePlayStat>();
                }
            }
            return _stats;
        }
        set
        {
            _stats = value;
            SaveStats();
        }
    }

    public string serialPort
    {
        get { return PlayerPrefs.GetString("serialPort"); }
        set { PlayerPrefs.SetString("serialPort", value); }
    }

    private void SaveStats()
    {
        var wrapper = new GamePlayStats { stats = _stats };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("stats", json);
        PlayerPrefs.Save();
    }

    public void AddStat(GamePlayStat stat)
    {
        stats.Add(stat);
        SaveStats();
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
