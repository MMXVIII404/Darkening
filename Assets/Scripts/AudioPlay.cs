using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    public void PlaySound()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("AudioSource ªÚ AudioClip Œ¥…Ë÷√£°");
        }
    }
}
