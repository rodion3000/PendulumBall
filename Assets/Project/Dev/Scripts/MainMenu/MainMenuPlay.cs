using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPlay : MonoBehaviour
{
    public void OnGameScene()
    {
        SceneManager.LoadScene("GameScene"); // Переход на сцену GameScene
    }
}
