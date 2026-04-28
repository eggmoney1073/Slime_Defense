using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipDatabase : MonoBehaviour
{
    [Serializable]
    public struct SFXClip
    {
        public SoundManager.SFXType Type;
        public AudioClip Clip;
    }

    [Serializable]
    public struct BGMClip
    {
        public SoundManager.BGMType Type;
        public AudioClip Clip;
    }

    [Header("BGM")]
    [SerializeField] private BGMClip[] _bgmClips;

    [Header("SFX")]
    [SerializeField] private SFXClip[] _sfxClips;

    private Dictionary<SoundManager.BGMType, AudioClip> _bgmClipDict;
    private Dictionary<SoundManager.SFXType, AudioClip> _sfxClipDict;

    private bool _isInitialized = false;

    public AudioClip GetBGMClip(SoundManager.BGMType type)
    {
        Init();
        return _bgmClipDict.TryGetValue(type, out AudioClip clip) ? clip : null;
    }

    public AudioClip GetSFXClip(SoundManager.SFXType type)
    {
        Init();
        return _sfxClipDict.TryGetValue(type, out AudioClip clip) ? clip : null;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_isInitialized)
        {
            return;
        }

        _bgmClipDict = new Dictionary<SoundManager.BGMType, AudioClip>(_bgmClips.Length);
        _sfxClipDict = new Dictionary<SoundManager.SFXType, AudioClip>(_sfxClips.Length);

        foreach (var bgmClip in _bgmClips)
        {
            if (bgmClip.Clip == null)
                Debug.LogWarning($"BGM {bgmClip.Type} 클립이 비어있음(null).", this);

            if (!_bgmClipDict.TryAdd(bgmClip.Type, bgmClip.Clip))
                Debug.LogWarning($"BGM {bgmClip.Type} 중복!", this);
        }

        foreach (var sfxClip in _sfxClips)
        {
            if (sfxClip.Clip == null)
                Debug.LogWarning($"SFX {sfxClip.Type} 클립이 비어있음(null).", this);

            if (!_sfxClipDict.TryAdd(sfxClip.Type, sfxClip.Clip))
                Debug.LogWarning($"SFX {sfxClip.Type} 중복!", this);
        }

        _isInitialized = true;
    }
}
