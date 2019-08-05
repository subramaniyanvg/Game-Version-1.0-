using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;
    [HideInInspector]
    public float camFadeCompSecs = -1;

    public delegate void OnLevelLoadedDel();
    public event OnLevelLoadedDel OnLevelLoadedEvt = null;

    public delegate void OnLevelEndDel();
    public event OnLevelEndDel OnLevelEndEvt = null;

    [HideInInspector]
    public bool levelReady = false;    

    //Will be called by FadeScreen as all transition happens through fading its the responsible of FadeScreen class to call these function.
    public void InvokeLevelLodedEvt()
    {
        if (OnLevelLoadedEvt != null)
            OnLevelLoadedEvt.Invoke();
        OnLevelLoadedEvt = null;
        levelReady = true;
    }

    private void Awake()
    {
        Cursor.visible = false;
		if (instance == null) 
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
        else
			Destroy (gameObject);
    }

    private void OnDestroy()
    {
        if (GameObject.Equals(LevelManager.instance.gameObject, gameObject))
        {
            OnLevelEndEvt = null;
            OnLevelLoadedEvt = null;
            levelReady = false;
        }
    }
    
    public void LoadSceneFromMainMenu(int sceneInd)
    {
        levelReady = false;
        OnLevelLoadedEvt = null;
        OnLevelEndEvt = null;
        GetComponent<FadeScreen>().sceneLoaded = false;
        StartCoroutine(LoadYourAsyncScene(sceneInd));
    }

    public void LoadNextScene(Scene scene)
    {
        if (OnLevelEndEvt != null)
            OnLevelEndEvt.Invoke();
        LeanTween.cancelAll (false);
        levelReady = false;
        OnLevelLoadedEvt = null;
        OnLevelEndEvt = null;
        GetComponent<FadeScreen> ().sceneLoaded = false;
        int nextSceneIndex = scene.buildIndex + 1;
		if (nextSceneIndex <= (SceneManager.sceneCountInBuildSettings - 1))
            StartCoroutine(LoadYourAsyncScene(nextSceneIndex));
    }

    public void ReloadScene(Scene scene)
    {
	   LeanTween.cancelAll (false);
       levelReady = false;
       OnLevelLoadedEvt = null;
       OnLevelEndEvt = null;
       GetComponent<FadeScreen> ().sceneLoaded = false;
       StartCoroutine(LoadYourAsyncScene(scene.buildIndex));
    }

    IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        string currentScene = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Additive);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
            yield return null;
        SceneManager.UnloadSceneAsync(currentScene);
    }
}