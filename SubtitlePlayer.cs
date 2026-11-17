using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class SubtitlePlayer : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public string speaker;
        [TextArea] public string text;
        public float start;
        public float end;
    }

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] float timeOffset = 0f; // + shows subtitles later, - shows them earlier

    Line[] lines = new Line[]
    {
        new Line { speaker = "Mika",   start = 0f,  end = 9f,  text = "Okay, guys. The presentation is tomorrow. Kindly pa-ayos ako ng lahat ng need ayusin, and Eli? Pwede’ng paki-practice na ng mga explanation na need bukas?" },
        new Line { speaker = "Eli",    start = 10f, end = 11f, text = "Sure!" },
        new Line { speaker = "Mika",   start = 17f, end = 19f, text = "Nasa’n pala si James?" },
        new Line { speaker = "Eli",    start = 20f, end = 27f, text = "Ah, si James, nakita ko siya lumabas kanina, baka nag-cr lang." },
        new Line { speaker = "Nathan", start = 28f, end = 34f, text = "Pwede’ng pahiram muna ng flash drive? I-check ko lang if okay na." },
        new Line { speaker = "Mika",   start = 35f, end = 37f, text = "Go ahead." },
        new Line { speaker = "Mika",   start = 40f, end = 46f, text = "Lalabas muna ako ah? Nagugutom na kasi ako eh. Sama ka ba, Eli?" },
        new Line { speaker = "Eli",    start = 47f, end = 51f, text = "Tapusin ko muna ’tong script natin. Hindi rin ako gutom e. Anyway, take care!" },
        new Line { speaker = "Mika",   start = 52f, end = 55f, text = "Okay. Good luck!" },
        new Line { speaker = "Mika",   start = 60f, end = 63f, text = "Eli, nasa’n na yung flash drive?" },
        new Line { speaker = "Eli",    start = 64f, end = 67f, text = "Hindi ba’t hiniram ni Nathan?" },
        new Line { speaker = "Eli",    start = 68f, end = 71f, text = "Uy, Nathan? Saan mo ba dinala?" },
        new Line { speaker = "Nathan", start = 71f, end = 75f, text = "Huh? Nilagay ko diyan sa may table ah?" },
        new Line { speaker = "Eli",    start = 77f, end = 84f, text = "Oh, nasa’n na ngayon? I did not notice na nagbaba ka ng flash drive diyan sa may table." },
        new Line { speaker = "Nathan", start = 84f, end = 94f, text = "Malay ko?! Parang hindi mo ako nakitang bumalik, Eli ah?" },
    };

    void Update()
    {
        if (!SettingsMenu.SubtitlesOn || videoPlayer == null || !videoPlayer.isPlaying)
        {
            if (!SettingsMenu.SubtitlesOn) subtitleText.text = "";
            return;
        }

        double t = videoPlayer.time - timeOffset;
        string show = "";

        foreach (var line in lines)
        {
            if (t >= line.start && t <= line.end)
            {
                show = $"{line.speaker}: {line.text}";
                break;
            }
        }

        subtitleText.text = show;
    }
}