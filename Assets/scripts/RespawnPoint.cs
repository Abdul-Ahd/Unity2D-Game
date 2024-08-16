using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            killplayer killPlayer = other.GetComponent<killplayer>();
            if (killPlayer != null)
            {
                killPlayer.SetRespawnPoint(transform.position);
                Debug.Log("respown Set");
            }
        }
    }
}