using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SceneChange(string SceneName)
    {
        if(GameManager.instance.DiePanel != null)
            GameManager.instance.DiePanel.SetActive(false);
        SceneManager.LoadScene(SceneName);
    }

    public void TitleSceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void OutGame(string SceneName)
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(GameManager.instance.FindPlayer.gameObject);
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        GameManager.instance.DiePanel.SetActive(false);
        GameManager.instance.NowHealth = GameManager.instance.MaxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
