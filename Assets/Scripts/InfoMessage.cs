using UnityEngine;
using System.Collections;

public class InfoMessage : MonoBehaviour {

    [SerializeField] private string _message = "";

	public void OnTriggerEnter2D(Collider2D collision)
	{
	    StartCoroutine(CutsceneUtility.ShowText(collision.gameObject, _message));
        //collision.gameObject.SendMessage("DisplayText", _message);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        collision.gameObject.SendMessage("DisplayText", "");
    }

    
}
