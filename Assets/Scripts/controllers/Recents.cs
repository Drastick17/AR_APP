using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Medical;
using System.IO;
using TMPro;
using UnityVolumeRendering;

public class RecentsUI : MonoBehaviour
{
    [SerializeField] private Transform pnlContent;
    [SerializeField] private GameObject prefabRecentButton;



    private void Awake()
    {
        string[] fileList = DataPersistence.LoadListRecentFiles();

        foreach (string file in fileList)
        {

            FileInfo fileInfo = new(file);

            GameObject newBtn = Instantiate(prefabRecentButton, pnlContent);
            TMP_Text newTxtBtn = newBtn.transform.GetChild(0).GetComponent<TMP_Text>();

            newTxtBtn.text = fileInfo.Name;
            Button newButton = newBtn.GetComponent<Button>();
            newButton.onClick.AddListener(() => LoadAndRenderFile(fileInfo.Name));

        }
    }

    private async void LoadAndRenderFile(string fileName)
    {
        RecentFile data = DataPersistence.LoadRecentFile<RecentFile>(fileName);

        VolumeRenderedObject obj = data.fileType == "dir"
            ? await MedicalFiles.LoadDirectory(data.path)
            : await MedicalFiles.LoadFile(data.path);

        Debug.Log("loaded" + obj.name);
        GlobalState.Instance.SetLoadedObject(obj);
        ActiveARScene();
    }


    public void ActiveARScene() {
        GameManager.instance.ARView();
    }

    public void ActiveFileExplorerScene(string fileType)
    {
        GlobalState.Instance.SetUploadType(fileType);
        GameManager.instance.FileExplorerView();
    }
   
}
