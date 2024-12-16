using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARFacePosition : MonoBehaviour
{
    [SerializeField] private ARFaceManager arFaceManager;

    void Update()
    {
        if (arFaceManager == null && arFaceManager.trackables.count == 0) return;
       
            foreach (var face in arFaceManager.trackables)
            {
                Transform faceTransform = face.transform;

                // Log the position
                Debug.Log($"Face Position: {faceTransform.position}");
            }
        
    }

}
