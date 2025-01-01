using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Medical;
using System.IO;
using TMPro;

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
        Debug.Log(fileName);
        RecentFile data = DataPersistence.LoadRecentFile<RecentFile>(fileName);
        if (data.fileType == "dir")
        {
           await MedicalFiles.LoadDirectory(data.path);
        }

        if (data.fileType == "file")
        {
            await MedicalFiles.LoadFile(data.path);
        }

    }
   
}
