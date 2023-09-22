using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    //Reference to the top right and bottom left boundaries for invaders.
    public GameObject invaderTrBoundary;
    public GameObject invaderBlBoundary;

    //Position of the boundaries.
    private Vector3 invaderTrBoundaryPos;
    private Vector3 invaderBlBoundaryPos;

    // Number of rows and columns for the grid of invaders.
    private int rows = 5;
    private int columns = 19;

    //How far the invaders move each time they move.
    private float distanceInvadersMove = 0.75f;

    //Event to let the GameManager know when an invader has been killed.
    public System.Action<Invader> invaderKilledEvent;

    private AudioSource audioDataMissile;

    private Vector3 initialPosition = new Vector3(0, 1.75f, 0);

    // Array of Invader prefabs to be instantiated.
    public Invader[] invaderPrefabs;

    // Keep track of the number of invaders killed, use it to calculate number of invaders alive which is used to determine how often the invaders move.
    public int totalInvaders => rows * columns;
    public int numOfInvadersKilled;
    private int numOfInvadersAlive => totalInvaders - numOfInvadersKilled;
    private float ratioOfInvadersAlive => (float)numOfInvadersAlive / (float)totalInvaders;

    //How many seconds between each call of FireMissiles
    private float missileFireRate = 0.75f;

    public Missile missilePrefab;

    private void Awake()
    {
        audioDataMissile = GetComponent<AudioSource>();
        invaderTrBoundaryPos = invaderTrBoundary.transform.position;
        invaderBlBoundaryPos = invaderBlBoundary.transform.position;
    }

    private void Start()
    {
        DisplayGrid();
        StartCoroutine("_MoveInvaders");
        InvokeRepeating("FireMissiles", missileFireRate, missileFireRate);
    }

    private void DisplayGrid()
    {
        float spacing = 0.75f;
        float gridWidth = spacing * (columns - 1);
        float gridHeight = spacing * (rows - 1);
        Vector3 centering = new Vector3(-gridWidth / 2, -gridHeight / 2);

        for (int row = 0; row < rows; row++)
        {
            Vector3 rowPosition = new Vector3(0, row * spacing, 0);
            for (int col = 0; col < columns; col++)
            {
                Vector3 colPosition = new Vector3(col * spacing, 0, 0);
                Invader invader = Instantiate(invaderPrefabs[row], transform);
                // Add the function to be called when the invader killed event is invoked.
                invader.invaderKilled += InvaderKilled;
                invader.transform.localPosition = rowPosition;
                invader.transform.localPosition += colPosition;
                invader.transform.localPosition += centering;
            }
        }
    }

    private void MoveInvaders()
    {
        List<Vector3> directions = new List<Vector3> { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) //If invader has been killed (so no longer active in game) the script will move to the next iteration in the foreach loop
            {
                continue;
            }
            if (invader.position.x >= invaderTrBoundaryPos.x)
            {
                directions.Remove(Vector2.right);
            }
            if (invader.position.y >= invaderTrBoundaryPos.y)
            {
                directions.Remove(Vector2.up);
            }
            if (invader.position.x <= invaderBlBoundaryPos.x)
            {
                directions.Remove(Vector2.left);
            }
            if (invader.position.y <= invaderBlBoundaryPos.y)
            {
                directions.Remove(Vector2.down);
            }
        }
        transform.position += directions[Random.Range(0, directions.Count)] * distanceInvadersMove;
    }

    // Coroutine that moves the invaders.
    private IEnumerator _MoveInvaders()
    {
        while (true)
        {
            yield return new WaitForSeconds(ratioOfInvadersAlive + 0.15f);
            MoveInvaders();
        }
    }


    private void InvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        numOfInvadersKilled++;
        invaderKilledEvent.Invoke(invader);
    }

    private void FireMissiles()
    {
        foreach (Transform invader in transform)
        {
            //If invader has been killed (so no longer active in game),
            if (!invader.gameObject.activeInHierarchy)
            {
                //move to the next iteration in the foreach loop
                continue;
            }

            if (Random.value < (1.0f / (float)numOfInvadersAlive))
            {
                Missile missile = Instantiate(missilePrefab, invader.position, Quaternion.identity);
                audioDataMissile.Play();
                missile.GetComponent<SpriteRenderer>().color = invader.GetComponent<SpriteRenderer>().color;
                break;
            }
        }
    }

    public void SpawnInvaders()
    {
        transform.position = initialPosition;
        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }
}
