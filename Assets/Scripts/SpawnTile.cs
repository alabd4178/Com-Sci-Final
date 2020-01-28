using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {
	
	public GameObject tile;
	
	void Start () {

		GameObject instance = (GameObject)Instantiate(tile, transform.position, Quaternion.identity);
		instance.transform.SetParent(transform);

    }// end void

}// end class

/*

This class is pretty basic, it is attached to spawn points that make up the shape of the rooms. 
When the rooms are placed onto the scene the code here runs and spawns the attached tile at the 
corrisponding spawn point. once the object is instantiated we set it's parent to be the room that
was spawned so all the tiles are neatly tucked away.

*/
