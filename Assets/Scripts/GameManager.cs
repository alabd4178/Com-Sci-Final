using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of " + this + " found");
        }

        instance = this;
    }// end awake

    /*
        
        Allow me to explain what an instance is, an instance is used
        to basicaly make a static container of the code file that the 
        instance is set to. It can be accessed by other classes when
        they import the class and create a variable equal to the
        instance. 
        
        *NOTE* see void start for more details.

    */

    LevelGenerator levelGenerator;
    RoomManager roomManager;
    
    Player player;
    Camera cam;
    public int numberOfSteps;


    /*
        
        I use the level generator and room manager class in
        the game manager, so I define them to use as a local
        class so i can set them to the instance later on.

        player is used so I can access instantiate the player's
        instance and access the paused variable inside.

        cam is the local name used for the camera class, this allows me to
        move the camera around the sceen, and it's here so I can do the pan
        effect.

        number of steps is used in the code that makes the camera pan, and it's
        used to hold how many steps it takes in between moving from the original
        point to the new point
    */

    private float roomPosX;
    private float roomPosY;

    /*
        
        room pos x and room pos y are used to hold the room pos x and y from
        room manager, this allows us to know where the player and the camera
        should be going when we start the game, when we change rooms, and when
        we move to the next floor.

    */ 

    public GameObject menu;
    public GameObject loseScreen;
    public GameObject loadScreen;

    public GameObject map;
    
    public GameObject game;
    public GameObject playerPrefab;

    /*
        
        all of the game objects from menu to load screen are canvases that
        are used for screens that have text and sometimes buttons.
        for example lose screen has the restart button on the text, and it 
        has the image. 

        the reason they are game objects is so I can turn them off and on, 
        making them only show up when needed.

        pay no heed to map, it is a feature I didn't have time to impliment.

        game is connected to the Game game object on the scene, because  it 
        stores the room manager, level generator, and most importantly the 
        Game UI it also needs to be turned off when the menus are showing.

        lastly the player prefab is linked to the player prefab in my assets,
        I do this because I need to spawn the player and I had to include the 
        prefab in the script so I could spawn it.

    */

    private void Start()
    {

        levelGenerator = LevelGenerator.instance;
        roomManager = RoomManager.instance;
        
        cam = Camera.main;

        loadMenu();

    }// end start

    /*
        
        in void start we do some quick setup, but before I go any further
        let me explain what the first two lines are doing. Part of the joy
        of using instances is that we have to set a variable equal to the 
        instance, that's exactly what those lines are doing. So in short,
        I'm setting level generator equal to the instance of LevelGenerator
        

        after that we link cam to the main camera on the stage so it knows 
        what camera to move, then we call load Menu.

    */

    public void loadMenu()
    {
        menu.SetActive(true);
        game.SetActive(false);
        map.SetActive(false);
        loseScreen.SetActive(false);
        loadScreen.SetActive(false);

    }// end loadMenu

    /*
        
        load menu first turns on the menu, and then it deactivates every other 
        game object that is linked in the script.

    */ 


    public void startGame()
    {
        menu.SetActive(false);
        game.SetActive(true);
        map.SetActive(false);
        loseScreen.SetActive(false);
        loadScreen.SetActive(false);

        loadLevel();

    }// end startGame

    /*
        
        start game does basicaly the same thing, but instead it turns off the menu
        and turns on game. after that it calls load level to do all the stuff to 
        load the game, hence the name.

    */ 

    public void gameOver()
    {

        menu.SetActive(false);
        game.SetActive(false);
        map.SetActive(false);
        loseScreen.SetActive(true);
        loadScreen.SetActive(false);

    }

    /*
        
        game over is load menu but for the
        lose screen.

    */

    public void reset()
    {

        roomManager.deleteOldRoom();

        while (roomManager.listofEnemies.Count > 0)
        {
            Destroy(roomManager.listofEnemies[0]);
            roomManager.listofEnemies.RemoveAt(0);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }

        Destroy(player.gameObject);

        startGame();
    }

    /*
        
        reset is actualy different. reset is paired to the restart button, so when 
        you click the restart button it calls this function.

        first reset calls the delete old room function in room manager (see room manager for details)
        next it runs a while loop that will run as long as there are still enemies in the enemy list,
        in the while loop we just keep deleting the enemy in the first spot in the enemy list.

        once there are no more enemies in the list we run a for each loop that deletes all game objects 
        with the tag enemy, this is more for enemy bullets because they also have the enemy tag

        once all the enemy bullets are gone we do the same thing but we look for objects with the bullet 
        tag, meaning we are destroying the player bullets.

        lastly we destroy the player, and then we start the game again.

    */

    void loadLevel()
    {
        levelGenerator.generateLevel();

        //levelGenerator.createMap();
        
        roomManager.startLevel();

        roomPosX = roomManager.roomPosX;
        roomPosY = roomManager.roomPosY;

        cam.transform.position = new Vector3(roomPosX, roomPosY, -10);

        Instantiate(playerPrefab, game.transform);

        player = Player.instance;
        player.rb.position = new Vector3(roomPosX, roomPosY, 0);

    }// end loadLevel

    /*
        
        load level is more like a compiler of code that does all the work to generate
        the game. *NOTE* create map is commented out because I did not fully get it working

        we call generate level which does all the lifting to make the dungeon (see level generator for details)

        next we call start level(see room manager for details)

        *NOTE* start level basicaly sets up the game's first room, so the room pos variables are set to that of
        the starting room

        after that we set room pos x and room pos y to the starting room's x and y positions, this allows us to
        move the camera to the starting room's x and y positions. 

        finaly we instantiate the player from the player prefab, and set it to the instance of player, once
        that has happened we move the player to the starting room's x and y coordiantes.

    */

    public IEnumerator newFloor()
    {
        menu.SetActive(false);
        game.SetActive(false);
        map.SetActive(false);
        loseScreen.SetActive(false);
        loadScreen.SetActive(true);

        roomManager.deleteOldRoom();

        while (roomManager.listofEnemies.Count > 0)
        {
            Destroy(roomManager.listofEnemies[0]);
            roomManager.listofEnemies.RemoveAt(0);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }

        yield return new WaitForSeconds(3);

        menu.SetActive(false);
        game.SetActive(true);
        map.SetActive(false);
        loseScreen.SetActive(false);
        loadScreen.SetActive(false);

        levelGenerator.generateLevel();
        
        //levelGenerator.createMap();

        roomManager.startLevel();

        roomPosX = roomManager.roomPosX;
        roomPosY = roomManager.roomPosY;

        cam.transform.position = new Vector3(roomPosX, roomPosY, -10);
        player.rb.position = new Vector3(roomPosX, roomPosY, 0);
    }

    /*
        
        new floor isn't that special, if you look at the code it basicaly does the same thing
        as reset and start game, with a three second delay between the two funcions.

        the only other diference is it doesn't make a new player.

    */ 

    public void moveCamera(Vector3 newPos)
    {
        float newPlayerX = player.rb.position.x + roomManager.playerPos.x;
        float newPlayerY = player.rb.position.y + roomManager.playerPos.y;

        player.rb.position = new Vector3(newPlayerX, newPlayerY, 0);
        player.paused = true;

        StartCoroutine(MoveCamera(newPos));
    }// end moveCamera

    /*
        
        move camera is called when we load a new room in room manager. It gets the new player x 
        and the new player y that will be used to move the player to the correct spot when we load a 
        new room, and it pauses the player. We do this because right after that I start a 
        coroutine that actualy moves the camera, and I don't want the player to be able to move
        while we move the camera.

        the move camera coroutine first finds how big each step should be, it does this by taking
        the new pos it was inputted, and subtracts that by the current position of the camera.
        this gets the total distance we have to move, and by dividing that total distance we
        get how big each step should be. I should note that step size is a vector three because
        I needed to be able to do math with a vector and the camera position is a vector3,
        because vector twos and vetor threes don't get along I couldn't use a vector 2.

        once we have the step count, we run a for loop that runs as many times as the number of steps.
        (I set number of steps to 10 in the inspector, so the for loop will loop 10 times.)
        each time we run the for loop the cam takes a step, then we run the delay WaitForFixedUpdate. 
        It is important to know that fixed update is called every 0.02 seconds, so to put it simply the 
        line "yield return new WaitForFixedUpdate();" is actually just saying wait 0.02 seconds. 

        once the camera has been moved to the new spot we un pause the player allowing the player to be
        controlled again

    */

    IEnumerator MoveCamera(Vector3 newPos)
    {
        Vector3 stepSize = (newPos - cam.transform.position) / numberOfSteps;

        for (int i = 0; i < numberOfSteps; i++)
        {
            cam.transform.position += new Vector3
            {
                x = stepSize.x,
                y = stepSize.y,
                z = stepSize.z
            };

            yield return new WaitForFixedUpdate();
        }

        // after the for loop it un-pauses the player
        player.paused = false;
    }// end coroutine

}// end class
