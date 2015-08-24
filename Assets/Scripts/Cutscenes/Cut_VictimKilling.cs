using UnityEngine;
using System.Collections;

public class Cut_VictimKilling : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _victim;
    [SerializeField] private GameObject _music;
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _cam;
    [SerializeField] private Fader _faderRed;
    [SerializeField] private Flasher _flasherWhite;
    [SerializeField] private Flasher _flasherRed;
    [SerializeField] private Sprite _markHuman;
    [SerializeField] private Sprite _markGlue;
    [SerializeField] private Sprite _blood;
    [SerializeField] private Transform _textScreen;
    [SerializeField] private GameObject _endText;
    [SerializeField] private AudioClip _gueAttack;
    [SerializeField] private AudioClip _gueSound;
    [SerializeField] private AudioClip _cherryAttack;
    [SerializeField] private AudioClip _cherryOnMark;
    [SerializeField] private AudioClip _markScream;
    [SerializeField] private AudioClip _cherryScream;
    private AudioSource _source;
    private bool _isActive;
    private bool _isDecided;
    private bool _choosedAttack;
    private bool _restart;


    void Awake()
    {
        _isActive = false;
        _isDecided = false;
        _choosedAttack = false;
        _restart = false;
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_isActive) return;
        if (!_isDecided && Input.GetKeyDown(KeyCode.E))
        {
            _isDecided = true;
            _choosedAttack = true;
        }
        if (!_isDecided && Input.GetKeyDown(KeyCode.Q))
        {
            _isDecided = true;
        }
        if (_restart && Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive) return;
        StartCoroutine(CutScene());
    }

    IEnumerator CutScene()
    {
        
        Debug.Log("CutSceneStarted");
        var vrb = _victim.GetComponent<Rigidbody2D>();
        var prb = _player.GetComponent<Rigidbody2D>();
        _player.SendMessage("StartCutscene");
        
        _cam.SendMessage("ChangeTarget", _target);

        //Glue talks
        _source.PlayOneShot(_gueSound);
        yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "A human, here in this realm?\nHow funny.", 0.1f));
        yield return new WaitForSeconds(1f);
        _source.PlayOneShot(_gueSound);
        yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Your end is near, mortal.", 0.1f));
        yield return new WaitForSeconds(1f);
        _victim.SendMessage("DisplayText", "");
        //Glue attacks
        _music.SetActive(true);
        vrb.MovePosition(new Vector2(vrb.position.x-0.95f, vrb.position.y));
        _source.PlayOneShot(_gueAttack);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Ahh!", 0.01f));
        yield return new WaitForSeconds(0.5f); 
        //Cherry goes back
        _cam.SendMessage("ChangeTarget", _player.transform);
        prb.MovePosition(new Vector2(prb.position.x - 0.1f, prb.position.y));
        _player.SendMessage("DisplayText", "");
        yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Yes, you are going to die here!", 0.05f));
        yield return new WaitForSeconds(1f);
        _victim.SendMessage("DisplayText", "");
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "No...no...", 0.15f));
        yield return new WaitForSeconds(1f);
        _isActive = true;
        //Decision Time
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Flee (Q)?\nFight (E)?"));
        while (!_isDecided) yield return null;
        _music.SetActive(false);
        if (_choosedAttack)
        {
            //Cherry Attacks
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "I do'nt want to die!!!", 0.01f));
            prb.MovePosition(new Vector2(prb.position.x + 0.15f, prb.position.y));
            _source.PlayOneShot(_cherryAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Huah! Die! Die!", 0.01f));
            _source.PlayOneShot(_cherryAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return new WaitForSeconds(0.5f);
            _source.PlayOneShot(_cherryAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            //Mark as Human 
            _victim.GetComponent<SpriteRenderer>().sprite = _markHuman;
            _player.SendMessage("DisplayText", "");
            yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Wha..? Cher...?", 0.1f));
            yield return new WaitForSeconds(0.25f);
            _source.PlayOneShot(_cherryOnMark);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Ahh..uhhu..", 0.1f));
            yield return new WaitForSeconds(0.25f);
            _victim.SendMessage("DisplayText", "");
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Mark?!", 0.1f));
            yield return new WaitForSeconds(0.5f);
            //Mark to Bloody Glue
            _victim.SendMessage("DisplayText", "");
            _victim.GetComponent<SpriteRenderer>().sprite = _markGlue;
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Ahhhh! Die!", 0.1f));
            _source.PlayOneShot(_cherryOnMark);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return new WaitForSeconds(0.5f);
            _source.PlayOneShot(_cherryOnMark);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return new WaitForSeconds(0.5f);
            _source.PlayOneShot(_cherryOnMark);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return StartCoroutine(_flasherRed.Flash());
            _player.SendMessage("DisplayText", "");
            //Fade Out
            //yield return new WaitForSeconds(0.5f);
            _source.PlayOneShot(_markScream);
            yield return StartCoroutine(_faderRed.FadeOut());
            _victim.GetComponent<SpriteRenderer>().sprite = _blood;
            vrb.MovePosition(new Vector2(vrb.position.x + 0.3f, vrb.position.y - 0.2f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(_faderRed.FadeIn());           
           // yield return StartCoroutine(_faderRed.FadeIn());
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "It's...it's over...?", 0.25f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(_flasherWhite.Flash());

            _player.SendMessage("DisplayText", "");
            Debug.Log("CutSceneEnded");
            _isActive = false;
            _player.SendMessage("EndCutscene");
            Destroy(gameObject);
        }
        else
        {
            //Cherry dies
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "I am scared.", 0.1f));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "I want to run.", 0.1f));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "But my legs are frozen!", 0.01f));
            yield return new WaitForSeconds(0.5f);
            _player.SendMessage("DisplayText", "");
            yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Muhaha", 0.1f));
            yield return new WaitForSeconds(0.5f);
            _victim.SendMessage("DisplayText", "");
            vrb.MovePosition(new Vector2(vrb.position.x - 0.15f, vrb.position.y));
            _source.PlayOneShot(_gueAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            _source.PlayOneShot(_gueAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Ahhh."));
            _source.PlayOneShot(_gueAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return StartCoroutine(_flasherRed.Flash());
            _source.PlayOneShot(_gueAttack);
            yield return StartCoroutine(_flasherRed.Flash());
            yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Uhhh."));
            _player.SendMessage("DisplayText", "");
            //Fade Out
            _source.PlayOneShot(_cherryScream);
            yield return StartCoroutine(_faderRed.FadeOut());
            _cam.SendMessage("ChangeTarget", _textScreen);
            yield return new WaitForSeconds(2f);
            _endText.SetActive(true);
            yield return StartCoroutine(_faderRed.FadeIn());
            _restart = true;
        }
    }

}
