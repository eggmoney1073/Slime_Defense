using UnityEngine;

public class SoundManager : SingletonDontDestroyOnLoad<SoundManager>
{
    public enum SFXType
    {
        ButtonClick,
        PlayerShoot,
        EnemyHit,
        EnemyDeath,
        GameOver,
        GameClear
    }

    public enum BGMType
    {
        MainMenu,
        Gameplay
    }

    [SerializeField] private AudioSource _BGMAudioSource;
    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private SoundClipDatabase _soundClipDatabase;

    public float BGMVolume
    {
        get => _BGMAudioSource.volume;
        set => _BGMAudioSource.volume = value;
    }

    public float SFXVolume
    {
        get => _SFXAudioSource.volume;
        set => _SFXAudioSource.volume = value;
    }

    #region [ Effect Sound ]

    public void PlaySFX(SFXType clipType, float volumeScale = 1f)
    {
        AudioClip clip = _soundClipDatabase.GetSFXClip(clipType);
        if (clip == null)
        {
            Debug.LogWarning($"SFX {clipType} 클립이 존재하지 않음.", this);
            return;
        }
        _SFXAudioSource.PlayOneShot(clip, volumeScale);
    }

    public void StopSFX()
    {
        _SFXAudioSource.Stop();
    }

    #endregion

    #region [ BGM ]

    public void PlayBGM(BGMType clipType)
    {
        AudioClip clip = _soundClipDatabase.GetBGMClip(clipType);
        if (clip == null)
        {
            Debug.LogWarning($"BGM {clipType} 클립이 존재하지 않음.", this);
            return;
        }

        if (_BGMAudioSource.clip == clip)
            return;

        _BGMAudioSource.clip = clip;
        _BGMAudioSource.loop = true;
        _BGMAudioSource.Play();
    }

    public void StopBGM()
    {
        _BGMAudioSource.Stop();
        _BGMAudioSource.clip = null;
    }

    #endregion
}
