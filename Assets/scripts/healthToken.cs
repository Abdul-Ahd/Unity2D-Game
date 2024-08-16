using UnityEngine;

public class healthToken : MonoBehaviour
{
    public int healthRestoreAmount = 50;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.tag); // Check if the method is called

        if (other.CompareTag("player"))
        {
            Debug.Log("Collided with Player"); // Confirm collision with Player

            killplayer playerHealth = other.GetComponent<killplayer>();
            
            if (playerHealth != null)
            {
                Debug.Log("Health before: " + playerHealth.health); // Debug player's health before healing

                playerHealth.Heal(healthRestoreAmount);

                Debug.Log("Health after: " + playerHealth.health); // Debug player's health after healing

                Destroy(gameObject); // Destroy the health token after it has been picked up
                Debug.Log("Health token picked up and destroyed");
            }
            else
            {
                Debug.LogError("KillPlayer component not found on Player");
            }
        }
    }
}
