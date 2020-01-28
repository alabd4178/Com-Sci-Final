using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    
    public Image[] images;

    /*

        images is an array of images,
        it holds the three hearts

    */

    public static GUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of " + this + " found");

        }// end if

        instance = this;

    }// end void awake

    /*
        
        making an isntance is basicaly making a copy of this exact file, we
        constantly re-update the instance so the code that access this instance
        will be getting the current version of this script.

        this allows me to update the values of variables inside this code, and 
        everything else will be able to recognize these changes.

        for example player uses and instance of gui to make the heart monitor function. 

     */

    public void updateGUI(int health)
    {

        for (int i = 0; i < images.Length; i++)
        {

            if (i < health)
            {
                images[i].color = Color.white;

            }// end if

            else
            {
                images[i].color = Color.clear;

            }// end else

        }// end for

        /*
            
            update GUI runs a for loop that runs the length of the images array,

            the if statment inside is checking if health is greater than the current
            run through of the for loop, meaning that if health is i is greater than health
            it will go to the else statment.

            if i is less the health we set the heart at spot i in the images array to visible
            otherwise we go to the else statment and set it to transparent
            
        */

    }// end updateGUI

}// end class
