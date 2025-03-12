using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Привязка к элементу UI TextMeshPro

    private void Start()
    {
        // Получаем общий счет из GameManager и отображаем его
        scoreText.text = $"Total Score: {GameManager.totalScore}";
    }
}
