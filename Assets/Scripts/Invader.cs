using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    //Array that holds the 2 sprites the script will switch between.
    public Sprite[] sprites;

    //Time between each animation.
    private float timeBetweenAnimation = 0.5f;

    //Reference to the sprite renderer used to change which sprite is being displayed for the invader.
    private SpriteRenderer spriteRen;

    //Index of the current sprite.
    private int currentSprite;

    //Score per invader kill - default value (value for each type of invader is set on prefabs).
    public int invaderScore = 25;

    //Event that allows invaders script to know when a invader has been killed.
    public System.Action<Invader> invaderKilled;

    private void Awake()
    {
        //Gets the sprite renderer component.
        spriteRen = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //Invoking the function "AnimateSprite" after a delay of "timeBetweenAnimation" seconds and then every "timeBetweenAnimation" seconds.
        InvokeRepeating("AnimateSprite", timeBetweenAnimation, timeBetweenAnimation);
    }

    //Function that switches the sprite being displayed for the invader.
    private void AnimateSprite()
    {
        //Increments the current sprite index.
        currentSprite++;

        //If currentSprite equals the length of sprites array, reset currentSprite to 0.
        if (currentSprite == sprites.Length)
        {
            currentSprite = 0;
        }

        //Set the sprite renderer to the updated sprite.
        spriteRen.sprite = sprites[currentSprite];
    }

    //Function that is called when the invader collides with something.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the collision is with a laser, if it is then the invader killed event is invoked.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            invaderKilled.Invoke(this);
        }
    }
}
