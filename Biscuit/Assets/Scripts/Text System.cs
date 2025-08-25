using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TextSystem : MonoBehaviour
{

    public TMP_Text text;

    public GameObject textBackground; //text is a child

    private List<string> textContent;

    public float textDelay = 1f;
    public float startDelay = 0.1f;

    int numLetters = 1;

    int numLines = 0;

    string textPlaying;

    bool showText = false;

    bool keyPressed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textContent = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(KeyPressed(textDelay));
        }

        if (showText)
        {
            textBackground.SetActive(true);
        } 
        else
        {
            textBackground.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            PlayText("Hi guys!|Im on shrroms rn|:drool:");
            Debug.Log("testing");
        }
    }
    IEnumerator KeyPressed(float time)
    {
        keyPressed = true;
        yield return new WaitForSeconds(time);
        keyPressed = false;
    }

    public void PlayText(string text)
    {
        if (textContent.Count == 0)
        {
            showText = true;

            string textDup = text;

            int i = 0;

            while (textDup.IndexOf("|") != -1)
            {
                i = textDup.IndexOf("|");
                textContent.Add(textDup.Substring(0, i));
                textDup = textDup.Substring(i + 1, textDup.Length - i - 1);
            }

            textContent.Add(textDup.Substring(0, textDup.Length));

            InvokeRepeating("PlayingText", startDelay, textDelay);

        }
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

            Debug.Log("Dialogue Line Finished");

            if (keyPressed && numLines < textContent.Count - 1)
            {
                Debug.Log("New Line Started");

                numLines++;
                textPlaying = textContent[numLines];
                numLetters = 1;

            } else if (numLines + 1 == textContent.Count)
            {
                Debug.Log("finished 13 seconds");

                if (keyPressed)
                {
                    showText = false;

                    numLines = 0;
                    numLetters = 1;
                    textContent = new List<string>();
                    text.text = "";
                    CancelInvoke("PlayingText");
                    //deactivate
                }


            }

        }
       
    }


}
