using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArFaceTracker : MonoBehaviour
{
    [SerializeField] private ARFaceManager arFaceManager;



    private void OnEnable()
    {
        // Subscribe to the event when a new face is added
        arFaceManager.facesChanged += OnFacesChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        arFaceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        // Iterate through added faces
        foreach (ARFace face in args.added)
        {
            Vector3 facePosition = face.transform.position;
            Debug.Log($"AR Face position: {facePosition}");
        }

        // Optional: Handle updated and removed faces as needed
        foreach (ARFace face in args.updated)
        {
            Vector3 updatedFacePosition = face.transform.position;
            Debug.Log($"AR Face updated at position: {updatedFacePosition}");
        }

        foreach (ARFace face in args.removed)
        {
            Debug.Log($"AR Face removed: {face.trackableId}");
        }
    }
}
