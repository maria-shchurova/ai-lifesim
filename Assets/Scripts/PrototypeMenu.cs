using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeMenu : MonoBehaviour
{
    public Button CommandsScene;
    public Button KatScene;
    public Button StoreScene;

    void Start()
    {
        CommandsScene.onClick.AddListener(() => PlayCommandsScene());
        KatScene.onClick.AddListener(() => PlayKatScene());
        StoreScene.onClick.AddListener(() => PlayStoreScene());
    }

    private void PlayCommandsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    private void PlayKatScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    private void PlayStoreScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
