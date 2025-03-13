using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallBounce : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения
    public float jumpForce = 5f; // Сила прыжка
    public float changeDirectionInterval = 5f; // Интервал смены направления
    public float gravityScale = 1f; // Масштаб гравитации

    private Rigidbody2D rb;
    private Vector2 currentDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());
        currentDirection = Vector2.right; // Начинаем движение вправо
    }

    private void Update()
    {
        // Перемещаем мячик в текущем направлении
        rb.velocity = new Vector2(currentDirection.x * moveSpeed, rb.velocity.y); // Поддерживаем вертикальную скорость мячика

        // Применяем искусственную гравитацию
        rb.AddForce(Vector2.down * gravityScale, ForceMode2D.Force);
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Ждем установленный интервал
            yield return new WaitForSeconds(changeDirectionInterval);

            // Генерация случайного направления (влево или вправо)
            currentDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bounce"))
        {
            // Получаем нормаль столкновения
            Vector2 normal = collision.contacts[0].normal;

            // Рассчитываем новое направление отталкивания
            currentDirection = Vector2.Reflect(currentDirection, normal).normalized;

            // Если направление изменилось (например, мячик отскочил от стены), добавляем прыжок
            rb.velocity = new Vector2(currentDirection.x * moveSpeed, jumpForce);
        }
    }
}
