using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _music;

    [SerializeField]
    private List<Sprite> _soundOnOffSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> _musicOnOffSprites = new List<Sprite>();

    private bool _isSoundActive = true;

    private bool _isMusicActive = true;

    [SerializeField]
    private Image _musicImage;

    [SerializeField]
    private Image _soundImage;

    void Start()
    {
        CheckAndSetIfMusicAndSoundActive();
    }


    public void SoundPressed()
    {
        _isSoundActive = !_isSoundActive;
        if (_isSoundActive)
        {
            _soundImage.sprite = _soundOnOffSprites[1];
            PlayerPrefs.SetInt("isSoundActive", 1);
        }
        else
        {
            _soundImage.sprite = _soundOnOffSprites[0];
            PlayerPrefs.SetInt("isSoundActive", 0);
        }
    }

    public void MusicPressed()
    {
        _isMusicActive = !_isMusicActive;
        if (_isMusicActive)
        {
            _musicImage.sprite = _musicOnOffSprites[1];
            _music.enabled = true;
            PlayerPrefs.SetInt("isMusicActive", 1);
        }
        else
        {
            _musicImage.sprite = _musicOnOffSprites[0];
            _music.enabled = false;
            PlayerPrefs.SetInt("isMusicActive", 0);
        }
    }

    private void CheckAndSetIfMusicAndSoundActive()
    {
        if (PlayerPrefs.GetInt("isSoundActive", 1) == 1)
        {
            _soundImage.sprite = _soundOnOffSprites[1];
            _isSoundActive = true;
        }
        else
        {
            _soundImage.sprite = _soundOnOffSprites[0];
            _isSoundActive = false;
        }

        if (PlayerPrefs.GetInt("isMusicActive", 1) == 1)
        {
            _isMusicActive = true;
            _musicImage.sprite = _musicOnOffSprites[1];
            _music.enabled = true;
        }
        else
        {
            _musicImage.sprite = _musicOnOffSprites[0];
            _music.enabled = false;
            _isMusicActive = false;
        }
    }
}
