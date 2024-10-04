using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelInfo : MonoBehaviour
{
    [Header("Element")]
    public TextMeshProUGUI highScoreText;
    public Image[] stars;

    [Header("Config")]
    public Color normalStarColor = Color.white;
    public Color grayStarColor = Color.gray;

    public void Setup(int star, int highScore)
    {
        SetStar(star);
        SetHighScoreText(highScore);
    }

    public void SetStar(int star)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = (i < star) ? normalStarColor : grayStarColor;
        }
    }

    public void SetHighScoreText(int highScore)
    {
        highScoreText.SetText((highScore > 0) ? "High Score: " + highScore : "Locked");
    }
}