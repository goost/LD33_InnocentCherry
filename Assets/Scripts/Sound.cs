using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour
{

    [SerializeField] private AudioClip _footLeft;
    [SerializeField] private AudioClip _footRight;
    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    void PlayFootLeft()
    {
        _source.PlayOneShot(_footLeft);
    }

    void PlayFootRight()
    {
        _source.PlayOneShot(_footRight);
    }
}
