using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    int width;
    int height;
    int growthMax;

    public Room[,] rooms;
    public int[,] intModel;

    public Vector2Int startingIndex;
    public Vector2Int bossRoomIndex;

    public MapData(int width, int height, int growthMax)
    {
        this.width = width;
        this.height = height;
        this.growthMax = growthMax;

        intModel = new int[width, height];
        rooms = new Room[width, height];
    
    }// end constructor code

    /*
        
        this is constructor code, if you compare it to constructor codes
        in flash the setup is quite similar. The constructor code runs when
        I make a new mapdata, and when making a new map data it requires three
        int variables to be input. It requires a width, a height and a growth max,
        these variables are used to define some limits on the map.

        inside the constructor code we set the width variable outside the constructor code 
        equal to the on input into the function, we do that for the height and growth max
        variables as well

        after that we make a 2D integer array, wich is easiest to think about like a grid.
        The grid will have as many spots as width x height, so if width is 10 and height is
        10 then the array will be 10 x 10, and have 100 spots inside.
        
        now that I've explained 2D arrays let me tell you why it is an integer array, simply
        put I use it to define if a room should be in that spot or not. Simply put if the spot
        has a 1 in it there is a room that needs to go there, and if there is a zero there that 
        is empty space.

        rooms is also a 2d array and it will be the same dimensions as int model, but it is a 2D
        array of the type room class. So if a spot is filled it will be a room. This one is used 
        after we have done all the randomization to the int model array.

    */

    public void PlaceRooms()
    {
        List<Placer> placers = new List<Placer>();

        int randomX = Random.Range(5,16);
        int randomY = Random.Range(5,16);

        // picks somewhere between (5,5) and (15,15)

        placers.Add(new Placer(randomX, randomY));

        intModel[randomX, randomY] = 1;

        int growth = 0;

        /*
        
            First thing place rooms does is make a new list of the 
            class placers, this list is used to contain the placers
            that are used to place the rooms. look at the placer 
            class for more details as to what each object in the 
            array does, because each object is of the class type.

            after we define the array we choose a random x and y
            from 5 - 15, and make a new placer at that spot, then we
            set the spot to 1 because a room will go there. lastly 
            we make a local variable to keep track of the growth.

        */

        while (placers.Count > 0 && growth < growthMax)
        {
            /*
        
                this while loop will run as long as there is a placer in the 
                placers array and as long growth is less than the growth max

            */

            growth++;
            
            // we add one to growth every time the while loop runs

            foreach (Placer placer in placers)
            {
                // we preform the code for every placer in the placers array

                int RandomMoveDir = Random.Range(0, 4);

                switch (RandomMoveDir)
                {
                    default:
                    case 0:
                        movePlacerUp(placer);
                        break;
                    case 1:
                        movePlacerRight(placer);
                        break;
                    case 2:
                        movePlacerDown(placer);
                        break;
                    case 3:
                        movePlacerLeft(placer);
                        break;
                
                }// end switch

                /*
        
                  first thing we do is choose a random number from
                  0, 1, 2, or 3. I use a switch statment to check
                  wich way the placer should move, for example,
                  if the random dir is set to 2 we will move the 
                  placer down by calling the move placer down function
                  and we input the placer that we are currently preforming
                  this code with. in other words because we use a for each
                  we can just take the placer that the for loop is currently
                  preforming with

                */

                intModel[placer.x, placer.y] = 1;
                placer.step++;

                if (placer.step == 5)
                {
                    placers.Add(new Placer(placer.x,placer.y));
                    break;
                }

                /*
        
                    once we move the placer we set the new spot to have a room in it,
                    and we increase the step variable of the specific placer we are 
                    working with in the for loop by one. 

                    if the placer has moved five times then we make a new placer at the
                    same spot.
                    

                */


                if(growth > 5){
                
                    float deathChance = Random.Range(0, 1f);
                    
                    if (deathChance >= 0.75f)
                    {
                        placer.lifespan--;
                        
                        if (placer.lifespan == 0)
                        {
                            placers.Remove(placer);
                            break;
                        
                        }// end if
                    
                    }// end if
                
                }// end if

                /*
        
                    if growth is greater than 5, then we start having a chance to kill the placers,
                    each placer has a life span that can be from 1 - 3.

                    if the random float death chance is greater or equal to 0.75 then remove one from
                    the placer's life span, and if the placer's life span has been reduce to zero we kill the
                    placer and return back to the while loop

                */

            }// end for each

        }// end while
    
    }// end place rooms

    public void SubtractRooms()
    {
        bool[,] roomsToMurder = new bool[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // these two for loops cycle through each x and y position in width x height

                if (intModel[x, y] == 1)
                {
                    bool destroyRoom = true;
                    
                    if (x == 0 || x >= width - 1 || y == 0 || y >= height - 1)
                    {
                        destroyRoom = false;
                    
                    }// end if

                    /*
                        
                        first thing we do is set all rooms for demolition, then we go and see what rooms
                        need to be saved, so the first thing we do after that is check if the room is at
                        the edge of the map's limts. If it is we save it.
                        
                    */ 

                    else
                    {
                        for (int deltaX = -1; deltaX <= 1; deltaX++)
                        {
                            for (int deltaY = -1; deltaY <= 1; deltaY++)
                            {
                                if (intModel[x + deltaX, y + deltaY] == 0)
                                {
                                    destroyRoom = false;
                                
                                }// end if
                            
                            }// end for
                        
                        }// end for
                    
                        /*
                            
                            if the room isn't an edge room we check if the room isn't completly surrounded by 
                            other rooms, what I mean by this is if the eight spots around the room are filled
                            we destroy the room in the middle
                        
                        */ 

                    }// end else

                    if (destroyRoom)
                    {
                        roomsToMurder[x, y] = true;
                    
                    }// end if
                    
                    /*
                        
                        if destroy room is set to true for this current
                        x and y value we set the coresponding spot in the
                        rooms to murder boolean array to true.
                        
                    */
                
                }// end if

            }// end for

        }// end for

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roomsToMurder[x, y])
                {
                    intModel[x, y] = 0;
                
                }// end if
            
            }// end for
        
        }// end for
        
        /*
            
            Lastly we run through the x and y values again and check if that spot
            is set to true in the rooms to murder array, if it is then we set the
            corisponding spot in the int model to true, erasing that room from the
            map.
            
        */ 

    }// end subtract rooms

    /*
        

        *NOTE* all of the move placer functions are practicaly the same, only difference is which direction they move in,
        and what the maximum or minium value they can move too is.

        for example look at move Placer Up, first thing it does is move the placer up
        but if the y of the placer is greater or equal to height it moves it back down
        and then chooses a random direction from 0 - 1 and if it is greater or equal to
        0.5 it moves right, otherwise it will move left.

    */

    void movePlacerUp(Placer placer)
    {
        placer.y++;

        if (placer.y >= height)
        {
            placer.y--;

            float randomDir = Random.Range(0, 1f);

            if (randomDir >= 0.5)
            {
                movePlacerRight(placer);
            
            }// end if
            
            else
            {
                movePlacerLeft(placer);
            
            }// end else

        }// end if
    
    }// end move placer up

    void movePlacerRight(Placer placer)
    {
        placer.x++;
        if (placer.x >= width)
        {
            placer.x--;

            float randomDir = Random.Range(0, 1f);

            if (randomDir >= 0.5)
            {
                movePlacerUp(placer);
            }// end if

            else
            {
                movePlacerDown(placer);
            
            }// end else

        }// end if
    
    }// end move placer right

    void movePlacerDown(Placer placer)
    {
        placer.y--;
        if (placer.y < 0)
        {
            placer.y++;

            float randomDir = Random.Range(0, 1f);

            if (randomDir >= 0.5)
            {
                movePlacerRight(placer);
            
            }// end if
            
            else
            {
                movePlacerLeft(placer);
            
            }// end else

        }// end if
    
    }// end move placer down

    void movePlacerLeft(Placer placer)
    {
        placer.x--;
        if (placer.x < 0)
        {
            placer.x++;

            float randomDir = Random.Range(0, 1f);

            if (randomDir >= 0.5)
            {
                movePlacerUp(placer);
            
            }// end if

            else
            {
                movePlacerDown(placer);
            
            }// end else

        }// end if
    
    }// end move player left

    public void initializeRooms()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (intModel[x,y] == 1)
                {
                    rooms[x, y] = new Room();
                
                }// end if
            
            }// end for
        
        }// end for

        /*
            
            initialize rooms just goes through each x and y and tests if that spot
            in the int model array is equal to one, meaning there is a "room" there,
            if there is then we make a new room in the rooms aray at the corisponding spot 
            
        */

    }// end initialize rooms

    public void markDoors()
    {
        Room currentRoom;

        /*
            
            this variable is used to store the room that is at the spot x,y
            and we use it to modify the booleans inside that specific room
            
        */

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // these for loops just loop through all spots in width * height

                if (rooms[x, y] != null)
                {
                    // as long as the spot isn't empty we continue

                    currentRoom = rooms[x,y];

                    /*
                        
                        *NOTE* the next four if statments are basicaly the same, only difference is
                        what direction they are checking for
                        
                        first one checks up, if there is a room above it the up boolean is set to true
                        in the room at x,y. 

                        second one does that but right, third one does that but down, fourth one does
                        that but left

                    */

                    if (y < height - 1)
                    {
                        if (rooms[x, y + 1] != null)
                        {
                            currentRoom.up = true;
                        
                        }// end if
                    
                    }// end if

                    if (x < width - 1)
                    {
                        if (rooms[x + 1, y] != null)
                        {
                            currentRoom.right = true;
                        
                        }// end if
                    
                    }// end if

                    if (y > 0)
                    {
                        if (rooms[x, y - 1] != null)
                        {
                            currentRoom.down = true;
                        
                        }// end if
                    
                    }// end if

                    if (x > 0)
                    {
                        if (rooms[x - 1, y] != null)
                        {
                            currentRoom.left = true;
                        
                        }// end if
                    
                    }// end if
                
                }// end if
            
            }// end for
        
        }// end for
    
    }// end mark doors

    public void setStartingIndex()
    {
        startingIndex = new Vector2Int
        {
            x = Random.Range(0, width),
            y = Random.Range(0, height)
        
        };// end starting index


        /*
            
            starting index is a random vector two with x and y values from between 0 - 19,
            so it can range from being a point at (0,0) to a point at (19,19) and anything
            in between
            
        */

        if (rooms[startingIndex.x, startingIndex.y] == null)
        {
            setStartingIndex();

        }// end if

        else
        {
            rooms[startingIndex.x, startingIndex.y].doNotSpawnEnemies = true;
        
        }// end else
        
        /*
            
            if the spot that was chosen in the rooms array is empty we go again,
            we keep reseting until we get a spot that isn't null. Once we've done
            that we leave starting index as it is and set the do not spawn enemies
            boolean of the room at that spot to true, because we don't want the 
            player to have to fight enemies when the first start.
            
        */

    }// end set starting index

    /*
            
        *NOTE* set boss room is exactly like set starting index but if we chose the same
        spot as starting index does we have to pick again, we do this to avoid any
        unpleaset errors that could occur from having the same room set to different 
        things.
        
    */

    public void setBossRoom()
    {
        bossRoomIndex = new Vector2Int
        {
            x = Random.Range(0, width),
            y = Random.Range(0, height)
        
        };// end boss room index

        if (rooms[bossRoomIndex.x, bossRoomIndex.y] == null)
        {
            setBossRoom();
        
        }// end if

        else if (bossRoomIndex == startingIndex)
        {
            setBossRoom();
        
        }// end if

        else
        {
            rooms[bossRoomIndex.x, bossRoomIndex.y].bossRoom = true;
        
        }// end else
    
    }// end set boss room

}// end class
