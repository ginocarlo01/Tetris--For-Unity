using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    AudioClip  sadBgSong;

    [SerializeField] private AudioSource _musicSource, _effectsSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        if(_musicSource!=null) _musicSource.gameObject.SetActive(JsonReadWriteSystem.INSTANCE.playerData.soundOn);
        if(_effectsSource!=null) _effectsSource.gameObject.SetActive(JsonReadWriteSystem.INSTANCE.playerData.soundOn);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)){
            JsonReadWriteSystem.INSTANCE.playerData.soundOn = !_musicSource.isActiveAndEnabled;
            if(_musicSource!=null) _musicSource.gameObject.SetActive(JsonReadWriteSystem.INSTANCE.playerData.soundOn);
            if(_effectsSource!=null) _effectsSource.gameObject.SetActive(JsonReadWriteSystem.INSTANCE.playerData.soundOn);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    

    public void PlaySadBgSong()
    {
        _musicSource.Stop();
        _musicSource.PlayOneShot(sadBgSong);
    }



    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
