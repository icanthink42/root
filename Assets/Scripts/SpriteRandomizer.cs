using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpriteRandomizer : MonoBehaviour
{
    private static Sprite[] _sprites;
    private static Random _random;
    // Start is called before the first frame update
    void Start()
    {
        if (_sprites == null)
        {
            _random = new Random();
            _sprites = Resources.LoadAll<Sprite>("RandomPrograms");
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = _sprites[_random.Next(0, _sprites.Length)];
    }
}
