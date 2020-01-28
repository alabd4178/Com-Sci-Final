using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBrain : MonoBehaviour
{
    public int doorDirection;
    bool hastriggered = false;

    /*
        
        the int door brain is used to determine which door this
        is, it is public because I set it to be between 1 - 5 
        on the actual door objects. each room prefab has a door 
        object, this door object will have it's door direction
        set to corispond with what ever door it is. 1 is left,
        2 is up, 3 is right, 4 is down, 5 is used to determine 
        if the door is a trapdoor leading down to the next floor.

        has triggered is used later to check if the door's collider
        has already been triggered by the player, this stops the door
        from running the trigger code more than once.

    */

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (hastriggered == false)
        {
            if (RoomManager.instance.currentRoom.locked == false)
            {
                if (collider.CompareTag("Player"))
                {

                    hastriggered = true;
                    RoomManager.instance.switchRoom(doorDirection);

                    Destroy(gameObject);

                }// end if

            }//end if

        }// end if

        /*
        
            if the the door has not been triggered yet, we check if the doors
            are locked. if they are not we check if the player is the one who
            triggered the collider, if it is we set has triggered to true, run
            the switch room function and input door direction as the int required,
            after that we destroy the door.

        */

    }// emd on trigger

}// end class
