using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StatsSØcript : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text todayMaxSpeed;

    [SerializeField]
    private TMPro.TMP_Text todayAvgSpeed;

    [SerializeField]
    private TMPro.TMP_Text todayMaxScore;

    [SerializeField]
    private TMPro.TMP_Text todayAvgScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<GamePlayStat> todayStats = GameData.instance.stats.FindAll(
            (stat) =>
            {
                DateTime today = DateTime.Today;
                DateTime target = DateTime.Parse(stat.startedAt);
                return today.Year == target.Year
                    && today.Month == target.Month
                    && today.Day == target.Day;
            }
        );

        if (todayStats.Any())
        {
            todayMaxSpeed.text = todayStats.Max((stat) => stat.avgSpeed) / 1000 + "초";
            todayMaxScore.color = new Color(255f / 255f, 105f / 255f, 93f / 255f);
            todayAvgSpeed.text = todayStats.Average((stat) => stat.avgSpeed) / 1000 + "초";
            todayAvgSpeed.color = new Color(158f / 255f, 238f / 255f, 255f / 255f);

            todayMaxScore.text = todayStats.Max((stat) => stat.score) + "점";
            todayMaxScore.color = new Color(255f / 255f, 105f / 255f, 93f / 255f);
            todayAvgScore.text = todayStats.Average((stat) => stat.score) + "점";
            todayAvgScore.color = new Color(158f / 255f, 238f / 255f, 255f / 255f);
        }
        else
        {
            todayMaxSpeed.text = "없음";
            todayMaxSpeed.color = new Color(1f, 1f, 1f, .5f);
            todayAvgSpeed.text = "없음";
            todayAvgSpeed.color = new Color(1f, 1f, 1f, .5f);
            todayMaxScore.text = "없음";
            todayMaxScore.color = new Color(1f, 1f, 1f, .5f);
            todayAvgScore.text = "없음";
            todayAvgScore.color = new Color(1f, 1f, 1f, .5f);
        }
    }

    // Update is called once per frame
    void Update() { }
}
