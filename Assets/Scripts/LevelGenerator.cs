using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of " + this + " found");
        
        }// end if

        instance = this;
    
        // by now I think you get why I use instances

    }// end awake

    public GameObject emptyGameObject;
    public GameObject roomGameObject;
    public GameObject nodeGameObject;

    // *NOTE* these game object are not used anymore

    public int mapWidth;
    public int mapHeight;
    public int mapGrowthMax;

    /*
        
        map width and map height are used to 
        store how long the side lengths are,
        so if the width is 10 and the height is
        10, then the map is a grid of 10 x 10

        map growth max is used to limit how many rooms can
        be placed, so the number of rooms on a floor can never
        be more than the value of max growth
    
    */ 

    //public GameObject[] rooms;
    //public Transform mapPapa;

    /* 
        
        *NOTE* these two things are used for the in game map that
        I wasn't able to get working, so they are commented out.

    */

    public MapData mapData;

    /*
        
        I import the class map data to use the code inside of it, 
        by now I think you get that if I want to use code from 
        another class I need to import it in this fasion.

    */ 

    public void generateLevel()
    {

        mapData = new MapData(mapWidth, mapHeight, mapGrowthMax);

        mapData.PlaceRooms();

        mapData.SubtractRooms();

        mapData.initializeRooms();

        mapData.markDoors();

        mapData.setStartingIndex();

        mapData.setBossRoom();
    
    }// end generate level

    /*
        
        Generate level is mainly used as a compiler for all the functions in map data
        that are used to generate the dungeon.

        it makes a new map data instead of using an instance because I need all new randomization,
        and map data uses a lot of randomization. So because I don't need consisant variables I make
        a new copy of the class instead of using an instance. I input map width, map height, and 
        map growth max into the map data, if you refer to map data you can see that it has constructor code
        that requires three integers to be input.

        after we make a new map data we run all the code required to generate a working dungeon, see map
        data for the details of each function.

    */

    /*public void createMap()
    {

        GameObject mapHolder;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = mapData.rooms[x, y];
                if (currentRoom != null)
                {
                    string roomType = "Room ";
                    if (currentRoom.left == true)
                    {
                        roomType += "L";
                    }

                    if (currentRoom.up == true)
                    {
                        roomType += "U";
                    }
                    if (currentRoom.right == true)
                    {
                        roomType += "R";
                    }
                    if (currentRoom.down == true)
                    {
                        roomType += "D";
                    }

                    GameObject roomPrefab = Array.Find(rooms, GameObject => GameObject.name == roomType);
                    mapHolder = Instantiate(roomPrefab, mapPapa);

                    mapHolder.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);

                    //Debug.Log(x + ", " + y);

                }
            }
        }
    }*/
    // *NOTE* this function is not used because it doesn't work
}
