using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int beat;
    public int Beat 
    { 
        get { return beat; }
        set 
        {
            if (value >= 8) Bar += 1;
            beat = value % 8; 
        }
    }

    private int Bar;

    public PulseToTheBeat testing;

    public bool playerInputAllowed = false;

    public enum inputNames
    {
        LEFT_EYE
    }

    private List<inputNames> generatedList;
    public List<inputNames> inputList;
    public List<GameObject> inputObjects;

    //[LEFT_EYE, MOUTH, RIGHT_EAR]

    private int[,] patterns = 
        {
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 },
            { 0, -1, 1, -1, -1, -1, 2, -1 }
        }; 
    

    void Start()
    {
        Beat = 0;
        Bar = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerInputAllowed = Bar == 3;
        playerInputAllowed = Bar == 7;
        if (Bar > 7) playerInputAllowed = Bar % 2 == 1;
    }

    //_,call,_,response,_,call,_,response....

    // BeatUpdate is called once per beat (quavers, 1/8th notes)
    public void BeatUpdate()
    {
        Beat++;
        Debug.Log(Beat);
        if (Beat%2==0) testing.Pulse();

        generatedList.SequenceEqual(inputList);

        if (Bar % 2 == 0)
        {
            //find out its 3
            int patternNumber = generatedList.Count-1;
            if (patterns[patternNumber, Beat] != -1)
            {
                //do something with generatedList[patterns[patternNumber, Beat]]
                foreach (GameObject g in inputObjects)
                {
                    if (g.GetComponent<TouchToggle>().inputType == generatedList[patterns[patternNumber, Beat]])
                    {
                        //animation
                    }
                }
            }
        }
    }

}
