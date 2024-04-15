using System.Collections;
using System.Collections.Generic;
using Ignix.EventBusSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void OnEnable()
    {
        EventBus.Register<OnReturnToMenuScene>(GoToMainScene);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnReturnToMenuScene>(GoToMainScene);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene(0);
    }

    void GoToMainScene(OnReturnToMenuScene args)
    {
        GoToMainScene();
    }
}
