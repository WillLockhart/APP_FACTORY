using NUnit.Framework; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    private List<inputNames> generatedList;
    internal List<inputNames> playerInputList;

    private int beat;
    private int Bar;
    private int currentPatternSize = 0;

    public bool playable = false;
    public bool playerInputAllowed = false;
    public List<GameObject> inputObjects;
    [SerializeField] private GameManager gameManager;

    public int Beat 
    { 
        get { return beat; }
        set 
        {
            if (value >= 8) Bar += 1;
            beat = value % 8; 
        }
    }

    public enum inputNames
    {
        LEFT_EYE,
        RIGHT_EYE,
        LEFT_CHEEK,
        RIGHT_CHEEK,
        LEFT_EAR,
        RIGHT_EAR,
        NOSE,
        HAT

    }

    //list of possible patterns. -1 represents a Beat that nothing needs doing on, 0-7 represent the one of the list that needs doing; the position of each number, 0-7, is the Beat the number is for
    private int[,] patterns = 
        {
            { 0, -1, -1, -1, -1, -1, -1, -1 },
            { 0, -1, -1, -1, 1, -1, -1, -1 },
            { 0, -1, 1, -1, 2, -1, -1, -1 },
            { 0, -1, 1, -1, 2, -1, 3, -1 },
            { 0, 1, 2, -1, 3, 4, -1, -1 },
            { 0, 1, 2, -1, 3, 4, 5, -1 },
            { 0, 1, 2, 3, 4, 5, 6, -1 },
            { 0, 1, 2, 3, 4, 5, 6, 7 }
        };

    void Awake()
    {
        playerInputList = new List<inputNames>();
        generatedList = new List<inputNames>();
        Beat = 0;
        Bar = 0;
    }

    void Update()
    {
        if (playable)
        {
            playerInputAllowed = true;
            //playerInputAllowed = Bar % 2 == 1;
        }
        else
        {
            playerInputAllowed = false;
        }
    }

    // This function can be called by a button in the intro UI
    public void StartGame()
    {
        playable = true;
    }

    // Can be called by another script or a UI button
    public void StopGame () 
    { 
        playable = false; 
    }

    public void AddInput(inputNames thisInputName)
    {
        playerInputList.Add(thisInputName);
        if (playerInputList.Count == generatedList.Count)
        {
            if (generatedList.SequenceEqual(playerInputList))
            {
                int score = 1;
                gameManager.AddPoints(score);
                Debug.Log("FUCK YEAH!");
            }
        }
        else
        {
            gameManager.livesCount--;
            //StopGame();
        }
    }

    public void BeatUpdate()
    {
        //Dont remove this it makes the whole thing work
        Beat++;
        //return;

        if (Bar % 2 == 0 && playable)
        {
            if (Beat == 0)
            {
                //generating new list in a Simon Says BAR
                //currentPatternSize = Random.Range(1, 9); // choosing a size at random
                currentPatternSize = Random.Range(1, 5);
                ReloadList(currentPatternSize); //generation
                currentPatternSize--; //decrementing by 1, to use for indexing into patterns array
            }
            if (patterns[currentPatternSize, Beat] != -1) //don't do the following if there wouldnt be anything to do
            {
                //do something with generatedList[patterns[s, Beat]]
                foreach (GameObject g in inputObjects) //check each possible interactable gameobject
                {
                    //for the gameobject we are on, we check if it's name (from the enum) is the same as the name of the one we need to animate
                    if (g.GetComponent<InputObject>().inputType == generatedList[patterns[currentPatternSize, Beat]]) 
                    {
                        //animation
                        //Debug.Log(g.GetComponent<InputObject>().inputType);
                        g.GetComponent<InputObject>().Animate();
                        g.GetComponent<InputObject>().PlaySound();
                        break;
                        
                    }
                }
            }
        } else
        {
            if (Beat == 0) playerInputList.Clear();
        }
    }

    void ReloadList(int s)
    {
        generatedList.Clear();

        for (int i = 0; i < s; i++)
        {
            //generatedList.Add((inputNames)Random.Range(0, 8));
            generatedList.Add((inputNames)Random.Range(0, 4));
        }
    }

}
