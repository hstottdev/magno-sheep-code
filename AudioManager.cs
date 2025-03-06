using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

enum SoundType
{
    music,
    sound
}

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// The current instance of the AudioManager
    /// </summary>
    public static AudioManager inst;
    public AudioSource music;
    [HideInInspector] public float defaultMusicVolume;
    [Header("Sound Effects")]
    public GameObject soundPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            inst = this;
        }
        defaultMusicVolume = music.volume;
    }

    public static void SetMusic(AudioClip song,float volume = 0.5f, bool looping = true)
    {
        if(inst.music.clip != song)
        {
            inst.music.clip = song;
            inst.music.Play();
            inst.music.loop = looping;
            inst.music.volume = volume;
        }
    }
    /// <summary>
    /// change the music track and set as the same playback position through as the previous track
    /// </summary>
    public static void SwitchMusicSamePosition(AudioClip song)
    {
        float timePassed = 0;

        if (inst.music.clip != null)
        {
            timePassed = inst.music.time;
        }

        SetMusic(song);
        inst.music.time = timePassed;
    }

    public static void PlaySound(string soundFile,float pitch = 1, float volume = 1,float delay = 0)
    {
        try
        {
            GameObject sfx = Instantiate(inst.soundPrefab, inst.transform);
            AudioSource source = sfx.GetComponent<AudioSource>();
            source.clip = Resources.Load<AudioClip>(soundFile);
            source.pitch = pitch;
            source.volume = volume;

            source.PlayDelayed(delay);

            Destroy(sfx, 10);
        }
        catch
        {
            Debug.LogWarning("Couldn't play sound: " + soundFile);
        }

    }

    void OnApplicationQuit()
    {
        inst = null;
    }
}
