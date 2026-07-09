using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<inputNames> generatedList;
    internal List<inputNames> playerInputList;

    private int beat;
    private int Bar;
    private int currentPatternSize = 0;
    private int currentPatternIndex = 0;

    public bool tutorial = false;

    public bool playable = false;
    public bool playerInputAllowed = false;

    private bool increase = true;

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
        Beat = 1;
        Bar = 0;
    }

    void Update()
    {
        if (playable)
        {
            //playerInputAllowed = true;
            playerInputAllowed = Bar % 2 == 1;
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
        checkState(false);
    }

    private void checkState(bool final)
    {
        if (!final && generatedList.Count != 0)
        {
            int index = playerInputList.Count - 1;
            if (playerInputList[index] != generatedList[index])
            {
                gameManager.livesCount--;
                StopGame();
                return;
            }
        }
        if (playerInputList.Count == generatedList.Count)
        {
            if (generatedList.SequenceEqual(playerInputList))
            {
                int score = 1;
                gameManager.AddPoints(score);
                Debug.Log("FUCK YEAH!");
                StopGame();
            }
            else
            {
                gameManager.livesCount--;
                StopGame();
            }
        }
        else if (final)
        {
            gameManager.livesCount--;
        }
    }

    public void BeatUpdate()
    {
        if (Bar % 2 == 0 && !tutorial)
        {
            if (Beat == 0)
            {
                if (playable) checkState(true);
                StartGame();
                //generating new list in a Simon Says BAR
                playerInputList.Clear();
                currentPatternSize = currentPatternSize < 8 && increase ? currentPatternSize + 1 : currentPatternSize; 
                //currentPatternSize = Random.Range(1, 5);
                ReloadList(currentPatternSize); //generation
                currentPatternIndex = currentPatternSize - 1; //storing correct index number, to use for indexing into patterns array
                increase = !increase;
            }
            if (patterns[currentPatternIndex, Beat] != -1) //don't do the following if there wouldnt be anything to do
            {
                //do something with generatedList[patterns[s, Beat]]
                foreach (GameObject g in inputObjects) //check each possible interactable gameobject
                {
                    //for the gameobject we are on, we check if it's name (from the enum) is the same as the name of the one we need to animate
                    if (g.GetComponent<InputObject>().inputType == generatedList[patterns[currentPatternIndex, Beat]]) 
                    {
                        //animation
                        //Debug.Log(g.GetComponent<InputObject>().inputType);
                        g.GetComponent<InputObject>().Animate();
                        g.GetComponent<InputObject>().PlaySound();
                        break;
                        
                    }
                }
            }
        }
        //Dont remove this it makes the whole thing work
        Beat++;
        //return;
    }

    void ReloadList(int s)
    {
        generatedList.Clear();

        for (int i = 0; i < s; i++)
        {
            //generatedList.Add((inputNames)Random.Range(0, 8));
            generatedList.Add((inputNames)Random.Range(4, 5));
        }
    }

}
