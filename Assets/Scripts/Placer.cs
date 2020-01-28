using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer
{

    public int x;
    public int y;

    public int step;
    public int lifespan;

    /*
        
        x and y are used to track the x and y
        positions of the placer, step is used to
        track how many times the placer has moved,
        and life span is used to track how many hits
        the placer can take before it dies.

    */
    public Placer(int x, int y)
    {
        this.x = x;
        this.y = y;

        step = 0;
        lifespan = Random.Range(1,4);
    
    }// end constructor code

    /*
        
        in the constructor code we set the x and y values input into the placer
        when it was made equal to the x and y values of the placer, this allows
        me to track where the placer is, and it allows me to move the placer to
        new x and y values based on the ones it is at.

        everytime a new placer is made the new placer's step value will be set to zero,
        and it's life span will be from 1, 2, or 3.

    */

}// end class