using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;
     private Animator animator;

    // Start is called before the first frame update
    void Start()
    {  
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolDestination == 0){
            transform.position= Vector2.MoveTowards(transform.position,patrolPoints[0].position,moveSpeed*Time.deltaTime);
           animator.SetBool("ismoving", true);
            if(Vector2.Distance(transform.position,patrolPoints[0].position)<.2f)
            {
                transform.localScale = new Vector3(-1,1,1);
                patrolDestination = 1;
            }
        }
         if (patrolDestination == 1){
            animator.SetBool("ismoving", true);
            transform.position= Vector2.MoveTowards(transform.position,patrolPoints[1].position,moveSpeed*Time.deltaTime);
            if(Vector2.Distance(transform.position,patrolPoints[1].position)<.2f)
            {
                  transform.localScale = new Vector3(1,1,1);
                patrolDestination = 0;
            }
        }
    }
}
