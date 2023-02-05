using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public string currentAnimation;
    private int frame = 0;
    [System.Serializable]
    class Animation
    {
        public string name;
        public Sprite[] frames;
        public float frameTime;
        public string onFinish;
    }
    [SerializeField] private Animation[] _frames;
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
    private float nextFrameTime = 0;
    private SpriteRenderer _spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFrameTime)
        {
            nextFrameTime = Time.time + getAnimation().frameTime;
            if (frame >= getAnimation().frames.Length)
            {
                frame = 0;
                currentAnimation = getAnimation().onFinish;
            }

            _spriteRenderer.sprite = getAnimation().frames[frame];
            frame++;
        }

    }

    void Start()
    {
        nextFrameTime = Time.time;
        foreach (Animation animation in _frames)
        {
            animations.Add(animation.name, animation);
        }

        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentAnimation = _frames[0].name;

    }

    Animation getAnimation()
    {
        return animations[currentAnimation];
    }


    public void PlayAnimation(string animation)
    {
        if (animation == currentAnimation)
        {
            return;
        }
        currentAnimation = animation;
        nextFrameTime = Time.time;
        frame = 0;
    }
}
