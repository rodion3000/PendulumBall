using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public List<Ball> ballsInZone = new List<Ball>();
    private Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInZone.Add(ball);
            
            // Увеличиваем счетчик для данного цвета
            if (!colorCounts.ContainsKey(ball.ballColor))
            {
                colorCounts[ball.ballColor] = 0;
            }
            colorCounts[ball.ballColor]++;
            
            // Проверяем, если у нас есть 3 шара одного цвета
            if (colorCounts[ball.ballColor] >= 3)
            {
                StartCoroutine(CheckAndDestroyAfterDelay());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInZone.Remove(ball);
            colorCounts[ball.ballColor]--;

            // Если счетчик стал равен нулю, удаляем цвет из словаря
            if (colorCounts[ball.ballColor] <= 0)
            {
                colorCounts.Remove(ball.ballColor);
            }
        }
    }

    private IEnumerator CheckAndDestroyAfterDelay()
    {
        // Ждем 2 секунды
        yield return new WaitForSeconds(2f);
        
        // Проверяем, есть ли три шара одного цвета в зоне
        foreach (var color in colorCounts.Keys)
        {
            if (colorCounts[color] >= 3) // Если есть 3 и более одинаковых
            {
                // Уничтожаем все шары в зоне
                foreach (Ball ball in new List<Ball>(ballsInZone)) // создаем копию списка для безопасного удаления
                {
                    Destroy(ball.gameObject);
                }
                ballsInZone.Clear(); // Очищаем список шаров в зоне
                colorCounts.Clear(); // Очищаем счетчики цветов
                yield break; // Выход из корутины, если нашли и удалили шары
            }
        }
    }
}
    

