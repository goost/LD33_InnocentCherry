using UnityEngine;
using System.Collections;

public class Texter : MonoBehaviour
{

    private TextMesh _tm;
    private string _displayText;

	// Use this for initialization
	void Awake ()
	{
	    _tm = GetComponentInChildren<TextMesh>();
	    _tm.text = "";
	}


    void DisplayText(string displayText)
    {
        _displayText = displayText;
    }

    void OnGUI()
    {
        _tm.text = _displayText;
    }
}
