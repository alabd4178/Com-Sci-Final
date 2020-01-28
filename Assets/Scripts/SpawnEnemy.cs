using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public static SpawnEnemy instance;

    int enemyCount;

    /*
        
        the two prefabs are used to spawn the enemies or the boss,
        so I set them to the corisponding prefabs in the inspector.

        

    */

    public void Start()
    {
        RoomManager.instance.currentRoom.spawnEnemy = this;
    
    }// end start

    /*
        
       this line looks kinda confusing but it's really simple,
       what it is doing is going to the room manager instance
       opening the current room that the instance is working
       with, and then sets the spawn enemy equal to this
       game object.

    */

    public void spawnBaddies(List<GameObject> enemies)
    {
        enemyCount = Random.Range(1, 6); // 1 - 5

        for (int i = 0; i < enemyCount; i++)
        {
            float randomX = Random.Range(-6, 7); // from -6 to 6
            float randomY = Random.Range(-2, 3); // from -2 to 2

            transform.localPosition = new Vector2(randomX, randomY);

            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            enemies.Add(enemy);
        
        }// end for

        /*
        
            spawn baddies is pretty simple, it require that a list be input when you
            call it, and if you look at room manager where we call it you can see we 
            input the enemy list that we use to check to see if the room is empty.

            because we have a list we can continue, the first thing the function does is
            choose a random number between 1 and 5 then it runs a for loop that will run
            that many time so if enemy count is set to 2 the for loop will run twice.

            in the for loop we choose random x and y values from between (-6,-2) and (6,2)
            so the enemies can be anywhere in between those points.

            after that we move the spawner to that random x and y spot, and spawn the enemies
            at that spot, once we spawned the enemy we add it to the enemies list.

        */

    }// spawn baddies

    public void spawnBoss(List<GameObject> enemies)
    {
        GameObject boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        enemies.Add(boss);
    
    }// end spawn boss

    /*
        
        spawn boss is simpler than spawn enemies, it also needs a list to be input but
        once it has that it will just spawn the boss at the enemy spawner (wich is in the
        middle of the room) and then add it to the enemy array.

        simple and easy.

    */

}// end class
