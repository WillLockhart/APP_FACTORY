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

    public bool playerInputAllowed = false;

    public enum inputNames
    {
        LEFT_EYE,
        RIGHT_EYE,
        MOUTH
    }

    private List<inputNames> generatedList;
    internal List<inputNames> playerInputList;


    public List<GameObject> inputObjects;

    //[LEFT_EYE, MOUTH, RIGHT_EAR]

    //list of possible patterns. -1 represents a Beat that nothing needs doing on, 0-7 represent the one of the list that needs doing; the position of each number, 0-7, is the Beat the number is for
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
        playerInputList = new List<inputNames>();
        Beat = 0;
        Bar = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //playerInputAllowed = Bar == 3;
        //playerInputAllowed = Bar == 7;
        //if (Bar > 7) 
        playerInputAllowed = Bar % 2 == 1;
    }

    //_,call,_,response,_,call,_,response....

    // BeatUpdate is called once per beat (quavers, 1/8th notes)
    public void BeatUpdate()
    {
        //Dont remove this it makes the whole thing work
        Beat++;
        //Debug.Log(Beat);

        //Modulo
        //int modulo = 72 % 5; // returns 2
        // % is the modulo operator: It gives the remainder after division.

        //checking if 2 lists are equivalent (returns a bool)
        //generatedList.SequenceEqual(playerInputList);

        // Casting an int to the enum type
        //inputNames n = (inputNames)0;
        //inputNames r = (inputNames)Random.Range(0, 7);

        //generates list
        //generatedList.Clear() empties the list

     
        //rough outline for making animations happen in a simon says Bar
        if (Bar % 2 == 0)
        {
            //generating new list in a Simon Says BAR
            int s = Random.Range(1, 8); // choosing a size at random
            reloadList(s); //generation
            s--; //decrementing by 1, to use for indexing into patterns array
            return;
            if (patterns[s, Beat] != -1) //don't do the following if there wouldnt be anything to do
            {
                //do something with generatedList[patterns[s, Beat]]
                foreach (GameObject g in inputObjects) //check each possible interactable gameobject
                {
                    if (g.GetComponent<InputObject>().inputType == generatedList[patterns[s, Beat]]) //for thw gameobject we are on, we check if it's name (from the enum) is the same as the name of the one we need to animate
                    {
                        //animation
                        g.GetComponent<InputObject>().Animate();
                        g.GetComponent<InputObject>().PlaySound();
                        break;
                        
                    }
                }
            }
        }
    }

    void reloadList(int size)
    {
        generatedList.Clear();

        for (int i = 0; i < size; i++)
        {
            generatedList.Add((inputNames)Random.Range(0, 7));
        }
    }

}
