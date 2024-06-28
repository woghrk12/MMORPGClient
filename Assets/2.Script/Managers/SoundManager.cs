using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESound
{ 
    BGM,
    EFFECT,
}

public class SoundManager
{
    #region Variables

    private AudioSource[] audioSources = null;

    private Dictionary<string, AudioClip> effectAudioClipDictionary = new();

    #endregion Variables

    #region Methods

    public void Init()
    {
        string[] names = Enum.GetNames(typeof(ESound));
        int length = names.Length;

        audioSources = new AudioSource[names.Length];

        GameObject root = GameObject.Find("@Sound");

        if (ReferenceEquals(root, null))
        {
            root = new GameObject { name = "@Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);
        }

        for (int index = 0; index < length; index++)
        {
            GameObject go = new GameObject { name = names[index].Substring(0, 1).ToUpper() + names[index].Substring(1).ToLower() };

            audioSources[index] = go.AddComponent<AudioSource>();

            go.transform.SetParent(root.transform);
        }

        audioSources[(int)ESound.BGM].loop = true;
    }

    public void Play(string path, ESound type = ESound.EFFECT, float pitch = 1f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type); 

        if (ReferenceEquals(audioClip, null))
        {
            Debug.LogWarning($"Failed to load audio clip. {path}");
            return;
        }

        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, ESound type = ESound.EFFECT, float pitch = 1f)
    {
        if (ReferenceEquals(audioClip, null)) return;

        switch (type)
        {
            case ESound.BGM:
                PlayBGM(audioClip, pitch);
                break;

            case ESound.EFFECT:
                PlayEffect(audioClip, pitch);
                break;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        effectAudioClipDictionary.Clear();
    }

    private AudioClip GetOrAddAudioClip(string path, ESound type)
    {
        if (!path.Contains("Sounds/"))
        {
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        if (type == ESound.BGM)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);

            if (ReferenceEquals(audioClip, null))
            {
                Debug.LogWarning($"Failed to load audio clip. {path}");
                return null;
            }
        }
        else
        {
            if (effectAudioClipDictionary.TryGetValue(path, out audioClip)) return audioClip;

            audioClip = Managers.Resource.Load<AudioClip>(path);

            if(ReferenceEquals(audioClip, null))
            {
                Debug.LogWarning($"Failed to load audio clip. {path}");
                return null;
            }

            effectAudioClipDictionary.Add(path, audioClip);
        }
        
        return audioClip;
    }

    private void PlayBGM(AudioClip bgmClip, float pitch = 1f)
    {
        AudioSource audioSource = audioSources[(int)ESound.BGM];

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.pitch = pitch;
        audioSource.clip = bgmClip;

        audioSource.Play();
    }

    private void PlayEffect(AudioClip effectClip, float pitch = 1f)
    {
        AudioSource audioSource = audioSources[(int)ESound.EFFECT];

        audioSource.pitch = pitch;

        audioSource.PlayOneShot(effectClip);
    }

    #endregion Methods
}
