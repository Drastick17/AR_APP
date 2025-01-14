using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARView : MonoBehaviour
{
   
    [SerializeField] private Camera mainCamara;
    // Componente de imagen para la resonancia
    [SerializeField] private RawImage imgResonance;
    [SerializeField] private GameObject progressBarXY;
    [SerializeField] private GameObject progressBarZX;
    [SerializeField] private GameObject progressBarZY;


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

    private void FollowHead()
    {
    if (arFaceManager == null && arFaceManager.trackables.count == 0) return;
        
            foreach (var face in arFaceManager.trackables)
            {
                Transform faceTransform = face.transform;

                // Log the position
                Debug.Log($"Face Position: {faceTransform.position}");

                //TODO ADAPT THIS TO CORRECT POSITIONING OF RENDERED VOLUME
            }
    }

    public void EnableXYSlixing(){
        //GET VALUE FROM progressBar
        //WHEN CHANGE GET THE CURRENT POSITION
    }


    private void Awake()
    {
        volumeManager.SetupVolume();
    }

    private void Start()
    {
        setStartObjectPosition();
    }

    private void Update()
    {
        FollowHead()
    }

}
