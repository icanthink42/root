using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class SpriteRandomizer : MonoBehaviour
{
    private static Sprite[] _sprites;
    private static List<Sprite> _availableSprites;
    private static Random _random;
    private GameObject outline;

    private static GameObject _highlight;
    // Start is called before the first frame update
    void Start()
    {
        if (_highlight == null)
        {
            _highlight = Resources.Load("highlight") as GameObject;
        }

        outline = Instantiate(_highlight, transform.position, transform.rotation);
        outline.transform.SetParent(transform);
        outline.SetActive(false);
        if (_sprites == null)
        {
            _random = new Random();
            _sprites = Resources.LoadAll<Sprite>("RandomPrograms");
            _availableSprites = (_sprites.Clone() as Sprite[]).ToList();
        }

        if (_availableSprites.Count < 1)
        {
            _availableSprites = (_sprites.Clone() as Sprite[]).ToList();
        }

        int randomIndex = _random.Next(0, _availableSprites.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = _availableSprites[randomIndex];
        _availableSprites.RemoveAt(randomIndex);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.transform.CompareTag("Player"))
            return;
        outline.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            StartCoroutine(DropPlatform());
        }
    }
    
    IEnumerator DropPlatform()
    {
        yield return new WaitForSeconds (3.0f);
        gameObject.AddComponent<Rigidbody2D>();
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (!col.transform.CompareTag("Player"))
            return;
        outline.SetActive(false);
    }
}
