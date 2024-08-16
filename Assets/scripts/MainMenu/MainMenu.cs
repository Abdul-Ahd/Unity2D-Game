using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
   public Transform playPoint;
   public Transform exitPoint;
       public float moveSpeed = 5f;
       public GameObject hero;
          public SceneLoader sceneLoader;
       private Animator heroAnimator; 
       private SpriteRenderer heroSpriteRenderer;
       private Rigidbody2D heroRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if (hero != null)
        {
            heroAnimator = hero.GetComponent<Animator>();
            heroSpriteRenderer = hero.GetComponent<SpriteRenderer>();
              heroRigidbody = hero.GetComponent<Rigidbody2D>();
        }
        if (sceneLoader == null)
        {
            Debug.LogError("SceneLoader is not assigned in the inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame()
    {
          StartCoroutine(MoveHeroAndExecute(playPoint.position, () => sceneLoader.LoadScene(1)));
    }

    public void ExitGame()
    {
        StartCoroutine(MoveHeroAndExecute(exitPoint.position, () => 
        {
            Debug.Log("Exit button pressed!");
            Application.Quit();
        }));
    }

   private IEnumerator MoveHeroAndExecute(Vector3 targetPosition, System.Action action)
    {
        while (Vector3.Distance(hero.transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - hero.transform.position).normalized;
            hero.transform.position = Vector3.MoveTowards(hero.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            heroRigidbody.velocity = new Vector2(direction.x * moveSpeed, heroRigidbody.velocity.y);
            // Update xvel based on movement direction
             heroAnimator.SetFloat("xvel", Mathf.Abs(heroRigidbody.velocity.x));
            // Flip hero's sprite based on movement direction
            if (direction.x != 0)
            {
                heroSpriteRenderer.flipX = direction.x < 0;
            }

            yield return null;
        }

        // Reset xvel to 0 when the hero stops moving
        heroAnimator.SetFloat("xvel", 0);

        // yield return new WaitForSeconds(1); 
        action.Invoke();
    }
}

