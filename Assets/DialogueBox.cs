using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    public void AddLetter(char letter) {
        tmp.text = tmp.text + letter;
    }

    public void SetText(string text) {
        tmp.text = text;
    }

    public void EraseText() {
        tmp.text = "";
    }

}
