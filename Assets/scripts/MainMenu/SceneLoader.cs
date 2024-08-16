using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loaderUI;
    public Slider Progressbar;
    public void LoadScene(int index){
        StartCoroutine(LoadScene_Coroutine(index));
    }
    public IEnumerator LoadScene_Coroutine(int index){
        Progressbar.value = 0;
        loaderUI.SetActive(true);
        AsyncOperation  asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation =false;
        float progress = 0;
        while(!asyncOperation.isDone){
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            Progressbar.value=progress;
            if(progress >= 0.9f)
            {
                Progressbar.value = 1;
                asyncOperation.allowSceneActivation=true;
            }
            yield return null;
        }
    }
}
