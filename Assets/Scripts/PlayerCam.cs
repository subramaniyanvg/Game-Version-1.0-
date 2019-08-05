using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour 
{
	private GameObject player;
	public  float 	   camPlayerDist;
	public  bool	   follow;
    public bool active = true;

    public void SetCamPlayerDist(float dist)
    {
        camPlayerDist = dist;
    }

    public void SetActive(bool state)
    {
        active = state;
    }

    private void OnLevelLoaded()
    {
        player = GameObject.Find("Player");
    }

    // Use this for initialization
    void Start () 
	{
        if (!LevelManager.instance.levelReady)
            LevelManager.instance.OnLevelLoadedEvt += OnLevelLoaded;
        else
            OnLevelLoaded();
    }

	void LateUpdate()
	{
       if(follow && player != null && active)
			transform.position = player.transform.position + (-player.transform.forward) * camPlayerDist;
	}
}