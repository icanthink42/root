using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    private string oldView = "[sudo] password for gill_bates: ";

    private TMP_InputField inputField;
    // Start is called before the first frame update
    void OnEnable()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        inputField.text = oldView;
        PlatformerController.frozen = true;
    }

    private void OnDisable()
    {
        PlatformerController.frozen = false;
    }

    public void ValueChange()
    {
        if (!inputField.text.StartsWith(oldView))
        {
            inputField.text = oldView;
        }
        else if (inputField.text.EndsWith("\n"))
        {
            if (TryPassword(inputField.text))
            {
                //idk like win game or smthn
            }
            else
            {
                inputField.text += "Sorry, Try again.\n[sudo] password for gill_bates: ";
                oldView = inputField.text;
                inputField.MoveTextEnd(false);
            }
        }
    }

    bool TryPassword(string terminal)
    {
        string[] split_term = terminal.Split("\n");
        if (split_term.Length < 1)
        {
            return false;
        }

        string password = split_term[^2].Substring(31, split_term[^2].Length - 31);
        print(password);
        return false;

    }
}
