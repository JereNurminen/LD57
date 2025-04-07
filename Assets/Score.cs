using UnityEngine;

public class Score : MonoBehaviour, IDifficultyConfigurable
{
    [SerializeField]
    private DifficultySettings<int> score;

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

    public void SetDifficulty(Difficulty difficulty)
    {
        score.SetDifficulty(difficulty);
    }
}
