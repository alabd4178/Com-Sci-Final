using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;

    public float speed;

    /*
        
        direction is a public vector three, it is a vector 3 
        because we cannot do math with a vector 2, it is 
        public because I set it in the player script when I spawn it.

        speed is used to make the bullets move at the speed it is set to,
        because it is a public float it can be set in the inspector. (which it is)

    */

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

    }// end update

    /*
        
        every frame we move the bullet in the direction of the
        vector 3 direction, then we multiply direction by speed
        and then multiply it by time.deltaTime. this makes the
        bullet move in a fluid motion, and it makes it move in
        the correct direction.

    */

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Bullet") != true && collider.CompareTag("Player") != true)
        {

            Destroy(this.gameObject);

        }// end if

    }// end on trigger

    /*
        
        when the collider is triggered we check if the object that
        triggered the collider is not another bullet and it is not
        the player. if something else triggered this function we 
        destroy the bullet.

    */

}// end class
