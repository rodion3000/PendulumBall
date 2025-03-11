using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public void DestroyBalls(List<Ball> balls)
    {
        foreach (var ball in balls)
        {
            Destroy(ball.gameObject);
        }
    }
}
