using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField]
    private Animator animator;

    public void SetMaxBattery()
    {
        slider.maxValue = PlayerBehaviour.player.maxBattery;
        slider.value = PlayerBehaviour.player.currentBattery;
    }

    public void UpdateBattery()
    {
        slider.value = PlayerBehaviour.player.currentBattery;
    }
}
