using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //Direction and speed of the laser.
    private Vector3 direction = Vector3.up;
    private float speed = 15;

    //Reference to the top right boundary.
    public GameObject trBoundary;

    //Position of the boundary.
    private Vector3 trBoundaryPos;

    //Event that is invoked when the laser is destroyed.
    public System.Action laserDestroyed;

    //trboundary won't move, so we can get it once during Awake.
    private void Awake()
    {
        trBoundaryPos = trBoundary.transform.position;
    }

    private void Update()
    {
        //Move the laser.
        transform.position += direction * speed * Time.deltaTime;

        //Check if the laser has crossed the boundary.
        if (transform.position.y > trBoundaryPos.y)
        {
            laserDestroyed.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the laser has collided with an invader, missile, or boss invader.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Boss Invader"))
        {
            laserDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}