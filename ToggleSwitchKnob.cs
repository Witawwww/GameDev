using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSwitchKnob : MonoBehaviour
{
    [SerializeField] RectTransform knob;
    [SerializeField] float offsetX = 12f;

    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateKnob);
        UpdateKnob(toggle.isOn);
    }

    void UpdateKnob(bool on)
    {
        knob.anchoredPosition = new Vector2(on ? offsetX : -offsetX, 0);
    }
}