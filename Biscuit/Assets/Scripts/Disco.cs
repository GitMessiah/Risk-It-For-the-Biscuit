using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Disco : MonoBehaviour
{

    RawImage rawDog;

    public float speed = 0.3f;
    public float opacity = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rawDog = GetComponent<RawImage>();
        InvokeRepeating("ChangeColor", 0f, speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeColor()
    {
        rawDog.color = new Color(Random.value, Random.value, Random.value, opacity);
    }

}
