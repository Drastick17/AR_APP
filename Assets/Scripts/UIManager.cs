using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject HomeCanvas;
    [SerializeField] private GameObject RecentsCanvas;
    [SerializeField] private GameObject FileExplorerCanvas;
    [SerializeField] private GameObject ARFaceTrackerView;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnStart += ActivateHome;
        GameManager.instance.OnRecentFiles += ActivateRecentFiles;
        GameManager.instance.OnLoadingResource += ActivateFileExplorer;
        GameManager.instance.OnFaceTracker += ActivateARFaceTracker;
    }

    private void ActivateHome()
    {
        HomeCanvas.SetActive(true);
        RecentsCanvas.SetActive(false);
        FileExplorerCanvas.SetActive(false);
        ARFaceTrackerView.SetActive(false);

        HomeCanvas.transform.localScale = new Vector3(1, 1, 1);
        RecentsCanvas.transform.localScale = new Vector3(0, 0, 0);
        FileExplorerCanvas.transform.localScale = new Vector3(0, 0, 0);
        ARFaceTrackerView.transform.localScale = new Vector3(0, 0, 0);
    }

    private void ActivateRecentFiles()
    {
        HomeCanvas.SetActive(false);
        RecentsCanvas.SetActive(true);
        FileExplorerCanvas.SetActive(false);
        ARFaceTrackerView.SetActive(false);

        HomeCanvas.transform.localScale = new Vector3(0, 0, 0);
        RecentsCanvas.transform.localScale = new Vector3(1, 1, 1);
        FileExplorerCanvas.transform.localScale = new Vector3(0, 0, 0);
        ARFaceTrackerView.transform.localScale = new Vector3(0, 0, 0);
    }

    private void ActivateFileExplorer()
    {
        HomeCanvas.SetActive(false);
        RecentsCanvas.SetActive(false);
        FileExplorerCanvas.SetActive(true);
        ARFaceTrackerView.SetActive(false);

        HomeCanvas.transform.localScale = new Vector3(0, 0, 0);
        RecentsCanvas.transform.localScale = new Vector3(0, 0, 0);
        FileExplorerCanvas.transform.transform.localScale = new Vector3(1, 1, 1);
        ARFaceTrackerView.transform.localScale = new Vector3(0, 0, 0);
    }

    private void ActivateARFaceTracker()
    {
        HomeCanvas.SetActive(false);
        RecentsCanvas.SetActive(false);
        FileExplorerCanvas.SetActive(false);
        ARFaceTrackerView.SetActive(true);

        HomeCanvas.transform.localScale = new Vector3(0, 0, 0);
        RecentsCanvas.transform.localScale = new Vector3(0, 0, 0);
        FileExplorerCanvas.transform.transform.localScale = new Vector3(0, 0, 0);
        ARFaceTrackerView.transform.localScale = new Vector3(1, 1, 1);
    }

}

