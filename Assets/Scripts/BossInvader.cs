using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInvader : MonoBehaviour
{
    private int numOfLasersRequiredToDestroyBoss = 3;

    //Event that allows bossInvaders scripts to know when a boss invader has been killed.
    public System.Action<BossInvader> bossInvaderKilled;

    //Score per boss invader kill
    public int bossInvaderScore = 100;

    //Function that is called when the boss invader collides with something.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the boss invader collides with a laser,
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            //and the number of lasers required to destroy the boss is greater than 0,
            if (numOfLasersRequiredToDestroyBoss > 0)
            {
                //decrement the number of lasers required to destroy the boss.
                numOfLasersRequiredToDestroyBoss--;
            }

            //otherwise invoke the boss invader killed event.
            else
            {
                bossInvaderKilled.Invoke(this);
            }
        }

    }
}
