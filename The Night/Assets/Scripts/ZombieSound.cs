using UnityEngine;

public class ZombieSound : MonoBehaviour
{
    public AudioClip growlSound;
    public AudioClip attackSound;
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayGrowl()
    {
        audioSrc.PlayOneShot(growlSound);
    }

    public void PlayAttack()
    {
        audioSrc.PlayOneShot(attackSound);
    }
}
