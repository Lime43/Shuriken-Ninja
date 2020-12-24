using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton class 
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("SFX Audio")]
    public AudioClip[] spark;
    [Header("Theme Audio")]
    public AudioClip adiTheme;

    private void Start()
    {
        PlaySound(this.gameObject, adiTheme,.3f);
    }

    public void PlaySound(GameObject targetObj,AudioClip adiClip,float volume)
    {
        if (!targetObj.GetComponent<AudioSource>())
            targetObj.AddComponent<AudioSource>();

        AudioSource adiSrc = targetObj.GetComponent<AudioSource>();

        adiSrc.clip = adiClip;
        adiSrc.volume = volume;
        adiSrc.Play();
    }
}
