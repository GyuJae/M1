using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SoundManager
{
    readonly Dictionary<string, AudioClip> audioClips = new();
    readonly AudioSource[] audioSources = new AudioSource[(int)Define.ESound.Max];
    GameObject soundRoot;

    public void Init()
    {
        if (soundRoot == null)
        {
            soundRoot = GameObject.Find("@SoundRoot");

            if (soundRoot == null)
            {
                soundRoot = new GameObject { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(soundRoot);

                var soundTypeNames = Enum.GetNames(typeof(Define.ESound));
                for (var count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    var go = new GameObject { name = soundTypeNames[count] };
                    audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = soundRoot.transform;
                }

                audioSources[(int)Define.ESound.Bgm].loop = true;
            }
        }
    }

    public void Clear()
    {
        foreach (var audioSource in audioSources)
            audioSource.Stop();

        audioClips.Clear();
    }

    public void Play(Define.ESound type)
    {
        var audioSource = audioSources[(int)type];
        audioSource.Play();
    }

    public void Play(Define.ESound type, string key, float pitch = 1.0f)
    {
        var audioSource = audioSources[(int)type];

        if (type == Define.ESound.Bgm)
        {
            LoadAudioClip(key, audioClip =>
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = audioClip;
                audioSource.Play();
            });
        }
        else
        {
            LoadAudioClip(key, audioClip =>
            {
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            });
        }
    }

    public void Play(Define.ESound type, AudioClip audioClip, float pitch = 1.0f)
    {
        var audioSource = audioSources[(int)type];

        if (type == Define.ESound.Bgm)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Stop(Define.ESound type)
    {
        var audioSource = audioSources[(int)type];
        audioSource.Stop();
    }

    void LoadAudioClip(string key, Action<AudioClip> callback)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(key, out audioClip))
        {
            callback?.Invoke(audioClip);
            return;
        }

        audioClip = Managers.Resource.Load<AudioClip>(key);

        if (audioClips.ContainsKey(key) == false)
            audioClips.Add(key, audioClip);

        callback?.Invoke(audioClip);
    }
}
