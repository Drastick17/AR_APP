using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public event Action OnStart;
    public event Action OnRecentFiles;
    public event Action OnLoadingResource;
    public event Action OnFaceTracker;
    public event Action OnFaceTrackerConfig;

    public GlobalState globalState;

    public static GameManager instance;

    void Awake()
    {

        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HomeView();
    }

    public void HomeView()
    {
        OnStart?.Invoke();
    }

    public void RecentsView()
    {
        OnRecentFiles?.Invoke();
    }

    public void FileExplorerView()
    {
        OnLoadingResource?.Invoke();
    }

    public void ARView()
    {
        OnFaceTracker?.Invoke();
    }

    public void ARConfigMenu()
    {
        OnFaceTrackerConfig?.Invoke();
    }

    public void CloseAPP()
    {
        Application.Quit();
    }

}
