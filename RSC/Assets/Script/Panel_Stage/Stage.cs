using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public int stageNumber; // 스테이지 번호
    public int difficulty;  // 스테이지 난이도 (1: 쉬움, 2: 중간, 3: 어려움)

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
