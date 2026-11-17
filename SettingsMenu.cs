using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] TMP_Text masterLabel;

    [Header("Subtitles")]
    [SerializeField] Toggle subtitlesToggle;
    [SerializeField] TMP_Text subtitlesLabel;

    [Header("Screen Size")]
    [SerializeField] TMP_Dropdown resolutionDropdown;

    public static bool SubtitlesOn { get; private set; } = true;
    public static event System.Action<bool> OnSubtitlesChanged;

    Resolution[] resolutions;

    void Start()
    {
        // Master volume
        masterSlider.minValue = 0f;
        masterSlider.maxValue = 1f;
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 1f);
        SetMaster(masterSlider.value);
        masterSlider.onValueChanged.AddListener(SetMaster);

        // Subtitles
        subtitlesToggle.isOn = PlayerPrefs.GetInt("Subtitles", 1) == 1;
        SetSubtitles(subtitlesToggle.isOn);
        subtitlesToggle.onValueChanged.AddListener(SetSubtitles);

        // Screen size
        SetupResolutions();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetMaster(float v)
    {
        float db = Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f;
        mixer.SetFloat("MasterVol", db);
        masterLabel.text = Mathf.RoundToInt(v * 100f) + "%";
        PlayerPrefs.SetFloat("MasterVol", v);
    }

    public void SetSubtitles(bool on)
    {
        SubtitlesOn = on;
        subtitlesLabel.text = on ? "On" : "Off";
        PlayerPrefs.SetInt("Subtitles", on ? 1 : 0);
        OnSubtitlesChanged?.Invoke(on);
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        int current = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add($"{resolutions[i].width} × {resolutions[i].height}");
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
                current = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResIndex", current);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        var r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResIndex", index);
    }

    void OnDisable() => PlayerPrefs.Save();
}