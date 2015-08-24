using UnityEngine;
using System.Collections;

public class Cut_Start : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _cam;
    [SerializeField] private Fader _fader;
    [SerializeField] private Flasher _flasherWhite;
    [SerializeField] private Flasher _flasherRed;
    [SerializeField] private AudioClip _headHurt;
    private AudioSource _source;
    private bool _isActive;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    void Start()
    {
        _player.SendMessage("StartCutscene");
    }

    void Update()
    {
        if (!_isActive && Input.GetKeyDown(KeyCode.Return))
        {
            _isActive = true;
            StartCoroutine(PlayCutscene());
        }
    }

    IEnumerator PlayCutscene()
    {
        
        Debug.Log("CutsceneStarted");
        yield return StartCoroutine(_flasherWhite.Flash());
        yield return StartCoroutine(_fader.FadeOut());
        _cam.SendMessage("ChangeTarget", _player.transform);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_fader.FadeIn());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"Ahhh...", 0.15f));
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"My head...", 0.15f));
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"Where am I?", 0.15f));
        yield return new WaitForSeconds(0.55f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"I was...visiting...", 0.15f));
        yield return new WaitForSeconds(0.55f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "...him...", 0.25f));
        yield return new WaitForSeconds(0.575f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"Wasn't I...?", 0.15f));
        yield return new WaitForSeconds(0.55f);
        _player.SendMessage("DisplayText", "");
        yield return StartCoroutine(_flasherWhite.Flash());
        Debug.Log("CutSceneEnded");   
        _player.SendMessage("EndCutscene");
        Destroy(gameObject);
    }

}
