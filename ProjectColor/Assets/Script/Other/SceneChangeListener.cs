using UnityEngine;

public class SceneChangeListener : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.ChangeScene();
    }
}