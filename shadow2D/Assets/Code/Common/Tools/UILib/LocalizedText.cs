using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using ProjectX.UnityGame.Data;

public class LocalizedText : MonoBehaviour {

    public string id;
    // Use this for initialization
    public string description;
    void Awake()
    {
        if (string.IsNullOrEmpty(id) == false)
        {
            Text text = GetComponent<Text>();
            if (null != text)
            {
                //object result = LanguageSet.GetString(id); 
                //string contentStr = result as string;
                //contentStr = contentStr.Replace("\\n", "\n");
                //text.text = contentStr;
            }
        }
    }
}
