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
    private bool isBallOnPendulum = false; // Добавлена переменная для отслеживания наличия шарика

    void Start()
    {
        SpawnRandomPrefab();
        StartCoroutine(SpawnPrefabAfterDelay(3f)); // Запускаем корутину для спавна через 3 секунды
    }

    void Update()
    {
        // Вычисляем текущее угловое значение на основе времени
        currentAngle += direction * swingSpeed * Time.deltaTime;

        // Меняем направление, если угол выходит за пределы
        if (currentAngle >= swingAngle || currentAngle <= -swingAngle)
        {
            direction *= -1f; // Изменяем направление
        }

        // Обновляем вращение объекта вокруг оси Z
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        if (Input.GetMouseButtonDown(0)) // Проверка нажатия на экран
        {
            ToggleRigidbody();
            DetachPrefabFromSpawnPoint(); // Отвязываем префаб от spawnPoint
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
        SpawnRandomPrefab(); // Создаем новый префаб только если на маятнике нет шарика
        StartCoroutine(SpawnPrefabAfterDelay(delay)); // Запускаем снова корутину для создания нового префаба через 3 секунды
    }

    private void ToggleRigidbody()
    {
        if (currentPrefab != null)
        {
            Rigidbody2D rb = currentPrefab.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = !rb.isKinematic; // Переключает значение isKinematic
            }
        }
    }

    private void DetachPrefabFromSpawnPoint()
    {
        if (currentPrefab != null)
        {
            // Устанавливает родителем null, чтобы отвязать
            currentPrefab.transform.SetParent(null);
            // Закрепите позицию префаба, чтобы он не перемещался
            currentPrefab.transform.position = currentPrefab.transform.position;
            currentPrefab.transform.rotation = currentPrefab.transform.rotation; // Закрепляем ориентацию

            Rigidbody2D rb = currentPrefab.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false; // Делает физику активной для свободного падения
            }
            isBallOnPendulum = false; // Обновляем статус при удалении шарика 
        }
    }
}
