using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public void OnReplayButtonClicked()
    {
        DestroyAllBalls(); // Уничтожаем все шары
        GameManager.totalScore = 0; // Обнуляем общий счет
        SceneManager.LoadScene("GameScene"); // Переход на сцену GameScene
    }

    private void DestroyAllBalls()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball"); // Находим все объекты с тегом "Ball"
        foreach (GameObject ball in balls)
        {
            Destroy(ball); // Уничтожаем каждый шар
        }
    }
}
