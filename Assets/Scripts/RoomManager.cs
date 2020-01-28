using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of " + this + " found");
        
        }// end if

        instance = this;
        
        // oh wow an instance! you better know what they do by now...

        placedRooms = new List<GameObject>();
        listofEnemies = new List<GameObject>();

        /*
            
            placed rooms is used to hold the rooms that have
            actually been spawned.

            list of enemies is used to hold all of the enemies
            that are spawned when you enter a room
            
        */

    }// end awake

    public float roomWidth;
    public float roomHeight;

    public float roomPosX = 0;
    public float roomPosY = 0;

    /*
        
        room width and room height are floats that I set in the inspector
        to the units of the length and width of the physical rooms that are
        spawned in. 

        room pos x and room pos y are used quite often, but in this 
        script they are used to determine where the room that is being 
        spawned in should be.

    */ 
    
    public float playerBump;

    //player bump is used to store how far over the player needs to be bumped after going through a door

    public Vector3 playerPos;
    Vector2Int playerRoomIndex;

    /*
        
        player pos is used to keep track of the player, because it is
        a public vector 3 because game manager needs to access it. it 
        needs to access it because it uses it to know where the player
        should be when a new room is loaded.

        player room index is used to hold the point that the player is at,
        by knowing where the player is we can then find out what room we
        should load when the player goes into a new room.

    */

    public GameObject[] rooms;
    public Transform gameParent;

    /*
        
        rooms is a game object array that holds all of the phsical room
        prefabs, it is used to spawn in the correct room.

        game parent is used to link the game game object and parent the
        new rooms to it.

    */

    MapData mapData;
    GameManager gameManager;

    /*
        
        map data and game manager are imported so I can use their instances
        later on.

    */

    List<GameObject> placedRooms;
    public List<GameObject> listofEnemies;

    public Room currentRoom;

    /*
        
        the placed rooms list is used to hold the information of the rooms that
        are currently active on the scene, this allows keep only one room active
        on the scene at a time.

        list of enemies is for one thing, unlike flash I don't need a list or an
        array to preform hit detection. Instead that is all done on each enemy on
        it's own, list of enemies soul purpose is to hold the number of enemies
        so I can check if the room has been cleared or not. When the length of 
        list of enemies is equal to zero then we know that the room is clear.

    */
    
    public void startLevel()
    {
        mapData = LevelGenerator.instance.mapData;
        gameManager = GameManager.instance;

        loadRoom(mapData.startingIndex);
    
    }// end start level

    /*
        
        start level is my version of a start function, but because room manager
        starts on the scene I couldn't just use void start. Instead I use start
        level, because that allows me to run this function when ever I start a 
        game.

        we set map data equal to the map data instance that level generator used
        to genereate the dungeon, I do this because I wouldn't get the same data
        other wise

        next we set game manager to it's instance, and then we call load room.
        We input the starting index that map data decide should be the starting room

        if you look at load room it requires a vector two to be input when we call it,
        and that is because it allows me to easily input the room that needs to be loaded
        in.

    */

    void loadRoom(Vector2Int roomIndex)
    {
        currentRoom = mapData.rooms[roomIndex.x, roomIndex.y];

        currentRoom.locked = true;

        // first thing load room does is get the room we just loaded in, then it locks that room's doors.

        if (currentRoom != null)
        {
            // as long as the room we are loading in exist this if statment will run, it should always run.

            string roomType = "Room ";
            
            /*
                
                room type is a string that I use to determine what room should be spawned in,
                in the four if statments below I add letters to the room type string if the room
                needs a door in that direction. So by the if statments are done, the room type 
                string is equal to the name of a prefab in the rooms array. for example if the
                room needs a door in the left and right walls, room type will be equal to Room LR.
                if you look at the rooms array in the inspector then you can see that there is a
                room with that name.

            */
            
            if (currentRoom.left == true)
            {
                roomType += "L";
            
            }// end if

            if (currentRoom.up == true)
            {
                roomType += "U";
            
            }// end if

            if (currentRoom.right == true)
            {
                roomType += "R";
            
            }// end if

            if (currentRoom.down == true)
            {
                roomType += "D";
            
            }// end if

            playerRoomIndex = roomIndex;

            roomPosX = roomIndex.x * roomWidth;
            roomPosY = roomIndex.y * roomHeight;


            /*
                
                once we know the room type we can set the player index equal to that of
                the room we are about to load in, so if the room we are loading in is at
                the point (7,13) then player room index will be equal to (7,13)

                after that we get the x and y position the room will need to be spawned at,
                to do this we take the room index and multiply it by the width and the height
                of the rooms. This makes the map all flow nicely when switching rooms and it
                allows the camera to move smoothly because the rooms all flow into each other.
                
            */

            GameObject roomPrefab = Array.Find(rooms, GameObject => GameObject.name == roomType);
            
            GameObject newRoom = Instantiate(roomPrefab, new Vector3(roomPosX, roomPosY, 0), Quaternion.identity, gameParent);
            placedRooms.Add(newRoom);

            /*
        
                now that we know the type of room and where it should go, we can spawn the room.

                to do that we make a game object called room prefab that is set to the room
                prefab in the rooms array with the name that is equal to room type, so if the
                room type is equal to Room LUR then room prefab will be equal to that room.

                once we have the prefab chosen we make a game object called new room and 
                instantiante the room prefabe at the room pos x and room pos y values we 
                found earlier. we then parent the room to the game game object on the
                scene.

                After that we add the room we just spawned into the placed rooms array, this 
                just allows us to keep track of this room and we can remove it easily because
                of that.

            */

            if (currentRoom.doNotSpawnEnemies != true)
            {
                StartCoroutine(delaySpawn());
                currentRoom.doNotSpawnEnemies = true;
            
            }// end if

            else
            {
                isRoomClear();
            
            }// end else

            /*
        
                Finaly we check if we should spawn enemies or not. If we need to
                spawn enemies because if do not spawn enemies is not true then we
                start the delay spawn coroutine and set the do not spawn enemies
                of the current room to true. so now once this room gets loaded again
                it's do not spawn enemies will be true, meaning we skip over the if
                statment and go to the else statment.

                the else statment just runs is room clear.

            */

        }// end if

    }// end load room

    public void switchRoom(int doorDirection)
    {
        playerPos = Vector3.zero;
        Vector2Int newRoomPos = playerRoomIndex;

        /*
        
            first thing we do is set the spot that the
            player should go to in the new room equal too
            zero, then we make a vector 2 that stores what
            room the player is currently in.

        */

        if (doorDirection == 1)
        {
            newRoomPos.x--;
            playerPos.x -= playerBump;
        
        }// end if

        if (doorDirection == 2)
        {
            newRoomPos.y++;
            playerPos.y += playerBump;
        
        }// end if

        if (doorDirection == 3)
        {
            newRoomPos.x++;
            playerPos.x += playerBump;
        
        }// end if

        if (doorDirection == 4)
        {
            newRoomPos.y--;
            playerPos.y -= playerBump;
        
        }// end if

        /*
        
            These four if statments do basicaly the same thing,
            they check to see what door the player went through
            and then select the room in the array that goes there,
            and then set the player pos equal to the correct side
            of the room. for example, if the door direction is 3
            (right) then we add one to the x of the room position
            and then add player bump to the x of the player pos.
            basicaly what this does is select the spot that the player
            moved to in the 2d array that holds the rooms, and then 
            sets the player pos to make it appear on the correct side
            of the room

        */

        if (doorDirection == 5)
        {
            gameManager.StartCoroutine(gameManager.newFloor());
        
        }// end if

        /*
        
            if the door has it's door direction set to five then it leads
            to a new floor, so we run the new floor coroutine in game
            manager. (*see game manager to see how it works*)

        */

        else
        {
            loadRoom(newRoomPos);
            gameManager.moveCamera(new Vector3(roomPosX, roomPosY, -10));
            deleteOldRoom();
        
        }// end else

        /*
        
            okay let me explain why this code is in an else statment, if you
            look at the if statment above it you will see that it is different
            that the four if statments above it. this code will only run if the
            door direction isn't 5, which is good because when I first implimented
            new floors these three lines would cause errors.

            now that you know why they are in an else statment let me explain what they
            do, first we call load room and input new room pos as the room it should 
            spawn. Next we call move camera and input a new vector 3 with the room pos
            x and room pos y variables as the x and y components we also put the z 
            component equal to -10 because if I didn't we wouldn't be able to see most of
            the game.

            *NOTE* z values in 2d space still exist, but they function more as layers rather
            than an actual z axis. because the camera is set to negative 10, the bigger the number
            the farther back it is and the closer it is to negative 10 the further up it is.

        */

    }// end switch room

    public void deleteOldRoom()
    {
        Destroy(placedRooms[0]);
        placedRooms.RemoveAt(0);
    
    }// end delete old room

    /*
        
        delete old room just deletes the room at spot 0 in the
        placed rooms array, so in short it kills the old room.

    */

    IEnumerator delaySpawn()
    {
        yield return new WaitForEndOfFrame();

        if (currentRoom.bossRoom == true)
        {
            currentRoom.spawnEnemy.spawnBoss(listofEnemies);
        
        }// end if
        
        else
        {
            currentRoom.spawnEnemy.spawnBaddies(listofEnemies);
        
        }// end else
    
    }// end delay spawn

    /*
        
        delay spawn is used to spawn the enemies at the start of the next frame, so
        once everything is done and the room has been placed we spawn a boss if
        it is the boss room, or the enemies if it is a normal rooms

    */

    void isRoomClear()
    {
        if (listofEnemies.Count == 0)
        {
            currentRoom.locked = false;
        
        }// end if
    
    }// end is room clear

    /*
        
        is room clear just double checks if there
        are any enemies left or not, if there
        are no enemies left we un lock the doors.

    */
    public void killEnemy(GameObject enemy)
    {
        listofEnemies.Remove(enemy);
        Destroy(enemy);

        isRoomClear();
    
    }// end kill enemy

    /*
        
        every time we kill an enemy we check if the room
        is clear or not, once it finaly is we can unlock
        the doors.

    */

}// end class
