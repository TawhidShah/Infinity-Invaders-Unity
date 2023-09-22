using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    //Direction and speed of the missile.
    private Vector3 direction = Vector3.down;
    private float speed = 10;

    //Reference to the bottom left boundary.
    public GameObject blBoundary;

    //Position of the boundary.
    private Vector3 blBoundaryPos;

    //Blboundary won't move, so we can get it once during Awake.
    private void Awake()
    {
        blBoundaryPos = blBoundary.transform.position;
    }

    private void Update()
    {
        //Move the missile.
        transform.position += direction * speed * Time.deltaTime;

        //Check if the missile has crossed the boundary.
        if (transform.position.y < blBoundaryPos.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the missile has collided with a player or a laser.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Destroy(gameObject);
        }
    }
}
