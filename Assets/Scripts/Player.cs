using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of " + this + " found");

        }// end if

        instance = this;

    }// end awake

    /*
        
        okay it's an instance, refer to another file with an instance if you still
        don't get it at this point cause I aint explaining it again

    */

    public bool paused;
    public float speed;

    /*
        
        paused is used to stop the player from responding to controls
        when paused is set to true, speed is used to set how fast the
        player will move and it is public so I can set it in the inspector

    */

    public Rigidbody2D rb;
    private SpriteRenderer spr;
    public Animator animator;

    /*
        
        I have a public rigid body because other scripts need to access the
        player rigid body, they need to access it to move the player around

        the sprite renderer spr is private because nothing else needs to
        access it, I have a sprite renderer so I can change the alpha of 
        the player when he gets hit

        the animator is public because the script needs to know it is there
        to access the properties of the animator,
        the reason for having and animator is quite simple. I need it to 
        animate the player... kinda common sense

    */


    public Transform firePoint;
    public GameObject bulletPrefab;

    /*
        
        these two public game objects are both used for shooting, fire
        point is a transform that is set to a game object that is 
        always at the x and y points of the gun's barrel. we spawn the
        bullets at this point so the bullets look like they are being
        shot from the gun.

        the bullet prefab is used to spawn the bullets, pretty simple
        becuase you should get why I used public game objects as 
        prefabs by now,

    */

    private Vector2 moveVelocity;

    // move velocity is a vector two that is used to move the player

    private float timeBtwShots;
    public float startTimeBtwShots;

    /*
        
        these are the same timers that I used in the enemies and the
        boss, just reffer to those if you need a refresher.

    */

    public int startHeath;
    int hearts;
    bool invincible;

    /*
        
        start health is used to hold how much health the player should
        start every run with, hearts is used to count how many hearts the
        player currently has, and the bool variable invincible is used to
        track if the player is invincible or not

    */

    private float invincibleTime;
    public float startInvincibleTime;

    // same type of deal as the first timers.

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        paused = false;

        hearts = startHeath;
        GUI.instance.updateGUI(hearts);

    }// end start

    /*
        
        when ever the player gets added to the scene this
        code will run, in a nut shell it sets the rigid body
        and the sprite renderer to the corisponding 
        components that are already attached to the player 
        game object.

        paused is set to false, we set hearts to the value of
        starting health and then we update the GUI so we have
        the correct amount of hearts.

    */

    void Update()
    {
        if (paused != true)
        {
            // as long as paused is false all the code here will run


            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal 1"), Input.GetAxisRaw("Vertical 1"));
            moveVelocity = moveInput.normalized * speed;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

            /*
        
                these three lines are used for movement, basicaly the first thing it does
                is get the inputs from the horizontal 1 and vertical 1 inputs. Unity has
                a built in feature that allows me to set these inputs, so for the horizontal
                inputs I use a and d, and for the vertical inputs I use w and s.

                the input class basicaly makes it so that the value of the component that it is
                working with is either -1, 0, or 1. so we can have the move velocity variable equal
                to vectors that point in the 8 cardinal and ordinal directions.

                next thing we do ones we have the move Input vector two is I set it to the move velocity
                vector two multiplied by the speed variable.

                lastly I use the MovePosition method that can be used to move rigid bodies, to explain
                whats happening, I basicaly set the rigid bodies position a step away from the original
                point in the direction of move velocity. I multiply it by Time.fixedDeltaTime to smoothen
                out the whole thing and make the player movement fluid and easy to control
                
            */

            Vector2 fireInput = new Vector2(Input.GetAxisRaw("Horizontal 2"), Input.GetAxisRaw("Vertical 2")).normalized;

            animator.SetFloat("Horizontal", fireInput.x);
            animator.SetFloat("Vertical", fireInput.y);
            animator.SetFloat("Magnitude", fireInput.magnitude);


            /*
        
                all of this is used for firing, I use the same logic to set fire input that
                I used to set the move input the only difference being which inputs it takes.
                
                for the fire inputs in the horizontal direction I use left and right, for the
                vertical direction I use up and down.

                after that I set the float variables I have in the animator that I use as
                peramiters to animate the character to their corisponding values from fire
                input.

            */

            if (fireInput.magnitude > 0)
            {
                if (timeBtwShots <= 0)
                {
                    Shoot(fireInput.x, fireInput.y);

                    timeBtwShots = startTimeBtwShots;

                }// end if

            }// end if

            timeBtwShots -= Time.deltaTime;

            /*
                
                if we are pressing any of the keys to fire a shot,
                the magnitude will always be greater than 0.

                since the magnitude is greater than zero we check if
                the shot timer is less or equal to zero, if it is we
                run the shoot function and input fire input as the x
                and y values required. lastly we reset the shot timer.s
                
                once we've done all that i have the tick down out side of
                the if statment, this is because if it wasn't you would
                have to keep holding down the firing buttons to tick down 
                the timer.

            */ 

            if (invincible == true)
            {
                invincibleTime -= Time.deltaTime;

            }// end if

            if (invincibleTime <= 0)
            {
                invincible = false;

                spr.color = Color.white;
            
            }// end if

            /*
                
                  the last two things in void update are the basic timers, the only
                  special thing these ones do is set invincible to false and reset the 
                  alpha of the player sprite when the timer runs out.
                
            */

        }// end if

    }// end update

    void Shoot(float x , float y){

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (x != 0 && y >= 0)
        {
            bullet.GetComponent<Bullet>().direction = Vector2.right * (x / Mathf.Abs(x));
        }// end if
        
        else
        {
            bullet.GetComponent<Bullet>().direction = Vector2.up * (y / Mathf.Abs(y));
        
        }// end else

        /*
            
            the shoot function is pretty straight forward, it spawns a bullet from the
            bullet prefab at the fire point. 
            
            next we do a little bit of simple logic
            to check wich way the bullet should move, these if statments are to overide
            which direction the bullets move based on the state of the player.

            if an x button is being pressed and the down button is not, then the if statment
            will run. what the if statment does is make the bullet's x component equal 1 and
            it's y component equal 0, next it multiplys that by x over the absolute value of x.
            in other words by deviding x by x we always get one, but if x is less than zero we will
            get negative one, so by multiplying the vector2 by either negative 1 or 1 we will make
            the bullet move in the correct direction
            
            if y is less than zero the else statment will run, the else statment does the same
            thing as the if statment, but with y instead of x.
        */ 

    }// end shoot

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Hurt();

        }// end if
    
    }// end ontrigger

    /*
        
        when the collider on the player is triggered we check if it was 
        triggered by something with the enemy tag, if it was we run
        the hurt function

    */

    void Hurt()
    {
        if (invincible != true)
        {
        // if the player isn't invincible continue

            if (hearts <= 0)
            {
                GameManager.instance.gameOver();

            }// end if

            // if the player has no more hearts when this function was triggered we end the game

            else {

                invincible = true;

                invincibleTime = startInvincibleTime;

                hearts--;

                spr.color = new Color (255 ,255, 255, 0.25f);

                GUI.instance.updateGUI(hearts);
            
            }// end else

            /*
        
                if we don't have to end the game we set invincible to true,
                and reset the invincible timer to it's starting value.
                Next we lower hearts by one and set the alpha of the player
                sprite to 25%. Lastly we update the GUI so the change is 
                shown.

            */

        }// end if

    }// end hurt

}// end class
