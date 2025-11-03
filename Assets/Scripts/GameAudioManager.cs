using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public float masterVolume = 1f;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    void Awake()
    {
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        if (musicSource == null) { var a = gameObject.AddComponent<AudioSource>(); a.loop = true; musicSource = a; }
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volumeScale * masterVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
}