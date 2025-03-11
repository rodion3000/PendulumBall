using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private float swingAngle = 30f; // Максимальный угол колебания
    [SerializeField] private float swingSpeed = 2f;   // Скорость колебания
    [SerializeField] private GameObject[] prefabs;    // Массив для хранения префабов
    [SerializeField] private Transform spawnPoint;     // Точка спавна

    private GameObject currentPrefab;
    private float currentAngle;
    private float direction = 1f; // Направление колебания
    private bool isBallOnPendulum = false; // Наличие шарика на маятнике

    public delegate void BallReleased(GameObject ball);
    public event BallReleased OnBallReleased; // Событие для оповещения об отзывании струны

    void Start()
    {
        SpawnRandomPrefab();
        StartCoroutine(SpawnPrefabAfterDelay(3f)); // Запускаем корутину для спавна через 3 секунды
    }

    void Update()
    {
        // Управление колебанием
        currentAngle += direction * swingSpeed * Time.deltaTime;

        if (currentAngle >= swingAngle || currentAngle <= -swingAngle)
        {
            direction *= -1f; // Изменяем направление
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        if (Input.GetMouseButtonDown(0)) // Проверка нажатия
        {
            DetachPrefabFromSpawnPoint(); // Отвязываем префаб
        }
    }

    private void SpawnRandomPrefab()
    {
        if (!isBallOnPendulum) 
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            currentPrefab = Instantiate(prefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);
            currentPrefab.transform.SetParent(transform); // Делает префаб дочерним к маятнику
            isBallOnPendulum = true; 
        }
    }

    private IEnumerator SpawnPrefabAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждем указанное время
        SpawnRandomPrefab(); // Создаем новый префаб
        StartCoroutine(SpawnPrefabAfterDelay(delay)); // Запускаем снова корутину
    }

    private void DetachPrefabFromSpawnPoint()
    {
        if (currentPrefab != null)
        {
            // Отвязываем префаб и повторно устанавливаем его положение и ориентацию
            currentPrefab.transform.SetParent(null);
            // Закрепляем позицию и ориентацию
            currentPrefab.transform.position = currentPrefab.transform.position; 
            currentPrefab.transform.rotation = currentPrefab.transform.rotation;

            // Активация физики
            Rigidbody2D rb = currentPrefab.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false; // Делает физику активной для падения
            }
            isBallOnPendulum = false;

            // Вызываем событие об освобождении шара
            OnBallReleased?.Invoke(currentPrefab);
        }
    }
}
