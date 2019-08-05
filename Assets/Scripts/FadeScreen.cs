using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DentedPixel;

public class FadeScreen : MonoBehaviour
{
    public delegate  void   OnFadeInCompl();
	public  static event	OnFadeInCompl OnFadeInComplEvt = null;

	public delegate  void   OnFadeOutCompl();
	public  static event	OnFadeOutCompl OnFadeOutComplEvt = null;

    public Texture2D        fadeTexture;
    [HideInInspector]
    public float            fadeCompSecs;
	private float           alpha = 1;
	private bool	        doFadeIn = false;
	private bool 	        doFadeOut = false;
	private int 	        frameWaited = -1;
	[HideInInspector]
	public  bool	        sceneLoaded = false;

    void UpdateAlpha(float alphaVal)
    {
        alpha = alphaVal;
    }

    public bool	DofadeIn
	{
		set 
		{
			if (value) 
			{
				doFadeIn = true;
				alpha = 0;
                LeanTween.value(gameObject, 0, 1, fadeCompSecs).setOnUpdate(UpdateAlpha);
			}
			else
				doFadeIn = value;
		}
        get
        {
            return doFadeIn;
        }
	}

	public bool	DofadeOut
	{
		set 
		{
			if (value) 
			{
				doFadeOut = true;
				alpha = 1;
                LeanTween.value(gameObject, 1, 0, fadeCompSecs).setOnUpdate(UpdateAlpha);
            }
            else
				doFadeOut = value;
		}
        get
        {
            return doFadeOut;
        }
	}

    void Awake () 
	{
        if (GameObject.Equals(LevelManager.instance.gameObject, gameObject))
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
	{
        if (GameObject.Equals(LevelManager.instance.gameObject,gameObject))
        {
            OnFadeInComplEvt = null;
            OnFadeOutComplEvt = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

	void Update()
	{
        if (GameObject.Equals(LevelManager.instance.gameObject, gameObject))
        {
            if (frameWaited >= 0)
            {
                frameWaited++;
                if (frameWaited > 3)
                {
                    if (SceneManager.sceneCount == 1)
                    {
                        sceneLoaded = true;
                        LevelManager.instance.InvokeLevelLodedEvt();
                        frameWaited = -1;
                    }
                    else
                        frameWaited = 3;
                }
            }
        }
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
        frameWaited = 0;
    }

    void OnGUI()
    {
        if (GameObject.Equals(LevelManager.instance.gameObject, gameObject))
        {
            if (!sceneLoaded)
            {
                GUI.color = new Vector4(0,0,0,1);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
                return;
            }
            if (doFadeOut)
            {
                GUI.color = new Vector4(0, 0, 0, alpha);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
                if (alpha <= 0)
                {
                    DofadeOut = false;
                    if (OnFadeOutComplEvt != null)
                        OnFadeOutComplEvt.Invoke();
                }
            }
            else if (doFadeIn)
            {
                GUI.color = new Vector4(0, 0, 0, alpha);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
                if (alpha >= 1)
                {
                    DofadeIn = false;
                    if (OnFadeInComplEvt != null)
                        OnFadeInComplEvt.Invoke();
                }
            }
        }
    }
}