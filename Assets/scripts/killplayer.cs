using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class killplayer : MonoBehaviour
{
    public int respawn;
    private GameObject player;
    public int maxHealth;
    public int health;
    private Animator playerAnimator;
    public AudioSource soundSource; // AudioSource for both death and revive sounds
    public AudioClip deathSoundClip; // Death sound AudioClip
    public AudioClip reviveSoundClip;
    public AudioClip healup; // Revive sound AudioClip
    public playerHealth healthBar;
    private CapsuleCollider2D  playerCollider;
    private Rigidbody2D playerRigidbody;
    private MonoBehaviour playerController;

    private int originalLayer;
     private Vector3 respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
         
    health = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            Debug.Log("Health bar assigned and max health set.");
        }
        else
        {
            Debug.LogError("Health bar not assigned!");
        } 
        player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
             playerCollider = player.GetComponent<CapsuleCollider2D>();
            playerRigidbody = player.GetComponent<Rigidbody2D>();
          playerController = player.GetComponent<MonoBehaviour>();

           originalLayer = player.layer;
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        health = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.setHealth(health);
        }
    }
    public void takedamage(int damage)
    {
        playerAnimator.SetTrigger("Attacked");
        health -= damage;

        if (healthBar != null)
        {
           
            healthBar.setHealth(health);
        }
        else
        {
            Debug.LogError("Health bar not assigned!");
        }
       
        if (health <= 0)
        {
              afterdeath();
            if (playerAnimator != null)
            {
                 if (soundSource != null && deathSoundClip != null)
            {
                soundSource.clip = deathSoundClip;
                soundSource.Play();
            }
                playerAnimator.SetBool("isDead", true);
              
            }
            else
            {
                Debug.LogError("Player animator not assigned!");
            }
            
        }
    }
     private void afterdeath(){
       
            // Instead of destroying the collider, change the tag or layer
            player.tag = "monster";
          player.layer = LayerMask.NameToLayer("enemy");
        

        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = 0;
            playerRigidbody.velocity = Vector2.zero; // Stop any movement
        }
        else
        {
            Debug.LogError("Player Rigidbody2D not assigned!");
        }

        if (playerController != null)
        {
            playerController.enabled = false; // Disable the player's control script
        }
        else
        {
            Debug.LogError("Player control script not assigned!");
        }
        }
    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("healthToken"))
        {
            Debug.Log("Collided with HealthToken");
            if (soundSource != null && healup != null)
            {
                soundSource.clip = healup;
                soundSource.Play();
            }
            else
            {
                Debug.LogError("Sound source or death sound clip not assigned!");
            }
            Heal(50); // Heal amount, you can adjust as needed

            Destroy(other.gameObject); // Destroy the health token
        }
        if(other.CompareTag("player"))
        {
            health=0;
            
            // Play the death sound
            if (soundSource != null && deathSoundClip != null)
            {
                soundSource.clip = deathSoundClip;
                soundSource.Play();
            }
            else
            {
                Debug.LogError("Sound source or death sound clip not assigned!");
            }

             if (healthBar != null)
            {
                healthBar.setHealth(health);
                
            }
            else
            {
                Debug.LogError("Health bar not assigned!");
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isDead", true);
            }
            else
            {
                Debug.LogError("Player animator not assigned!");
            }
            afterdeath();
        }
    }
    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void ResetPlayer()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isDead", false);
        }
        else
        {
            Debug.LogError("Player animator not assigned!");
        }

        // Play the revive sound
        if (soundSource != null && reviveSoundClip != null)
        {
            soundSource.clip = reviveSoundClip;
            soundSource.Play();
        }
        else
        {
            Debug.LogError("Sound source or revive sound clip not assigned!");
        }
             player.tag = "player";
              player.layer = originalLayer;

        // Load the respawn scene
        // SceneManager.LoadScene(respawn);
        playerAnimator.SetBool("isDead", false);
        player.transform.position = respawnPoint; // Move player to respawn point
        health = maxHealth; // Reset health
        healthBar.setHealth(health); // Update health bar

        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = 1;
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
