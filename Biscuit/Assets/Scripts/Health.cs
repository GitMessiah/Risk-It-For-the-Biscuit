using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float health = 100f;
    public float defenceMod = 0f;

    public float invincibleTime = 0.2f;

    bool invincible = false;

    SpriteRenderer sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (invincible)
        {
            sprite.color = Color.red;
            StartCoroutine(Invincible());
        }
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(invincibleTime);

        invincible = false;
        sprite.color = Color.white;
    }

    public void DoDamage(float damage)
    {
        
        if (!invincible)
        {
            health -= damage * (1 - defenceMod);
            invincible = true;
        }
        
    }

}
