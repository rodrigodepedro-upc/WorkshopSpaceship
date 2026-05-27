using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    TMP_Text text;

    int score = 0;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        UpdateText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateText();
    }

    void UpdateText()
    {
        // Always 4 digits, zero-padded (SCORE: 0000, SCORE: 0042, SCORE: 1234).
        text.text = "SCORE: " + score.ToString("D4");
    }
}
