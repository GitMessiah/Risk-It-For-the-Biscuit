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
    public float secondaryAttackSpeed = 5f;
    public float wallHitStun = 0.4f;
    public float maxSecondaryChargeTime = 1f;

    float secondaryCharge = 0;
    bool secondaryAttackActive = false;
    
    

    Rigidbody2D rb;

    public Camera cam;

    public GameObject scratchAttack;
    public GameObject aimingRetical;

    bool movementLock = false;
    bool canDash = true;
    bool canAttack = true;
    bool aimingReticalActive = false;

    Vector2 movement;
    Vector2 mousePos;

    SpriteRenderer aimingRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aimingRenderer = aimingRetical.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (aimingReticalActive)
            aimingRetical.SetActive(true);
        else 
            aimingRetical.SetActive(false);

        aimingRenderer.color = new Color((secondaryCharge) / maxSecondaryChargeTime, 0, 0);

        if (Input.GetMouseButton(0)){
            PrimaryAttack();
        }

        if (Input.GetMouseButton(1) && !movementLock)
        {
            secondaryCharge += Time.deltaTime;
            aimingReticalActive = true;
            
        } else
        {
            if (secondaryCharge > maxSecondaryChargeTime)
            {
                SecondaryAttack();
            }
            aimingReticalActive = false;
            secondaryCharge = 0;
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
        StartCoroutine(MovementLock(dodgeTime, true));
        rb.AddForce(movement * dodgeForce, ForceMode2D.Impulse);
    }

    IEnumerator MovementLock(float time, bool resetVelocity)
    {

        Debug.Log("run");

        movementLock = true;
        canDash = false;

        if (resetVelocity)
        {
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(time);

        movementLock = false;
        canDash = true;
    }

    private void FixedUpdate()
    {
        
        if (!movementLock)
        {
            rb.linearVelocity = movement * speed * Time.deltaTime;
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

    void SecondaryAttack()
    {

        Vector2 dashDir = mousePos - rb.position;
        dashDir.Normalize();

        movementLock = true;
        canDash = false;
        secondaryAttackActive = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDir * secondaryAttackSpeed);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (secondaryAttackActive)
        {
            if (collision.gameObject.CompareTag("Wall") && secondaryAttackActive)
            {
                StartCoroutine(MovementLock(wallHitStun, false));
                secondaryAttackActive = false;
            } else if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy with secondary attack");
                StartCoroutine(MovementLock(wallHitStun, false));
                secondaryAttackActive = false;

            }
        }
    }


}
