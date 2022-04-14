using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name => name;
    public AudioClip Clip => audioClip;
    public float Volume => volume;
    public bool Loop => loop;

    public AudioSource Source
    {
        get => audioSource;
        set => audioSource = value;
    }

    [SerializeField] private string name;
    [SerializeField] private AudioClip audioClip;

    [Range(0f, 1f)] 
    [SerializeField] private float volume = 0.5f;

    [SerializeField] private bool loop = false;

    private AudioSource audioSource;
}
