using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (string.Equals(col.gameObject.tag, "Player"))
            col.gameObject.transform.GetComponent<Player>().ReachedLevelEnd();
    }
}