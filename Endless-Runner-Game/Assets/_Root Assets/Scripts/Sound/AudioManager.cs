using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public const string FALL_SOUND = "e_fall_on_ground";
    public const string POWERUP_COLLECT = "e_powerup_collect";
    public const string POINT_COLLECT = "e_point_collect";
    public const string START_GAME = "e_start_game";
    public const string STEP_SOUND = "e_step_sound_";
    public const string AMBIENT_SOUND = "m_ambient_sound_";
    
    [SerializeField] private List<Sound> sounds;

    private AudioSource currentAmbientalSound = null;

    private int stepSounds;
    private int ambientSounds;
    
    private void Awake()
    {
        InitializeSingleton();

        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.loop = sound.Loop;

            if (sound.Name.Contains(STEP_SOUND))
            {
                stepSounds++;
            }
            if (sound.Name.Contains(AMBIENT_SOUND))
            {
                ambientSounds++;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(PlayAmbientalSoundsCoroutine());
    }

    private void Update()
    {
        UpdateSoundsVolume();
    }

    public void PlaySound(string soundName)
    {
        var playSound = GetSound(soundName);

        if (playSound == null)
        {
            Debug.LogWarning("Sound " + soundName + " not found!");
        }
        else
        {
            playSound.Source.Play();
        }
    }
    
    public void StopSound(string soundName)
    {
        var playSound = GetSound(soundName);

        if (playSound == null)
        {
            Debug.LogWarning("Sound " + soundName + " not found!");
        }
        else
        {
            playSound.Source.Stop();
        }
    }

    public void PlayRandomStepSound()
    {
        var rng = Random.Range(1, stepSounds);
        var stepSound = $"{STEP_SOUND}{rng}";
        PlaySound(stepSound);
    }
    
    private Sound GetSound(string soundName)
    {
        Sound playSound = null;
        foreach (var sound in sounds)
        {
            if (sound.Name == soundName)
            {
                playSound = sound;
                break;
            }
        }

        return playSound;
    }
    
    private void PlayRandomAmbientalSound()
    {
        if (currentAmbientalSound != null)
        {
            if (currentAmbientalSound.isPlaying)
            {
                return;
            }
        }
        
        var rng = Random.Range(1, ambientSounds);
        var ambientSound = $"{AMBIENT_SOUND}{rng}";
        currentAmbientalSound = GetSound(ambientSound).Source;
        PlaySound(ambientSound);
    }

    private void UpdateSoundsVolume()
    {
        var effectsVolume = UIManager.Instance.GetEffectsSliderValue();
        var musicVolume = UIManager.Instance.GetMusicSliderValue();

        foreach (var sound in sounds)
        {
            if (sound.Name[0] == 'e')
            {
                sound.Source.volume = effectsVolume;
            }
            
            if (sound.Name[0] == 'm')
            {
                sound.Source.volume = musicVolume;
            }
        }
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator PlayAmbientalSoundsCoroutine()
    {
        while (true)
        {
            PlayRandomAmbientalSound();
            yield return null;
        }
    }
}
