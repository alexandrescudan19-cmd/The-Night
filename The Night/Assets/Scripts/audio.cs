using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks;

    private int currentTrackIndex = -1;

    void Start()
    {
        if (musicTracks.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("MusicPlayer: No tracks or AudioSource assigned.");
            return;
        }

        ShuffleTracks();
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void ShuffleTracks()
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            int rnd = Random.Range(i, musicTracks.Length);
            (musicTracks[i], musicTracks[rnd]) = (musicTracks[rnd], musicTracks[i]);
        }
    }

    void PlayNextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= musicTracks.Length)
        {
            ShuffleTracks();
            currentTrackIndex = 0;
        }

        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
            audioSource.volume = volume;
    }
}
