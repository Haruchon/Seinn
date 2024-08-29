using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCaveTrigger : MonoBehaviour
{
    public GameObject warp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            warp.SetActive(false);
        }
    }

}
