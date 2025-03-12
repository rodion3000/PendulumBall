using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DropZone[] dropZones; // Массив всех триггер-объектов
    private int filledDropZonesCount = 0; // Количество заполненных триггеров

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

    private void LoadNextScene()
    {
        // Здесь укажите название следующей сцены
        SceneManager.LoadScene("ScoreScene");
    }
}
