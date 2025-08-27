using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Tilemaps;
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


    Animator animator;
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
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        aimingRenderer = aimingRetical.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {

        Animations();

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
            if ((movement.x != 0 || movement.y != 0) && canDash)
            {
                Dodge();
                StartCoroutine(DashCooldown(dodgeCooldown));
            }
                
        }
        

    }


    void Animations()
    {
        if (!secondaryAttackActive && !movementLock)
        {
            sprite.flipY = false;

            if (movement.x > 0)
            {
                sprite.flipX = false;
            }
            if (movement.x < 0)
            {
                sprite.flipX = true;
            }

            if (movement.x != 0)
            {
                animator.Play("Player_Run");
            } else
            {
                //transform.localScale = new Vector2(0.06f, 0.06f);
                animator.Play("Player_Idle_Up");
            }
        } else if (secondaryAttackActive)
        {
            sprite.flipX = false;

            Debug.Log(rb.rotation);

            if (rb.rotation > 90 || rb.rotation < -90)
            {
                sprite.flipY = true;
            } else
            {
                sprite.flipY = false;
            }

        }
    }

    IEnumerator DashCooldown(float time)
    {
        canDash = false;

        yield return new WaitForSeconds(time);

        canDash = true;

    }

    

    void Dodge()
    {
        sprite.flipX = false;
        sprite.flipY = false;

        if (movement.y > 0 && movement.x == 0)
        {
            rb.rotation = 90;
        }
        else if (movement.y < 0 && movement.x == 0)
        {
            rb.rotation = -90;
        } else if (movement.x < 0)
        {
            sprite.flipX = true;
        }


        animator.Play("Player_Dash");
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

        rb.rotation = 0;
        movementLock = false;
    }

    private void FixedUpdate()
    {
        
        if (!movementLock)
        {
            rb.linearVelocity = movement * speed * Time.deltaTime;
        }
        
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
        movementLock = true;
        canDash = false;
        secondaryAttackActive = true;

        Vector2 dashDir = mousePos - rb.position;
        dashDir.Normalize();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDir * secondaryAttackSpeed);

        float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        animator.Play("Dash_Enter");
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
                canDash = true;
                animator.Play("Dash_Exit");

            } else if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy with secondary attack");
                StartCoroutine(MovementLock(wallHitStun, false));
                secondaryAttackActive = false;
                canDash = true;
                animator.Play("Dash_Exit");
            }
        }
    }

    public void ScaleSet(float scale)
    {
        transform.localScale = new Vector2(scale, scale);
    }
    


}
