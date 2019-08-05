using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DentedPixel;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class RayProp
    {
        public GameObject ray;
        public float      length;
        public Color      rayColor;
        public LayerMask  colLayToCheckFor;
        public string     tagToCheckFor;
       [HideInInspector]
        public RaycastHit2D    hitInfo;
    }
    public bool drawRay = true;
    public RayProp[] slantedPlatRays;
    public RayProp[] headRays;
    public RayProp[] footRays;
    public RayProp[] sideLftRays;
    public RayProp[] sideRgtRays;

    public float  playerRotSpeed;
    public float  speed;
    public float  jumpUpSpeed;
    public float  jumpUpSecs = Mathf.Infinity;
    public float  comeDownSpeed;

    public bool  canFollowPlayer = true;
    public float fadeCompSecsOnStart = 1;
    public float fadeCompSecsOnRestart = 1;
    public UnityEvent onCamFadeOutCompOnStartEvt;

    private bool       movInputInvoked = false;
    private Vector3    lastPlayerPos = Vector3.positiveInfinity;
    private GameObject playerObj;
    private bool       onGround = false;
    private bool       jumpingUp = false;
    [HideInInspector]
    public bool         playerKilled = false;
    [HideInInspector]
    public float        health = 0;
    [HideInInspector]
    public bool         takeInput = false;
    [HideInInspector]
    public bool         processInput = true;
    private Vector2     playerVel = Vector2.zero;
    private float       playerRot = 0;
    private LTDescr     jumpUpTween;

    private Rigidbody2D rigBody;
    private float rotAmt = 0;


    public void TakeInput(bool state)
    {
        takeInput = state;
    }

    public void ProcessInput(bool state)
    {
        processInput = state;
    }

    void OnLevelLoaded()
    {
        lastPlayerPos = transform.position;
        playerObj = transform.Find("PlayerObj").gameObject;
        transform.GetComponent<Collider2D>().enabled = true;
        rigBody = transform.GetComponent<Rigidbody2D>();
        rigBody.gravityScale = 0;
        OnGameStart();
    }

    private void Start()
    {
        transform.GetComponent<Collider2D>().enabled = false;
        if (!LevelManager.instance.levelReady)
            LevelManager.instance.OnLevelLoadedEvt += OnLevelLoaded;
        else
            OnLevelLoaded();
    }

    public void ReachedLevelEnd()
    {
        CancelJumpingUp();
        Destroy(GetComponent<Collider2D>());
        takeInput = false;
        gameObject.SetActive(false);
        rigBody.velocity = Vector2.zero;
        Camera.main.GetComponent<PlayerCam>().follow = false;
        LevelManager.instance.camFadeCompSecs = -1;
        LevelManager.instance.GetComponent<FadeScreen>().fadeCompSecs = fadeCompSecsOnStart;
        LevelManager.instance.GetComponent<FadeScreen>().DofadeIn = true;
        FadeScreen.OnFadeInComplEvt += OnFadeInCompleteOnLevelEndReached;
    }

    public void OnGameStart()
    {
        if(LevelManager.instance.camFadeCompSecs == -1)
            LevelManager.instance.GetComponent<FadeScreen>().fadeCompSecs = fadeCompSecsOnStart;
        else
           LevelManager.instance.GetComponent<FadeScreen>().fadeCompSecs = fadeCompSecsOnRestart;
        LevelManager.instance.GetComponent<FadeScreen>().DofadeOut = true;
        FadeScreen.OnFadeOutComplEvt += OnFadeOutComplete;
    }

    void OnFadeOutComplete()
    {
        takeInput = true;
        onCamFadeOutCompOnStartEvt.Invoke();
        FadeScreen.OnFadeOutComplEvt -= OnFadeOutComplete;
    }

    void DisableLevelColliders()
    {
        Collider2D[] col = GameObject.FindObjectsOfType<Collider2D>();
        for (int i = 0; i < col.Length; i++)
            col[i].enabled = false;
    }

    void UnsubscribeToEvents()
    {
        FadeScreen.OnFadeInComplEvt -= OnFadeInCompleteOnLevelEndReached;
    }

    void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    void OnFadeInCompleteOnLevelEndReached()
    {
        Camera.main.gameObject.SetActive(false);
        DisableLevelColliders();
        LevelManager.instance.LoadNextScene(SceneManager.GetActiveScene());
    }

    private RayProp IsCealingHit()
    {
        for (int i = 0; i < headRays.Length; i++)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(headRays[i].ray.transform.position, headRays[i].ray.transform.forward, headRays[i].length,headRays[i].colLayToCheckFor);
            if (hitResult.collider != null)
            {
                headRays[i].hitInfo = hitResult;
                return headRays[i];
            }
        }
        return null;
    }

    private RayProp IsSideLeftWallHit()
    {
        for (int i = 0; i < sideLftRays.Length; i++)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(sideLftRays[i].ray.transform.position, sideLftRays[i].ray.transform.forward, sideLftRays[i].length, sideLftRays[i].colLayToCheckFor);
            if (hitResult.collider != null)
            {
                sideLftRays[i].hitInfo = hitResult;
                return sideLftRays[i];
            }
        }
        return null;
    }

    private RayProp IsSideRightWallHit()
    {
        for (int i = 0; i < sideRgtRays.Length; i++)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(sideRgtRays[i].ray.transform.position, sideRgtRays[i].ray.transform.forward, sideRgtRays[i].length, sideRgtRays[i].colLayToCheckFor);
            if (hitResult.collider != null)
            {
                sideRgtRays[i].hitInfo = hitResult;
                return sideRgtRays[i];
            }
        }
        return null;
    }

    private RayProp IsWallHit()
    {
        RayProp prop = IsSideLeftWallHit();
        if (prop != null)
            return prop;
        prop = IsSideRightWallHit();
        if (prop != null)
            return prop;
        return null;
    }

    private RayProp IsSlatedPlatformHit()
    {
        for (int i = 0; i < slantedPlatRays.Length; i++)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(slantedPlatRays[i].ray.transform.position, slantedPlatRays[i].ray.transform.forward, slantedPlatRays[i].length, slantedPlatRays[i].colLayToCheckFor);
            if (hitResult.collider != null)
            {
                slantedPlatRays[i].hitInfo = hitResult;
                return slantedPlatRays[i];
            }

        }
        return null;
    }

    private RayProp IsGroundHit()
    {
        for (int i = 0; i < footRays.Length; i++)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(footRays[i].ray.transform.position, footRays[i].ray.transform.forward, footRays[i].length, footRays[i].colLayToCheckFor);
            if (hitResult.collider != null)
            {
                footRays[i].hitInfo = hitResult;
                return footRays[i];
            }

        }
        return null;
    }

    private void ProcessInput()
    {
#if UNITY_STANDALONE
        if (processInput)
        {
            if (ControllerInfo.instance.IsControllerConnected())
                ProcessJoystickInput();
            else
                ProcessKeyboardInput();
        }
        #endif
    }

    
    void ProcessJumpAndWallInput()
    {
        if (onGround)
        {
            if (float.Equals(jumpUpSecs, 0.0f))
                OnMaxHeightReached();
            else
                jumpUpTween = LeanTween.value(gameObject,0, 1, jumpUpSecs).setOnComplete(OnMaxHeightReached);
            jumpingUp = true;
            onGround = false;
        }
    }

    void ProcessJoystickInput()
    {
        Vector2 dir = Vector2.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f)
            ComputePlyrVelOnInput(Input.GetAxis("Horizontal"));
        if (ControllerInfo.instance.IsConnectedControllerXboxOne())
        {
            if (Input.GetButtonDown("A"))
                ProcessJumpAndWallInput();
        }
        else if(ControllerInfo.instance.IsConnectedControllerPs4())
        {
            if (Input.GetButtonDown("PS4_X"))
                ProcessJumpAndWallInput();
        }
    }

    void ProcessKeyboardInput()
    {
        if (Input.GetKey(KeyCode.D))
            ComputePlyrVelOnInput(1);
        else if (Input.GetKey(KeyCode.A))
            ComputePlyrVelOnInput(-1);
        if (Input.GetKeyDown(KeyCode.Space))
            ProcessJumpAndWallInput();
    }

    void ComputePlyrVelOnInput(float xInpDir)
    {
        movInputInvoked = true;
        if (xInpDir > 0)
             playerVel = transform.right * speed;
        else if(xInpDir < 0)
             playerVel = -transform.right * speed;
    }

    private void Update()
    {
        if (playerObj == null)
            return;
        //Set Player Vel to zero
        if (takeInput)
        {
            movInputInvoked = false;
            playerVel = Vector2.zero;
            playerRot = 0;
            transform.parent = null;
            ProcessInput();
            Vector3 playerMovDir = (transform.position - lastPlayerPos).normalized;
            if (playerMovDir.x > 0)
                rotAmt = -playerRotSpeed;
            else if (playerMovDir.x < 0)
                rotAmt = playerRotSpeed;
            if (!jumpingUp)
                CheckForPlayerGroundPos();
            else 
                CheckForRotDirAndApplyRot();
            ProcessJumping();
            playerObj.transform.RotateAround(playerObj.transform.position, transform.forward, playerRot * Time.deltaTime);
           rigBody.velocity = playerVel;
        }
        else
        {
            playerRot = 0;
            playerVel = (-Vector2.up * comeDownSpeed);
            CheckForPlayerGroundPos();
            playerObj.transform.RotateAround(playerObj.transform.position, transform.forward, playerRot * Time.deltaTime);
            rigBody.velocity = playerVel;
        }
    }

    private void FixedUpdate()
    {
       lastPlayerPos = transform.position;
    }

    void CheckForRotDirAndApplyRot()
    {
        Vector3 playerMovDir = (transform.position - lastPlayerPos).normalized;
        if (playerMovDir.x == 0 && rotAmt == 0)
            playerRot = -playerRotSpeed;
        else
            playerRot = rotAmt;
    }

    void CheckForPlayerGroundPos()
    {
        RayProp prop = IsGroundHit();
        if (prop != null )
        {
            if (movInputInvoked)
            {
                Vector3 playerMovDir = (transform.position - lastPlayerPos).normalized;
                if (playerMovDir.x != 0 && IsWallHit() == null)
                    playerRot = rotAmt;
            }
            prop = IsSlatedPlatformHit();
            if (prop != null)
            {
                if (Vector2.Angle(Vector2.up, prop.hitInfo.normal) > 0 && !movInputInvoked)
                {
                    playerVel = Vector2.zero;
                    playerRot = 0;
                }
            }
            TakeJumpInput();
        }
        else
        {
            CheckForRotDirAndApplyRot();
            CancelJumpingUp();
        }
    }

    void OnMaxHeightReached()
    {
        jumpUpTween = null;
        jumpingUp = false;
    }

    void CancelJumpingUp()
    {
        if (jumpUpTween != null)
            LeanTween.cancel(gameObject,jumpUpTween.id, false);
        onGround = false;
        jumpingUp = false;
        jumpUpTween = null;
    }

    void TakeJumpInput()
    {
        if (jumpUpTween != null)
            LeanTween.cancel(gameObject,jumpUpTween.id, false);
        onGround = true;
        jumpingUp = false;
        jumpUpTween = null;
    }

    void ProcessJumping()
    {
        if (!onGround)
        {
            if (jumpingUp)
            {
                playerVel = playerVel + (Vector2.up * jumpUpSpeed);
                if (IsCealingHit() != null)
                    CancelJumpingUp();
            }
            else
                playerVel = playerVel + (-Vector2.up * comeDownSpeed);
        }
        else
        {
            RayProp prop = IsSlatedPlatformHit();
            if (prop != null)
            {
                if (Vector2.Angle(Vector2.up, prop.hitInfo.normal) > 0 && !movInputInvoked)
                    playerVel = Vector2.zero;
                else
                    playerVel = playerVel + (-Vector2.up * comeDownSpeed);
            }
            else
                playerVel = playerVel + (-Vector2.up * comeDownSpeed);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawRay)
        {
            if (headRays != null)
            {
                for (int i = 0; i < headRays.Length; i++)
                {
                    if (headRays[i].ray != null)
                    {
                        Gizmos.color = headRays[i].rayColor;
                        Gizmos.DrawLine(headRays[i].ray.transform.position, headRays[i].ray.transform.position + (headRays[i].ray.transform.forward * headRays[i].length));
                    }
                }
            }
            if (footRays != null)
            {
                for (int i = 0; i < footRays.Length; i++)
                {
                    if (footRays[i].ray != null)
                    {
                        Gizmos.color = footRays[i].rayColor;
                        Gizmos.DrawLine(footRays[i].ray.transform.position, footRays[i].ray.transform.position + (footRays[i].ray.transform.forward * footRays[i].length));
                    }
                }
            }
            if (slantedPlatRays != null)
            {
                for (int i = 0; i < slantedPlatRays.Length; i++)
                {
                    if (slantedPlatRays[i].ray != null)
                    {
                        Gizmos.color = slantedPlatRays[i].rayColor;
                        Gizmos.DrawLine(slantedPlatRays[i].ray.transform.position, slantedPlatRays[i].ray.transform.position + (slantedPlatRays[i].ray.transform.forward * slantedPlatRays[i].length));
                    }
                }
            }
            if (sideLftRays != null)
            {
                for (int i = 0; i < sideLftRays.Length; i++)
                {
                    if (sideLftRays[i].ray != null)
                    {
                        Gizmos.color = sideLftRays[i].rayColor;
                        Gizmos.DrawLine(sideLftRays[i].ray.transform.position, sideLftRays[i].ray.transform.position + (sideLftRays[i].ray.transform.forward * sideLftRays[i].length));
                    }
                }
            }
            if (sideRgtRays != null)
            {
                for (int i = 0; i < sideRgtRays.Length; i++)
                {
                    if (sideRgtRays[i].ray != null)
                    {
                        Gizmos.color = sideRgtRays[i].rayColor;
                        Gizmos.DrawLine(sideRgtRays[i].ray.transform.position, sideRgtRays[i].ray.transform.position + (sideRgtRays[i].ray.transform.forward * sideRgtRays[i].length));
                    }
                }
            }
        }
    }
#endif
}