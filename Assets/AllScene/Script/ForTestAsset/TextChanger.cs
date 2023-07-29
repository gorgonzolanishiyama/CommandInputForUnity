using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{

    public Text text;

    public void ChangeText( string text)
    {
        this.text.text = text;
    }
}
