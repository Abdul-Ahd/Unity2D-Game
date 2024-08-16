using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    public DetectionZone attackZone;
   public DetectPlayer movementZone;
    public AudioSource hitSoundSource; // Reference to the AudioSource component for the hit sound
    public AudioClip hitSoundClip; 
    public AudioClip attackSoundClip;
    public AudioClip deadsound; 
    private bool _hasTarget = false;
    public float radius;
    public LayerMask playerLayer;
    public int damage;
    public GameObject attackPoint;
       public float moveSpeed;
       private bool playerInRange= false;
       private bool isAttacking = false;
       private bool isdead=false;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            if (anim != null)
            {
                anim.SetBool("hasTarget", value);
                 
            }
        }
    }
    public bool Target {
        get{
            return playerInRange;
        }
        private set {
        playerInRange=value;
        if(anim !=null)
        {
            anim.SetBool("ismoving",value);
        }
    }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = health;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        if (attackZone == null)
        {
            Debug.LogError("DetectionZone not assigned in " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null || attackZone == null)
        {
            return;
        }

        if (health < currentHealth)
        {
            currentHealth = health;
            anim.SetTrigger("Attacked");
          
            PlaySoundEffect(hitSoundClip);
        }

        if (health <= 0)
        {
            isdead= true;
            anim.SetBool("isdead", true);
            StartCoroutine(DestroyColliderAfterDeath());
        }

        HasTarget = attackZone.detectedColliders.Count > 0;
        
        // if (HasTarget && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        // {
        //      anim.SetBool("ismoving",false);
        //     Attack();
           
        // }
        if (HasTarget && !isAttacking)
        {
            anim.SetBool("ismoving", false);
            Attack();
        }
        else if (movementZone != null && movementZone.detectedColliders.Count > 0)
        {
            Target = movementZone.detectedColliders.Count > 0;
            if (Target && !HasTarget && !isdead )
            {
                MoveTowardsTarget();
            }
            else
            {
                Target = false;
                rb2d.velocity = Vector2.zero;
            }
        }
        else
        {
            Target = false;
            rb2d.velocity = Vector2.zero;
        }
    }
  private void MoveTowardsTarget()
{
    if (movementZone.detectedColliders.Count == 0)
    {
        return;
    }

    Collider2D targetCollider = movementZone.detectedColliders[0];
    Vector2 direction = (targetCollider.transform.position - transform.position).normalized;
    rb2d.velocity = new Vector2(direction.x * moveSpeed, rb2d.velocity.y);

    // Flip the enemy to face the target
    if (transform.position.x < targetCollider.transform.position.x && transform.localScale.x > 0)
    {
        Flip();
    }
    else if (transform.position.x > targetCollider.transform.position.x && transform.localScale.x < 0)
    {
        Flip();
    }
     anim.SetBool("ismoving", true);
}


    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Attack()
    {
         isAttacking = true;
        anim.SetTrigger("Attack");
        
        
    }
 public void damageAttack()
    {
Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
              
            killplayer playerHealth = player.GetComponent<killplayer>();
            if (playerHealth != null)
            {
               
                playerHealth.takedamage(damage);
                
               
            }
        }
    }
     private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
    private IEnumerator DestroyColliderAfterDeath()
    {
      
        // Assuming the death animation is 2 seconds long
        yield return new WaitForSeconds(0.5f);

        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }

        if (rb2d != null)
        {
            rb2d.gravityScale = 1;
        }

        // Optionally, you can also destroy the GameObject after a delay
        Destroy(gameObject, 1f);
    }


    private void PlaySoundEffect(AudioClip clip)
    {
        if (hitSoundSource != null && clip != null)
        {
            hitSoundSource.clip = clip;
            hitSoundSource.Play();
        }
    }
    public void palyattack(){
        PlaySoundEffect(attackSoundClip);
    }
    public void deatheffect(){
        PlaySoundEffect(deadsound);
    }
 
}
