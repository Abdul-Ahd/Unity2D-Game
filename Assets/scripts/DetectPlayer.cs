using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    public LayerMask detectionLayer; // LayerMask for filtering

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D component missing on " + gameObject.name);
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning("Collider2D is not set as a trigger on " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !detectedColliders.Contains(collision) && ((detectionLayer.value & (1 << collision.gameObject.layer)) > 0))
        {
            detectedColliders.Add(collision);
            Debug.Log("Detected for movement: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            detectedColliders.Remove(collision);
            Debug.Log("Exited movement detection zone: " + collision.name);
        }
    }
}