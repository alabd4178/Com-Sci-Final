using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public bool left;
    public bool up;
    public bool right;
    public bool down;

    /*
        
        these boolean are used to check off if a room has a door in the
        corisponding direction. for example if a door has it's left boolean
        set to true then it has a door that opens to it's left.

    */

    public bool doNotSpawnEnemies;
    public bool locked;
    public bool bossRoom;

    /*
        
        do not spawnEnemies is used to check if enemies should be spawned or not,
        so if the room has been cleared or if it is the start room then it is set
        to true.

        locked is used to check if the room is locked or not, if it isn't then 
        we can change rooms when ever a door is touched by the player.

        boss room is used to define if the room is a boss room or not, simply put,
        if boss room is set to true then the room is a boss room

    */

    public SpawnEnemy spawnEnemy;

    public Room()
    {

        left = false;
        up = false;
        right = false;
        down = false;

        doNotSpawnEnemies = false;
        locked = false;
        bossRoom = false;

    }// end construtor code

    /*
        
        the constructor code just makes sure each room starts
        with it's bool variables set to false.

    */

}// end class
