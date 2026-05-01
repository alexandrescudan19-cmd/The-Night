using UnityEngine;

public class SoldierSound : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip swordAttackSound;

    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayWalkSound()
    {
        audioSrc.PlayOneShot(walkSound);
    }

    public void PlayAttackSound()
    {
        audioSrc.PlayOneShot(swordAttackSound);
    }
}
