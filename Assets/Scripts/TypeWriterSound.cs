using UnityEngine;
using System.Collections;

public class TypeWriterSound : MonoBehaviour
{
    public static TypeWriterSound Singleton;

    [SerializeField] private AudioClip _typeClip;
    private AudioSource _source;

	// Use this for initialization
	void Awake ()
	{
	    Singleton = this;
	    _source = GetComponent<AudioSource>();
	}

    public void Play()
    {
        _source.PlayOneShot(_typeClip);
    }
}
