using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private void Start()
    {
        _slider.onValueChanged.AddListener(val => SoundManager.instance.ChangeMasterVolume(val));
    }
}
