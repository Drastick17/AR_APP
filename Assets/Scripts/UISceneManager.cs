using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void ActivateScene(string gameObjectName)
    {
        List<GameObject> activeGameObjects = GetActiveGameObjects();

        Debug.Log("Active GameObjects in the scene:");
        foreach (GameObject go in activeGameObjects)
        {
            Debug.Log(go.name);
        }

        foreach (GameObject go in activeGameObjects)
        {
            if (go.name.Contains("Scene") && go.name != gameObjectName)
            {
                Debug.Log($"Deactivating: {go.name}");
                go.SetActive(false);
            }
        }

        GameObject targetGameObject = GameObject.Find(gameObjectName);
        if (targetGameObject != null)
        {
            Debug.Log($"Activating: {targetGameObject.name}");
            targetGameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"GameObject '{gameObjectName}' not found in the scene.");
        }
    }

    void ActivateAllRootGameObjects()
    {
        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (!go.activeInHierarchy)
            {
                Debug.Log($"Activating: {go.name}");
                go.SetActive(true);
            }
        }
    }

    List<GameObject> GetActiveGameObjects()
    {
        List<GameObject> activeObjects = new List<GameObject>();

        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (go.activeInHierarchy)
            {
                activeObjects.Add(go);
            }
        }

        return activeObjects;
    }

    void Start()
    {
        ActivateAllRootGameObjects();
        ActivateScene("RecentsScene");
    }
}
