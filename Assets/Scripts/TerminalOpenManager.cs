using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalOpenManager : MonoBehaviour
{
    [SerializeField] private GameObject terminal; 
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            terminal.SetActive(true);
        }
        
    }
}
