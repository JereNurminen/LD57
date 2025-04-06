using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private int score = 5;

    public void ScorePoints()
    {
        // Assuming you have a reference to the GameManager or a similar class
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddToScore(score);
        }
        else
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }
}
