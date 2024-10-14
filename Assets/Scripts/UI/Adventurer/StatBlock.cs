using UnityEngine;
using UnityEngine.UI;

public class StatBlock : MonoBehaviour
{
    public StatName statType;
    public Slider statSlider;

    public void UpdateStatSliderValue(Adventurer adventurer)
    {
        statSlider.value = adventurer.Char_Stats.Get(statType) / Stats.GetMaxValue();
    }

    public void SetSliderValue(float value)
    {
        statSlider.value = value;
    }
}