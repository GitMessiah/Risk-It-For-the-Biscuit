using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public string bossName;

    public float speed;

    bool canMove = true;
    bool doingAttack = false;

    Transform player;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Vector2 movement = player.position - transform.position;
            movement.Normalize();
            rb.linearVelocity = movement;
        }

        if (!doingAttack)
        {
            StartCoroutine(bossName);
        }

    }


    IEnumerator LockMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

}
