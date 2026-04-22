using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    //public TextMeshPro text;
    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.timeSinceLevelLoad;
        string tempo = "";
        int min = (int) time / 60;
        if (min < 10) tempo += "0";
        tempo += min.ToString("N0") + ":";

        float sec = time % 60;
        if (sec < 10) tempo += "0";
        tempo += sec.ToString("F2");

        text.SetText(tempo);
    }
}
