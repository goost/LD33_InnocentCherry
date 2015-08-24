using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class CutsceneUtility {

    static Regex rgx = new Regex(@"^[a-zA-Z0-9?.!',]$");

    static public IEnumerator ShowText(GameObject who, string what, float wait = 0.05f)
    {
        Debug.Log("Start ShowText");
        for (var i = 0; i < what.Length; i++)
        {
            var s = what.Substring(0, i+1);
            if (rgx.IsMatch(what.ElementAt(i).ToString()))
            {
                Debug.Log("Match! TypewriterSound");
                TypeWriterSound.Singleton.Play();
            }
            
            who.SendMessage("DisplayText", s);
            yield return new WaitForSeconds(wait);
        }
        Debug.Log("End ShowText");
    }
	
}
