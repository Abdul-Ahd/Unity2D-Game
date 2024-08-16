using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    float startz;
    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    Vector2 parallaxFactor;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startz = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition+travel;
    }
}
