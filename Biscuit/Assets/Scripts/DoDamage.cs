using UnityEngine;

public class DoDamage : MonoBehaviour
{


    public string attackTag;

    public float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(attackTag))
        {
            collision.gameObject.GetComponent<Health>().DoDamage(damage);
        }
        Debug.Log("test");
    }

    

}
