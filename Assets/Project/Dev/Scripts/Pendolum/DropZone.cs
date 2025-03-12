using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public List<Ball> ballsInZone = new List<Ball>();
    public ParticleSystem disappearEffectPrefab;

    private Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();
    private bool isFilled = false; // Новый флаг для отслеживания заполненности зоны
    private int totalBallsInZone = 0; // Общее количество шаров в зоне

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInZone.Add(ball);
            totalBallsInZone++; // Увеличиваем общее количество шаров в зоне

            if (!colorCounts.ContainsKey(ball.ballColor))
            {
                colorCounts[ball.ballColor] = 0;
            }
            colorCounts[ball.ballColor]++;

            // Если добавленный шар сделал цвет с количеством 3 или больше, активируем корутину
            if (colorCounts[ball.ballColor] >= 3)
            {
                StartCoroutine(CheckAndDestroyAfterDelay());
            }

            // Уведомляем GameManager, если вся зона заполнилась
            if (totalBallsInZone >= 3 && !isFilled)
            {
                isFilled = true; // Отмечаем, что зона заполнена
                FindObjectOfType<GameManager>().OnDropZoneFilled(); // Уведомляем GameManager
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInZone.Remove(ball);
            totalBallsInZone--; // Уменьшаем общее количество шаров в зоне

            colorCounts[ball.ballColor]--;
            if (colorCounts[ball.ballColor] <= 0)
            {
                colorCounts.Remove(ball.ballColor);
            }
        }
    }

    private IEnumerator CheckAndDestroyAfterDelay()
{
    // Ждем 0.5 секунды
    yield return new WaitForSeconds(0.5f);

    // Проверяем, есть ли три шара одного цвета в зоне
    foreach (var color in colorCounts.Keys)
    {
        if (colorCounts[color] >= 3) // Если есть 3 и более одинаковых
        {
            int score = 0; // Переменная для хранения очков

            // Определяем количество очков в зависимости от цвета шара
            // Здесь используем if-else для определения количества очков
            if (color == Color.blue) // Для цвета Blue
            {
                score = 50;
            }
            else if (color == Color.green) // Для цвета Green
            {
                score = 70;
            }
            else if (color == Color.red) // Для цвета Red
            {
                score = 100;
            }


            // Лог, перед начислением очков
            Debug.Log($"Attempting to add score for {color}: {score} points.");

            // Начисляем очки в GameManager
            if (score > 0)
            {
                FindObjectOfType<GameManager>().AddScore(score);
                // Лог для подтверждения добавления очков
                Debug.Log($"Added {score} points for {color} balls.");
            }

            foreach (Ball ball in new List<Ball>(ballsInZone)) // создаем копию списка для безопасного удаления
            {
                // Создаем эффект исчезновения
                ParticleSystem effect = Instantiate(disappearEffectPrefab, ball.transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration); // Удаляем эффект после его завершения

                // Удаляем шар
                Destroy(ball.gameObject);
            }

            ballsInZone.Clear(); // Очищаем список шаров в зоне
            colorCounts.Clear(); // Очищаем счетчики цветов
            totalBallsInZone = 0; // Обнуляем общее количество шаров
            yield break; // Выход из корутины, если нашли и удалили шары
        }
    }
}
}
    

