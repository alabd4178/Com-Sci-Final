using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 direction;

    public float speed;

    /*
        
        much like the player's bullets, we have the
        direction vector two and the speed float.

        speed is used for the same thing, but as I
        explained in the boss script direction is 
        made to point towards the player and then
        it moves on that line. because we only set
        the direction once, the bullet moves in a 
        straight line.

    */

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

    }// end update

    /*
        
        woooowie, just like bullet we move the enemy bullet
        in the direction of direction and multiply that by
        speed and time.deltaTime wooooow!

    */

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") != true)
        {
            Destroy(this.gameObject);

        }// end if

        /*
            
            just like the player's bullet we check the trigger,
            unlike the player bullet we check to see if the
            trigger is not anything with the tag enemy. if it isn't
            we destroy the bullet, in other word if bullet hits anything
            besides something with the tag enemy we destroy the bullet.
            
        */ 

    }// end on trigger

}// end class
