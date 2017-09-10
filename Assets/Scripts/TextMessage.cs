using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMessage : MonoBehaviour {

    public static TextMessage Instance { get; private set; }
    
    public GameObject textMessageBorderGmObj;

    void Awake()
    {
        Instance = this;
    }

    public void ChangeTextMessage_Border(string textMessage)
    {
        textMessageBorderGmObj.GetComponent<TextMesh>().text = textMessage;
    }
}
