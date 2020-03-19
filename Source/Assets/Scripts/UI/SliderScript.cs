using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public int PlayerIndex;
    public SliderType sliderType;
    private Slider mainSlider;

    public void Start()
    {
        // Get slider component
        mainSlider = GetComponent<Slider>();

        // Add a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate { SliderValueChange(); });
    }

    public void SliderValueChange()
    {
        switch (sliderType)
        {
            case SliderType.Hat:
                LevelManager.Instance.ChangeHat(PlayerIndex, (int)mainSlider.value);
                break;
            case SliderType.Watergun:
                LevelManager.Instance.ChangeWatergun(PlayerIndex, (int)mainSlider.value);
                break;
            default:
                break;
        }
    }
}

public enum SliderType
{
    Hat,
    Watergun
}
