using UnityEngine;
using System.Collections;

public class Cut_VictimIntro : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _victim;
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _cam;
    [SerializeField] private Flasher _flasherWhite;
    [SerializeField] private AudioClip _gueSound;
    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        StartCoroutine(CutScene());
    }

    IEnumerator CutScene()
    {
        _player.SendMessage("StartCutscene");

        Debug.Log("CutSceneStarted");
        yield return StartCoroutine(CutsceneUtility.ShowText(_player,"Huh...? What is that?", 0.1f));
        yield return new WaitForSeconds(1f);
        _player.SendMessage("DisplayText", "");
        _cam.SendMessage("ChangeTarget", _target);
        _source.PlayOneShot(_gueSound);
        yield return StartCoroutine(CutsceneUtility.ShowText(_victim, "Uhh...",0.25f));
        yield return new WaitForSeconds(1f);
        _cam.SendMessage("ChangeTarget", _player.transform);
        _victim.SendMessage("DisplayText", "");
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "I am scared...\nWhat is this place?", 0.1f));
        yield return new WaitForSeconds(0.75f);
        _player.SendMessage("DisplayText", "");
        yield return StartCoroutine(_flasherWhite.Flash());
        Debug.Log("CutSceneEnded");   
        _player.SendMessage("EndCutscene");
        Destroy(gameObject);
    }

}
