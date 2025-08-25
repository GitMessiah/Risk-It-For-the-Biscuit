using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f;
    public float attackLength = 0.5f;
    public float dodgeForce = 5f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 0.3f;
    public float attackCooldown = 1f;
    

    Rigidbody2D rb;

    public Camera cam;

    public GameObject scratchAttack;

    bool movementLock = false;
    bool canDash = true;
    bool canAttack = true;

    Vector2 movement;
    Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0)){
            PrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash)
            {
                Dodge();
                StartCoroutine(DashCooldwon(dodgeCooldown));
            }
                
        }
        

    }

    IEnumerator DashCooldwon(float time)
    {
        canDash = false;

        yield return new WaitForSeconds(time);

        canDash = true;

    }


    void Dodge()
    {
        StartCoroutine(MovementLock(dodgeTime));
        rb.AddForce(movement * dodgeForce, ForceMode2D.Impulse);
    }

    IEnumerator MovementLock(float time)
    {

        Debug.Log("run");

        movementLock = true;
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;

        yield return new WaitForSeconds(time);

        movementLock = false;
    }

    private void FixedUpdate()
    {
        
        if (!movementLock)
        {
            rb.linearVelocity = movement * speed;
        }

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void PrimaryAttack()
    {
        
        if (!scratchAttack.activeSelf && canAttack)
        {
            scratchAttack.SetActive(true);
            StartCoroutine(DisableHitbox());
            StartCoroutine(AttackCooldown(attackCooldown));
        }




    }
    IEnumerator AttackCooldown(float time)
    {
        canAttack = false;

        yield return new WaitForSeconds(time);

        canAttack = true;
    }
    


    IEnumerator DisableHitbox()
    {

        Debug.Log("start");

        yield return new WaitForSeconds(attackLength);

        scratchAttack.SetActive(false);
    }
    


}
