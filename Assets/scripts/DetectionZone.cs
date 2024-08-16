using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
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
        if (collision != null && !detectedColliders.Contains(collision))
        {
            detectedColliders.Add(collision);
            Debug.Log("Player detected: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            detectedColliders.Remove(collision);
             Debug.Log("Player left detection zone: " + collision.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Optionally, you can initialize or check something at the start.
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally, you can add code here if you need to update something each frame.
    }
}
