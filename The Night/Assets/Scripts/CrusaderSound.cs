using UnityEngine;

public class MainSound : MonoBehaviour
{
    public AudioClip stepSound;
    public AudioClip hitSound;
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        audioSrc.PlayOneShot(stepSound);
    }

    public void PlayHit()
    {
        audioSrc.PlayOneShot(hitSound);
    }
}
