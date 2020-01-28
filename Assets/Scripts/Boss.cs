using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public float speed;
    public int health;

    /*
    
    speed and health are public variables that we set in the inspector
    this allows me to tweak them quickly and fine tune the game, speed 
    is how fast the boss moves, and health is how many hits it takes to
    kill the boss.

    */

    public float retreatDistance;
    public float tooClose;

    private Transform player;

    /*

        retreatDistance and tooClose are floats that I set in the inspector,
        retreat distance is used to tell the boss if the player is within 
        the range of running away. Too close is used to tell if the player
        is too close.

        the two floats are used in code later and if the distance from the boss
        to the player is less than retreat we make the boss retreat, if it is less
        than too close we make the boss charge the player


        the player transform is set in the inspector to the player, this allows me
        to find the x and y positions of the player. 

    */

    private float timeBtwShots;
    public float startTimeBtwShots;

    private float chargingTimer;
    public float startChargingTimer;

    private float standingStillTimer;
    public float startStandingTimer;

    /*
        
        these are all used as timers, they are paired as how long 
        the timers are and the current time of the timer. 

        for example timeBtwShots is the current time that the timer
        that tracks the time between shots is at, and start time
        between shots is the duration of the delay. 

        the timers are pretty simple, basicaly if the current time is less or
        equal to zero we run the stuff that should happen after the delay.
        If the delay isn't one of thise things then we tick down the timer by
        Time.deltaTime, basicaly it makes it work with real time.

    */ 

    Vector2 moveMe;

    public GameObject bulletPrefab;
    public GameObject trapdoorPrefab;

    /*
        
        move me is a vector 2 that is used to move the boss, and it is set to 
        x and y segments in code.

        the two public game objects are used to spawn in the prefab that is set
        to that name in the inspector, so if i want to make a bullet I instantiate 
        bullet prefab

    */ 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;

        standingStillTimer = startStandingTimer;

    }// end start

    /*
        
        in the start function we run code that should run when ever we spawn the boss,
        basicaly this is setup code that we need to run.

        first i set the player game object to the object with the tag player, I do this
        because it is the simplest way to always select the player.

        next we set the two timers to the values that they should be when they start,
        in other words we make it so the delays can run properly

     */ 

    private void Update()
    {
        if (chargingTimer > 0)
        {
            charge();
            chargingTimer -= Time.deltaTime;

        }// end if

        /*
        
            if the charging timer is greater than zero,
            we make the enemy charge and tick down the 
            timer, this will eventualy make the boss stop charging

            if charging timer isn't > 0 we run the else statment below.

        */

        else
        {

            if (Vector2.Distance(transform.position, player.position) <= retreatDistance)
            {
                if (Vector2.Distance(transform.position, player.position) <= tooClose)
                {
                    charge();

                }// end if

                else
                {
                    moveMe = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                    transform.position = moveMe;

                }// end else

            }// end if

            /*
        
                in this if statment we check if the distance to the player is less than 
                retreat distance, if it is we run the code inside.

                if the player is too close we run the charge function, otherwise we make the
                enemy retreat.

                to do this we set move me to a vector two that points in the direction of the
                player, and make it move along that line backwards in turn making it retreat from
                the player
                
            */

            else if (Vector2.Distance(transform.position, player.position) >= retreatDistance)
            {
                moveMe = Vector2.zero;

            }// end else if

            /*
        
                if the player isn't in the retreat distance we set move me to zero, making the
                enemy stop.

            */

        }// end else

        if (moveMe == Vector2.zero)
        {

            if(standingStillTimer <= 0)
            {
                standingStillTimer = startStandingTimer;
                StartCharging();
                // if standing still for how long start standing still timer is start charging.

            }

            else
            {
                standingStillTimer -= Time.deltaTime;

            }// end else if

        }// end if

        /*
        
            if move me is equal to zero, the boss isn't moving, we check if the boss
            has been standing still for the duration of start standing still timer.
            if it is we set standing still timer back to to start standing still timer, 
            and we tell the boss to start charging.

            if that isn't true we tick down the timer.
            
        */

        if (timeBtwShots <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;

            Vector2 bulletDir = (player.position - transform.position).normalized;

            bullet.GetComponent<EnemyBullet>().direction = new Vector3(bulletDir.x, bulletDir.y, 0);

        }// end if

        else
        {
            timeBtwShots -= Time.deltaTime;

        }// end else

        /*
        
            if the time between shots timer is less or equal to zero, we make make 
            a bullet by instantiating the bullet prefab next we set it's transform to
            the x and y of the boss, and set it's rotation to non existance.

            next we make the vector two that the enemy bullet moves along a line that points
            towards the player. last we set the x and y components of the Enemy Bullet direction
            vector 3, to the bullet direction x and y segments.

        */

    }

    void StartCharging()
    {
        chargingTimer = startChargingTimer;
    }

    /*
        
        start charging sets the charging timer to the stating value,
        in turn this will triger the if statment I had earlier that 
        checks if the charging timer is greater than zero

    */

    void charge()
    {
        moveMe = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        transform.position = moveMe;

    }// end charge

    /*
        
        charge is called and it sets move me to point toward the player and we make it move
        towards the player

    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            StartCharging();

        }// end if

    }// end on collision

    /*
        
        if the boss bumps into a wall we spook him and we call start
        charging

     */

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            if (health < 0)
            {
                RoomManager.instance.killEnemy(gameObject);

                GameObject.Instantiate(trapdoorPrefab, transform.position, Quaternion.identity);

            }// end if

            else
            {
                health--;

            }// end else

        }// end if

        /*
        
            if the trigger collider is triggered by a bullet we check if health is less than zero,
            if it is we run the kill enemy function in room manager and input the boss as the object 
            that room manager needs to be input.

            if we don't have to kill the boss we subtract one from health

        */

    }// end on triger

}// end class
