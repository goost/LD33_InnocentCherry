using UnityEngine;
using System.Collections;

public class Cut_JumpEnd : MonoBehaviour
{

    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _endText;
    [SerializeField] private Fader _faderRed;
    [SerializeField] private AudioClip _scream;
    private AudioSource _source;
    private bool _isActive;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!_isActive) return;
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive) return;
        StartCoroutine(Cutscene(collision.gameObject));
    }

    IEnumerator Cutscene(GameObject player)
    {
        _isActive = true;
        _source.PlayOneShot(_scream);
        _cam.SendMessage("ChangeTarget", transform);
        yield return StartCoroutine(_faderRed.FadeOut());
        player.SendMessage("StartCutscene");
        _cam.SendMessage("ChangeTarget", _target);
        _endText.SetActive(true);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(_faderRed.FadeIn());
    }

 
}
