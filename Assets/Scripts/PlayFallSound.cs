using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFallSound : MonoBehaviour
{
    private AudioSource particleAudio;
    public AudioClip fallSound;

    // Start is called before the first frame update
    void Start()
    {
        particleAudio = gameObject.GetComponent<AudioSource>();
        particleAudio.PlayOneShot(fallSound, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
