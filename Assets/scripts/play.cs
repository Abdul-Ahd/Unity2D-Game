using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class play : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    public VirtualButton jump;
    public VirtualButton Attack;
    private Rigidbody2D rb;
    private Animator animator;
    private float hInput;
    float jumpingPower = 10f;
    bool isGrounded = false;
    private bool isAttacking = false;
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;
    public float baseDamage = 10f;
    private float currentDamage;
    public AudioSource src;
    public AudioClip jumpS, landS, attackS, walkS;
    public string finishTag = "Finish";
    public int nextLevelIndex = 2;
    public bool Sowrd = false;
    public SceneLoader sceneLoader; 

    private bool isOnWall = false;
    private float wallTiltAngle = 45f;
    
      private CapsuleCollider2D bodyCollider;
    private CapsuleCollider2D climbCollider;
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        enabled = true;
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (joystick == null)
        {
            Debug.LogError("FixedJoystick component not assigned to " + gameObject.name);
        }

        if (jump == null)
        {
            Debug.LogError("Jump button not assigned to " + gameObject.name);
        }

        currentDamage = baseDamage; // Initialize current damage
        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
        if (colliders.Length < 2)
        {
            Debug.LogError("Expected 2 CapsuleCollider2D components but found " + colliders.Length);
        }
        else
        {
            climbCollider = colliders[0]; // Assuming the first collider is for climbing
            bodyCollider = colliders[1]; // Assuming the second collider is for the body
        }

    }

    void Update()
    {
        if (joystick != null)
        {
            hInput = joystick.Horizontal * moveSpeed;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(hInput));
            animator.SetInteger("Direction", hInput > 0 ? 1 : hInput < 0 ? -1 : 0);
            animator.SetBool("sword", Sowrd); // Update animator with the sword status
        }

        if (hInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (hInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (jump != null && jump.IsPressed() && isGrounded)
        {
            Jump();
        }

        if (Attack != null && Attack.IsPressed())
        {
            Debug.Log("Attack button pressed");
            attack();
        }

        if (Mathf.Abs(hInput) > 0 && isGrounded && !src.isPlaying)
        {
            src.clip = walkS;
            src.Play();
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(hInput, rb.velocity.y);

            if (animator != null)
            {
                animator.SetFloat("xvel", Mathf.Abs(rb.velocity.x));
                animator.SetFloat("yvel", rb.velocity.y);
            }

            if (rb.velocity.y <= 0)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        { 
            animator.SetBool("wall",false);
            isGrounded = true;
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
               
            }
            PlaySoundEffect(landS);
        }
        // if (collision.gameObject.CompareTag("wall"))
        // {
        //     isGrounded = true;
        //     isOnWall = true;
        //     if (animator != null)
        //     {
        //         animator.SetBool("isJumping", false);
        //         animator.SetBool("wall", true);
        //     }
        //     PlaySoundEffect(walkS);
           
        //       if (climbCollider != null)
        //     {
        //         climbCollider.transform.rotation = Quaternion.Euler(0, 0, wallTiltAngle);
        //     }
          
        // }

        if (collision.CompareTag("Sword"))
        {
            EquipSword();
            Destroy(collision.gameObject); // Remove the sword from the scene
        }

        if (collision.CompareTag(finishTag))
        {
            StartCoroutine(NextLevel());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
        // if (collision.gameObject.CompareTag("wall"))
        // {
        //     isOnWall = false;
        //    if (climbCollider != null)
        //     {
        //         climbCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        //     }

        //     if (animator != null)
        //     {
        //         animator.SetBool("wall", false);
        //     }
        // }
    }

    private void Jump()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
                isGrounded = false;
                animator.SetBool("isJumping", !isGrounded);
            }
            PlaySoundEffect(jumpS);
        }
    }

    public void damageAttack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);
        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("Hit enemy");
            enemy.GetComponent<EnemyHealth>().health -= currentDamage;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }

    private void attack()
    {
        if (!isAttacking && rb != null && animator != null)
        {
            animator.SetBool("att", true);
            isAttacking = true;
            PlaySoundEffect(attackS);
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("att", false);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        if (src != null && clip != null)
        {
            src.clip = clip;
            src.Play();
        }
    }

    private void EquipSword()
    {
        Sowrd = true; // Set the sword status to true
        currentDamage = baseDamage + 15f; // Increase damage by 15
        animator.SetTrigger("equip");
        animator.SetBool("sword",true); // Trigger the equip animation
    }

     private IEnumerator NextLevel()
    {
        rb.velocity = Vector2.zero;
        enabled = false;
        yield return new WaitForSeconds(0);
        sceneLoader.LoadScene(nextLevelIndex); // Use the SceneLoader to load the next level
    }
}
