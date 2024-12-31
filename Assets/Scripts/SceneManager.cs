using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject currentScene;

    public void ChangeScene(GameObject nextScene)
    {
        nextScene.SetActive(true);
        currentScene.SetActive(false);
    }


}
