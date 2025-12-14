using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetControlType : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    void OnEnable()
    {
        string controlStyle = PlayerPrefs.GetString("ControlStyle", "default");

        if (controlStyle == "default")
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }

        UpdateText(controlStyle);
    }

    public void SwitchControlType()
    {
        string controlStyle = PlayerPrefs.GetString("ControlStyle", "default");

        if (controlStyle == "default")
        {
            PlayerPrefs.SetString("ControlStyle", "rotational");
        }
        else
        {
            PlayerPrefs.SetString("ControlStyle", "default");
        }
        UpdateText(PlayerPrefs.GetString("ControlStyle", "default"));
    }

    void UpdateText(string controlStyle)
    {
        if (controlStyle == "default")
        {
            text.text = "Default Controls";
        }
        else
        {
            text.text = "Rotational Controls (more diffiult)";
        }
    }
}
