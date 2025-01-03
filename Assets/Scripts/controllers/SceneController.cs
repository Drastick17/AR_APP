using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControler : MonoBehaviour
{
    [SerializeField] private GameObject currentScene;

    public void ChangeScene(GameObject nextScene)
        {
            if (!currentScene || !nextScene) return;

            currentScene.SetActive(false);
            nextScene.SetActive(true);

    }
}
