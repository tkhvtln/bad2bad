using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _sprSoundOn;
    [SerializeField] private Sprite _sprSoundOff;

    [Space]
    [SerializeField] private SoundAudioSource[] _soundAudioSources;

    private bool _isSound;

    public void Init()
    {
        _isSound = GameController.Instance.ControllerSave.DataPlayer.IsSound;
        _image.sprite = _isSound ? _sprSoundOn : _sprSoundOff;
        AudioListener.volume = _isSound ? 1 : 0;
    }

    public void PlaySound(SoundName soundName)
    {
        foreach (SoundAudioSource soundAudioSource in _soundAudioSources)
        {
            if (soundAudioSource.soundName == soundName)
                soundAudioSource.audioSource.PlayOneShot(soundAudioSource.audioClip);
        }
    }

    public void StopSound(SoundName soundName)
    {
        foreach (SoundAudioSource soundAudioSource in _soundAudioSources)
        {
            if (soundAudioSource.soundName == soundName)
                soundAudioSource.audioSource.Stop();
        }
    }

    public bool IsPlaying(SoundName soundName) 
    {
        foreach (SoundAudioSource soundAudioSource in _soundAudioSources)
        {
            if (soundAudioSource.soundName == soundName)
                return soundAudioSource.audioSource.isPlaying;
        }

        return false;
    }

    public void SwitchSound()
    {
        _isSound = !_isSound;
        _image.sprite = _isSound ? _sprSoundOn : _sprSoundOff;

        AudioListener.volume = _isSound ? 1 : 0;

        GameController.Instance.ControllerSave.DataPlayer.IsSound = _isSound;
        GameController.Instance.ControllerSave.Save();
    }

    [System.Serializable]
    public class SoundAudioSource
    {
        public SoundName soundName;
        public AudioSource audioSource;
        public AudioClip audioClip;
    }

    public enum SoundName
    {
        NONE
    }
}
