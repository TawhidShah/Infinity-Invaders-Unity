using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class BossInvaders : MonoBehaviour
{
    //Event to let the GameManger know when a boss invader has been killed.
    public System.Action<BossInvader> bossInvaderKilledEvent;

    public BossInvader bossInvaderPrefab;

    //Number of boss invaders.
    public int totalNumOfBosses = 7;

    //Spacing between each boss invader.
    private float bossSpacing = 2.5f;

    private AudioSource audioDataBossMissile;

    private void Awake()
    {
        audioDataBossMissile = GetComponent<AudioSource>();
    }

    //SPAWNNING
    public void SpawnBossInvaders()
    {
        //Loop through the number of boss invaders.
        for (int bossNum = 0; bossNum < totalNumOfBosses; bossNum++)
        {
            //Spawn a boss invader.
            BossInvader boss = Instantiate(bossInvaderPrefab, transform);

            //Add the boss invader killed event to the boss invader.
            boss.bossInvaderKilled += BossInvaderKilled;

            //Set the boss invader's initial position.
            boss.transform.position = transform.position;

            //Move the boss invader to the right by the spacing multiplied by the boss number.
            boss.transform.localPosition += Vector3.right * bossSpacing * bossNum; 
        }
        InvokeRepeating("FireBossMissiles", missileFireRate, missileFireRate);
    }

    //Keep track of the number of boss invaders killed, use it to calculate number of boss invaders alive which is used to determine the probability of a boss missile being fired.
    public int numOfBossesKilled;
    private int numOfBossesAlive => totalNumOfBosses - numOfBossesKilled;

    // Function that is called when a boss invader is killed.
    private void BossInvaderKilled(BossInvader bossInvader)
    {   
        Destroy(bossInvader.gameObject);
        numOfBossesKilled++;
        // Invoke the boss invader killed event for GameManager.
        bossInvaderKilledEvent.Invoke(bossInvader);
    }

    //BOSS MISSILES
    private float missileFireRate = 0.5f;
    public Missile missilePrefab;

    private void FireBossMissiles()
    {
        foreach (Transform bossInvader in transform)
        {
            if (!bossInvader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1 / (float)numOfBossesAlive))
            {
                Missile missile = Instantiate(missilePrefab, bossInvader.transform.position, Quaternion.identity);
                audioDataBossMissile.Play();
                missile.GetComponent<SpriteRenderer>().color = bossInvader.GetComponent<SpriteRenderer>().color;  
            }
        }
    }
}