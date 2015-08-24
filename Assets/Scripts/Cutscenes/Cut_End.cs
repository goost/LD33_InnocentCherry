using UnityEngine;
using System.Collections;

public class Cut_End : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _textScreen;
    [SerializeField] private GameObject _endMessage;
    [SerializeField] private GameObject _music;
    [SerializeField] private GameObject _rainMusic;
    [SerializeField]private Transform _target;
    [SerializeField] private Camera _cam;
    [SerializeField] private Fader _fader;
    [SerializeField] private Fader _faderRed;
    [SerializeField] private Flasher _flasherWhite;
    [SerializeField] private Flasher _flasherRed;
    [SerializeField] private AudioClip _headHurt;
    private AudioSource _source;
    private bool isActive;

    void Awake()
    {
        isActive = false;
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isActive) return;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive) return;
        StartCoroutine(CutScene());
    }

    IEnumerator CutScene()
    {

        Debug.Log("CutSceneStarted");
        _player.SendMessage("StartCutscene");
        isActive = true;
        
        _player.transform.localScale = new Vector3(-1,1,1);
        yield return new WaitForSeconds(0.5f);
        _cam.SendMessage("ChangeTarget", _target);
        yield return new WaitForSeconds(2f);
        _cam.SendMessage("ChangeTarget", _player.transform);
        _player.transform.localScale = new Vector3(1, 1, 1);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "What was that?", 0.1f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Did I see...him?", 0.1f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Where am I, dawm it!", 0.1f));
        yield return new WaitForSeconds(0.5f);
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Ahh!"));
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "My head still hurts...", 0.1f));
        yield return new WaitForSeconds(0.25f);
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "!!"));
        _source.PlayOneShot(_headHurt);
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(_flasherRed.Flash());
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "I remember..."));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_player, "Ohnononono",0.4f));

        
        yield return StartCoroutine(_fader.FadeOut());
        _rainMusic.SetActive(false);
        _player.SendMessage("DisplayText", "");
        _cam.SendMessage("ChangeTarget", _textScreen.transform);
        yield return new WaitForSeconds(1f);
        _music.SetActive(true);
        yield return StartCoroutine(_fader.FadeIn());
        yield return StartCoroutine(CutsceneUtility.ShowText(_textScreen, "Mark and I went out....\n" +
                                                                            "We were going to party\n" +
                                                                            "and party hard we did.\n" +
                                                                            "I remember.\n" +
                                                                            "Mark and I shared something.\n" +
                                                                            "LSD? I'm not sure.\n", 0.15f));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_textScreen, "But I know, I definitely know...\n" +
                                                                          "That it is my head,\n" +
                                                                          "messing with me...\n" +
                                                                          "And that means...\n", 0.15f));
        _music.SetActive(false);
        yield return StartCoroutine(CutsceneUtility.ShowText(_textScreen, "I killed Mark.", 0.5f));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(CutsceneUtility.ShowText(_textScreen, "My mind went blank.\n" +
                                                                            "My mind tricked me,\n" +
                                                                            "made me do the most monstrous\n" +
                                                                            "thing a human being can do.\n" +
                                                                            "To kill.\n" +
                                                                            "I am a monster, forever.", 0.15f));
        yield return new WaitForSeconds(2f);
        _textScreen.SendMessage("DisplayText", "");
        _music.SetActive(false);
        _endMessage.SetActive(true);

        yield return StartCoroutine(_flasherWhite.Flash());
        Debug.Log("CutSceneEnded");
        isActive = false;
    }
}
