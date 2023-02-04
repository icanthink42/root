using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FolderSceneManager : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private Vector2 spawnLocation;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
            col.transform.position = spawnLocation;
        }
    }
}
