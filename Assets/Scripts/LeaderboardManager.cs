using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;

    public float topTime1;
    public float topTime2;
    public float topTime3;

    public float lastDeath = 0f;

    public void UpdateLeaderboard(float time)
    {
        if (time > topTime1) {
            float temp = topTime1;
            topTime1 = time;

            float temp2 = topTime2;
            topTime2 = temp;

            topTime3 = temp2;

            UpdateText("1. ", text1, topTime1);
            UpdateText("2. ", text2, topTime2);
            UpdateText("3. ", text3, topTime3);
        }
        else if (time > topTime2)
        {
            float temp = topTime2;
            topTime2 = time;

            topTime3 = temp;

            UpdateText("2. ", text2, topTime2);
            UpdateText("3. ", text3, topTime3);
        }
        else if (time > topTime3)
        {
            topTime3 = time;

            UpdateText("3. ", text3, topTime3);
        }
    }

    private void UpdateText(string start, TextMeshProUGUI text, float time)
    {
        string tempo = "";
        int min = (int)time / 60;
        if (min < 10) tempo += "0";
        tempo += min.ToString("N0") + ":";

        float sec = time % 60;
        if (sec < 10) tempo += "0";
        tempo += sec.ToString("F2");

        text.text = start + tempo;
    }
}
