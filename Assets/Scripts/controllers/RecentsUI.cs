using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Medical;
public class RecentsUI : MonoBehaviour
{

    private void Awake()
    {
        DataPersistence.LoadListRecentFiles();
    }

    private async void LoadAndRenderFile(string fileName)
    {
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
