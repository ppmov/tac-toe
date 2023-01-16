using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    [SerializeField]
    private string disabledValue;
    [SerializeField]
    private string enabledValue;

    [SerializeField]
    private TMPro.TMP_Text text;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnChangeMode);

        text.text = disabledValue;
    }

    public void OnChangeMode(float value)
    {
        text.text = value == 1 ? enabledValue : disabledValue;
    }
}
