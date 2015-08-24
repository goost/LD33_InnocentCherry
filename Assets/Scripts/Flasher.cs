using UnityEngine;
using System.Collections;

public class Flasher : MonoBehaviour
{

    private Animator _anim;
    private bool _isFlashing;

    // Use this for initialization
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _isFlashing = false;
    }


    public IEnumerator Flash()
    {
        _isFlashing = true;
        _anim.SetTrigger("Flash");
        while (_isFlashing) yield return null;
    }

    void AnimationComplete()
    {
        _isFlashing = false;
    }
}
