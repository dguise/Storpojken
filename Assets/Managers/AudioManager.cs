using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private AudioSource musicPlayer;
    private AudioSource musicPlayerTwo;
    private AudioSource burningLoop;

    private List<AudioClip> songs = new List<AudioClip>();
    private List<AudioClip> sounds = new List<AudioClip>();

    private List<AudioSource> tempSounds = new List<AudioSource>();

    private int currentTrack = 0;
    private int currentTrackTwo = 0;

    public static string[] Sounds = {
            "boost"         // 0
            , "burning"     // 1
            , "chargeup"    // 2
            , "exp_small1"  // 3
            , "exp_small2"  // 4
            , "exp_small3"  // 5
            , "exp_small4"  // 6
            , "exp1"        // 7
            , "exp2"        // 8
            , "exp3"        // 9
            , "impact1"     // 10
            , "impact2"     // 11
            , "impact3"     // 12
            , "astrocollect" // 13
            , "pickup_fc"   // 14
            , "pickup_fcsuper" // 15
        };

    public static string[] Songs = { "speep_danger", "speep_normal" };

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayerTwo = gameObject.AddComponent<AudioSource>();
        
        foreach (string song in Songs)
        {
            songs.Add((Resources.Load<AudioClip>(song)));
        }
        
        foreach (string sound in Sounds)
        {
            sounds.Add(Resources.Load<AudioClip>(sound) as AudioClip);
        }

        // When songs added:
        LoopSongOne(0);
        LoopSongTwo(1);

        burningLoop = gameObject.AddComponent<AudioSource>();
        burningLoop.clip = sounds[1];
        burningLoop.loop = true;
        burningLoop.volume = 0;
        burningLoop.Play();
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += StopAllSounds;
    }

    public void PlaySound(int sound, float pitch = 1f)
    {
        AudioSource effect = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        tempSounds.Add(effect);
        effect.clip = sounds[Mod(sound, sounds.Count)];
        if (sound == 0)
        {
            effect.volume = 0.5f;
        }
        else if (sound == 14) 
        {
            effect.volume = 0.4f;
        } else if (sound == 15) {
            effect.volume = 0.2f;
        }
        effect.Play();
        Destroy(effect, effect.clip.length + 0.2f);
    }

    public void PlayRandomize(float pitch, params int[] sound)
    {
        AudioSource effect = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        tempSounds.Add(effect);
        int random = Random.Range(0, sound.Length);
        effect.clip = sounds[Mod(sound[Mod(random, sound.Length)], sounds.Count)];
        effect.pitch = Random.Range(1 - pitch, 1 + pitch);
        effect.Play();
        Destroy(effect, effect.clip.length);
    }

    public void LoopSongOne(int number)
    {
        currentTrack = number;
        musicPlayer.loop = true;
        musicPlayer.clip = songs[number];
        musicPlayer.volume = 0;
        musicPlayer.Play();
    }

    public void LoopSongTwo(int number)
    {
        currentTrackTwo = number;
        musicPlayerTwo.loop = true;
        musicPlayerTwo.clip = songs[number];
        musicPlayerTwo.Play();
    }

    private int Mod(int x, int m)
    {
        return ((x % m) + m) % m;
    }

    // Special stuffs

    private bool shouldCharge = true;
    public IEnumerator PlayChargingSound(int sound = 2)
    {
        AudioSource effect = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        tempSounds.Add(effect);
        effect.clip = sounds[Mod(sound, sounds.Count)];
        effect.loop = true;
        effect.pitch = 0.1f;
        effect.volume = 0.2f;
        effect.Play();

        while (shouldCharge)
        {
            if (effect.pitch < 1.2f)
            {
                effect.pitch += Time.deltaTime * 0.5f;
                effect.volume += Time.deltaTime * 0.5f;
            }
            yield return new WaitForEndOfFrame();
        }
        shouldCharge = true;
        effect.Stop();
        Destroy(effect);
        PlaySound(0);
    }
    public void StopCharging()
    {
        shouldCharge = false;
    }

    public void ChangeBurningVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        burningLoop.volume = volume;
    }

    public void ChangeMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        musicPlayer.volume = volume * 0.8f;
        musicPlayerTwo.volume = (1 - volume * 0.8f);
    }

    public void StopAllSounds(Scene sc1, Scene sc2)
    {
        if (burningLoop != null)
            burningLoop.volume = 0;
        if (musicPlayer != null && musicPlayerTwo != null)
            ChangeMusicVolume(0);

        foreach (var sound in tempSounds)
        {
            Destroy(sound);
        }
    }
}