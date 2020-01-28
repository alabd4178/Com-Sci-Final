using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // *NOTE* the enemy is a dumber version of the boss *see the Boss class*
    // what i mean by this is that it uses a lot of the same code

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject bulletPrefab;
    
    /*
        
        look here i used even the same names as the other timers
        I used in the boss script, bullet prefab is used in the same
        way as well. I use it to instantiate the bullets

    */

    private Transform player;

    public float speed;
    public int health;

    /*
        
        the player transform is used to find the x, y 
        co-orddinates of the player. 
        
        speed is used to set the speed of the enemy,
        and health is used to set the number of hits
        it takes to kill the enemy. They are both 
        public so I can set them in the inspector.

    */

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots + Random.Range(0, 0.51f);

    }// end start

    /*
        
        this code runs when the enemy first spawns, it finds the player
        and sets the player transfrom to the transfrom of the player. 
        next it set time between shots to the starting value, and add 
        a small extra amount from 0 - 0.5 seconds. this just adds a little
        extra delay so all the enemies don't fire at the same time, this 
        spices it up and makes things a little more interesting

    */

    private void Update()
    {
        Vector2 moveMe = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        transform.position = moveMe;

        if (timeBtwShots <= 0)
        {
            GameObject bullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;

            Vector2 bulletDir = (player.position - transform.position).normalized;

            bullet.GetComponent<EnemyBullet>().direction = new Vector3(bulletDir.x , bulletDir.y, 0);

        }// end if

        else
        {
            timeBtwShots -= Time.deltaTime;

        }// end else

        /*
        
            every frame we set move me to point towards the player,it used the same
            code as the boss but it will only ever move towards the player.

            after that we just run the same if statments in the boss script to
            make the enemy fire.

        */

    }// end update

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            if (health < 0)
            {
                RoomManager.instance.killEnemy(gameObject);

            }// end if

            else
            {
                health--;

            }// end health

        }// end if

        /*
        
            just like the boss, if a bullet triggers the
            collider we check if health is less than zero.
            if it is we run the kill enemy function in 
            room manager and input itself as the game object 
            that the funtion requires.

            if we don't need to kill the enemy we lower the health
            by one (surprise surprise)

        */

    }// end on trigger

}// end class
