using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARView : MonoBehaviour
{

    [SerializeField] private Camera mainCamara;

    ARVolumeRendered volumeManager = new();

    private void setStartObjectPosition()
    {
        try
        {
            Vector3 positionInFront = mainCamara.transform.position + mainCamara.transform.forward * 2.0f;
            volumeManager.volumeRendered.transform.position = positionInFront;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void Awake()
    {
        volumeManager.SetupVolume();
    }

    private void Start()
    {
        setStartObjectPosition();
    }

}
