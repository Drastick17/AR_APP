using UnityEngine;

public class GlobalState : MonoBehaviour
{

    public string path;
    public string uploadType;


    // This part helps ensure there's only one instance of this class.
    private static GlobalState instance;

    public static GlobalState Instance

    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }


    public void SetUploadType(string fileType)
    {
        uploadType = fileType;
    }

    public void SaveRecentRender(string name)
    {
        RecentFile recentFile = new RecentFile();

        recentFile.name = name;
        recentFile.path = path;
        recentFile.fileType = uploadType;
        recentFile.created_at = System.DateTime.UtcNow;

        DataPersistence.saveRecentPath(recentFile, name);
    }
}