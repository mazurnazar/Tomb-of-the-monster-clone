using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioClip move, hit, death, collect, explode;
    [SerializeField] private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "move":
                audioSource.PlayOneShot(move);
                break;
            case "hit":
                audioSource.PlayOneShot(hit);
                break;
            case "death":
                audioSource.PlayOneShot(death);
                break;
            case "collect":
                audioSource.PlayOneShot(collect);
                break;
            case "explode":
                audioSource.PlayOneShot(explode);
                break;
        }
    }

}
