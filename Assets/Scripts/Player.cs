using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Speed of the player.
    private float speed = 5.0f;
    //A variable that will hold the distance moved in the x and y directions.
    private Vector2 playerMovementInput;

    //Reference to the bottom left and top right boundaries.
    public GameObject blBoundary;
    public GameObject trBoundary;

    //Position of the boundaries.
    private Vector3 blBoundaryPos;
    private Vector3 trBoundaryPos;

    // Event to let game manager know when the player has been killed/lost a life
    public System.Action playerKilledEvent;

    private Vector3 playerInitialPosition = new Vector3(0, -4, 0);

    private AudioSource audioDataLaser;

    private void Awake()
    {
        //Get the position of the boundaries
        blBoundaryPos = blBoundary.transform.position;
        trBoundaryPos = trBoundary.transform.position;

        audioDataLaser = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Player Movement
        //Detect the players input and hold the positional change in x and y direction.
        playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //If player is at the boundary x positions,
        if ((transform.position.x - 0.78 <= blBoundaryPos.x && playerMovementInput.x == -1) || (transform.position.x + 0.69 >= trBoundaryPos.x && playerMovementInput.x == 1))
        //- and + of values is required as thetransform.position refers to the centre of the image but I need edges of the ship to stay in bounds
        {
            //their movement in that direction is set to 0 meaning no movement
            playerMovementInput.x = 0;
        }
        //If player is at the boundary y positions,
        if ((transform.position.y - 0.95 <= blBoundaryPos.y && playerMovementInput.y == -1) || (transform.position.y + 0.95 >= trBoundaryPos.y && playerMovementInput.y == 1))
        {
            //their movement in that direction is set to 0 meaning no movement
            playerMovementInput.y = 0;
        }

        //Adds on the position moved through input to the position of the player, * by T.dT to adjust for difference in frame rates
        transform.position += new Vector3(speed * playerMovementInput.x * Time.deltaTime, speed * playerMovementInput.y * Time.deltaTime);

        //Check if players presses space or m1
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            //If they do, the shoot function is called
            Shoot();
        }
    }


    public Laser laserPrefab;
    //Bool to keep track of if the laser is active
    private bool laserActive = false;
    //Reference to the players/ships canon game object
    public GameObject canon;

    private void Shoot()
    {
        // If laser is not active,
        if (!laserActive)
        {
            // Instantiate a laser at the canon's position
            Laser laser = Instantiate(laserPrefab, canon.transform.position, Quaternion.identity);
            audioDataLaser.Play();

            //Laser destroyed event is added to the laser
            laser.laserDestroyed += LaserDestroyed;

            //Sets laserActive to true as it has now been fired
            laserActive = true;
        }
    }

    private void LaserDestroyed()
    {
        //Sets laserActive to false as it has now been destroyed
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if player has collided with a invader, Boss invader or a missile
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader")
           || collision.gameObject.layer == LayerMask.NameToLayer("Missile")
           || collision.gameObject.layer == LayerMask.NameToLayer("Boss Invader"))
        {
            //Invokes the playerKilled event if it has collided
            playerKilledEvent.Invoke();
        }
    }

    public void SpawnPlayer()
    {
        //Set the player's position to the initial position.
        transform.position = playerInitialPosition;
        //Activate the players game object.
        gameObject.SetActive(true);
    }
}
