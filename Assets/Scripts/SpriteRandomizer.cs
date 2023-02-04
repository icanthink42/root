using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SpriteRandomizer : MonoBehaviour
{
    private static Sprite[] _sprites;
    private static List<Sprite> _availableSprites;
    private static Random _random;
    // Start is called before the first frame update
    void Start()
    {
        if (_sprites == null)
        {
            _random = new Random();
            _sprites = Resources.LoadAll<Sprite>("RandomPrograms");
            _availableSprites = (_sprites.Clone() as Sprite[]).ToList();
        }

        if (_availableSprites.Count < 1)
        {
            print("a");
            _availableSprites = (_sprites.Clone() as Sprite[]).ToList();
        }

        int randomIndex = _random.Next(0, _availableSprites.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = _availableSprites[randomIndex];
        _availableSprites.RemoveAt(randomIndex);
        print(_availableSprites.Count);
        
    }
}
