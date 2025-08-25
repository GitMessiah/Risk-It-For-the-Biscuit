using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class TextSystem : MonoBehaviour
{

    public TMP_Text text;

    private List<string> textContent;

    public float textDelay = 1f;

    int numLetters = 1;

    int numLines = 0;

    string textPlaying;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textContent = new List<string>();

        PlayText("WE love BIG rat TITTIES.|Rats Yum.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayText(string text)
    {
        string textDup = text; 

        int i = 0;

        while (textDup.IndexOf("|") != -1)
        {
            i = textDup.IndexOf("|");
            textContent.Add(textDup.Substring(0, i));
            textDup = textDup.Substring(i + 1, textDup.Length - i - 1);
        }

        textContent.Add(textDup.Substring(0, textDup.Length));
        
        InvokeRepeating("PlayingText", 3f, textDelay);
    }

    public void PlayingText() 
    {
        if (numLines == 0)
        {
            textPlaying = textContent[0];
        }


        if (numLetters <= textPlaying.Length)
        {
            text.text = textPlaying.Substring(0, numLetters);
            numLetters++;

        } else
        {

            if (Input.anyKeyDown && numLines < textContent.Count)
            {
                numLines++;
                textPlaying = textContent[numLines];
                numLetters = 1;
            } else if (numLines + 1 == textContent.Count)
            {
                CancelInvoke("PlayingText");
                Debug.Log("finished 13 seconds");
            }

        }
       
    }


}
