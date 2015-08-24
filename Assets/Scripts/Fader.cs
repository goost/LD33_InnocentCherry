using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

    private Animator _anim;
    private bool _isFading;

	// Use this for initialization
	void Awake ()
	{
	    _anim = GetComponent<Animator>();
	    _isFading = false;
	}


    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fading("FadeIn"));
        
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fading("FadeOut"));
    }

    private IEnumerator Fading(string trigger)
    {
        _isFading = true;
        _anim.SetTrigger(trigger);
        while (_isFading) yield return null;
    }
	// Update is called once per frame
	void Update () {
	
	}

    void AnimationComplete()
    {
        _isFading = false;
    }
}
