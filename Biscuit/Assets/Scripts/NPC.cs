using UnityEngine;

public class NPC : MonoBehaviour
{

    public string dialogue;
    TextSystem textSystem;

    bool interacting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textSystem = FindFirstObjectByType<TextSystem>().GetComponent<TextSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interacting && Input.GetKeyDown(KeyCode.E)) {
            textSystem.PlayText(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = false;
        }
    }
}
