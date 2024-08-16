using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void SetMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health;
    }
    public void setHealth(int health){
        slider.value = health;
    }
}
