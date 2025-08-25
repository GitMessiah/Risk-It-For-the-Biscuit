using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


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
        PlayText("WE love BIG rat TITTIES.|Rats Yum.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayText(string text)
    {

        int i = 0;

        while (text.IndexOf("|") != -1)
        {
            int start = i;
            i = text.IndexOf("|");
            textContent.Add(text.Substring(start, i));
        }

        textContent.Add(text.Substring(i, textContent.Count));
        
        InvokeRepeating("PlayingText", 3f, textDelay);
    }

    public void PlayingText() 
    {
        if (numLines == 0)
        {
            textPlaying = textContent[numLines];
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
            } else if (numLines == textContent.Count)
            {
                CancelInvoke("PlayingText");
                Debug.Log("finished 13 seconds");
            }

        }
       
    }


}
