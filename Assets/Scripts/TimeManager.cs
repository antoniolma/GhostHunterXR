using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float time;
    public float lastTime = 0f;
    public float totalTime;

    private void Start()
    {
        LeaderboardManager leaderboardManager = GameObject.FindGameObjectWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        lastTime = leaderboardManager.lastDeath;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.timeSinceLevelLoad;

        time = Time.timeSinceLevelLoad - lastTime;
        string tempo = "";
        int min = (int)time / 60;
        if (min < 10) tempo += "0";
        tempo += min.ToString("N0") + ":";

        float sec = time % 60;
        if (sec < 10) tempo += "0";
        tempo += sec.ToString("F2");

        text.SetText(tempo);
    }
}
