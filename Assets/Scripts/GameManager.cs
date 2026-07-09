using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    public int livesCount;
    [SerializeField] private TextMeshProUGUI livesCountText;

    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scoreTextRollingParent;
    [SerializeField] private TextMeshProUGUI scoreTextRolling;

    [SerializeField] private AudioSource lossClip;
    [SerializeField] private GameObject endScreen;
    public GameObject livesCountParent;

    private bool isGameOver = false;

    void Awake()
    {
        endScreen.SetActive(false);
    }

    void Update()
    {
        if (isGameOver == true)
        {
            return;
        }

        
        scoreTextRolling.text = "" + score;

        if (livesCount <= 0)
        {
            Debug.Log("AWW FUCK :(");
            isGameOver = true;
            EndGame();
        }
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    private void EndGame()
    {
        isGameOver = true;
        endScreen.SetActive(true);
        scoreText.text = "" + score;
        livesCountParent.SetActive(false);
        scoreTextRollingParent.SetActive(false);
        levelManager.tutorial = true;
        levelManager.StopGame();
        lossClip.Play();
    }
}
