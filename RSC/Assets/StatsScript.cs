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

        todayMaxSpeed.text =
            "오늘 최고 속도: "
            + (todayStats.Any() ? todayStats.Max((stat) => stat.avgSpeed) / 1000 + "초" : "없음");
        todayAvgSpeed.text =
            "오늘 평균 속도: "
            + (
                todayStats.Any()
                    ? todayStats.Average((stat) => stat.avgSpeed) / 1000 + "초"
                    : "없음"
            );
        todayMaxScore.text =
            "오늘 최고 점수: "
            + (todayStats.Any() ? todayStats.Max((stat) => stat.score) + "점" : "없음");
        todayAvgScore.text =
            "오늘 평균 점수: "
            + (todayStats.Any() ? todayStats.Average((stat) => stat.score) + "점" : "없음");
    }

    // Update is called once per frame
    void Update() { }
}
