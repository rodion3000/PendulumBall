using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DropZone[] dropZones; // Массив всех триггер-объектов
    private int filledDropZonesCount = 0; // Количество заполненных триггеров
    public static int totalScore = 0; // Общее количество очков

    private void Start()
    {
        dropZones = FindObjectsOfType<DropZone>();
    }

    public void OnDropZoneFilled()
    {
        filledDropZonesCount++; // Увеличиваем счетчик заполненных зон

        // Проверяем, все ли триггер-объекты пополнились
        if (filledDropZonesCount >= dropZones.Length)
        {
            LoadNextScene(); // Переход на следующую сцену
        }
    }

    public void OnDropZoneUnfilled()
    {
        filledDropZonesCount--; // Уменьшаем счётчик заполненных зон
    }

    private void LoadNextScene()
    {
        // Здесь укажите название следующей сцены
        SceneManager.LoadScene("ScoreScene");
    }

    // Метод для добавления очков
    public void AddScore(int score)
    {
        totalScore += score; // Добавляем очки к общему счету
        Debug.Log($"Current Score: {totalScore}"); // Выводим текущий счет в консоль
    }
}
