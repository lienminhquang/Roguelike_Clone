using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sourceEffect;
    public AudioSource sourceBackground;
    public static SoundManager instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerSingle(AudioClip audioClip)
    {
        sourceEffect.PlayOneShot(audioClip);
    }

    public void PlayerRandom(params AudioClip[] audioClips)
    {
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        PlayerSingle(clip);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
