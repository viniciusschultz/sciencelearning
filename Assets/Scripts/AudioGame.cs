using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGame : MonoBehaviour
{
    SpriteRenderer sprite;
    public static AudioGame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }   

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        AudioListener.pause = false;
    }
    private void Update()
    {
        if (AudioListener.pause == false)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    private void OnMouseDown()
    {
        if (gameObject.tag == "Mute")
        {
            sprite.color = new Color(1f, 1f, 1f, 0f);
            AudioListener.pause = !AudioListener.pause;
        }
    }
}
