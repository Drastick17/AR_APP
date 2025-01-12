using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class ARView3 : MonoBehaviour
{

    private readonly GameObject goCutoutBox;
    private readonly GameObject goCrossSectionPlane;

    private readonly List<GameObject> sliceObject = new();
    private readonly List<GameObject> transformScaleObject = new();


    VolumeRenderedObject volumeRendered;
    GameObject resonanceContainer;
    GameObject goResonance;

    // Límite para los cortes
    private readonly float minSlice = -0.5f;
    private readonly float maxSlice = 0.5f;

    // Para seguimiento de gestos táctiles
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float initialPinchDistance;
    private Vector3 initialScale;
    private bool isScaling = false;

    public ARView3(GameObject goContainerRes)
    {
        volumeRendered = GlobalState.Instance.loadedFile;
        resonanceContainer = GlobalState.Instance.loadFileContainerResonance;

        goResonance = volumeRendered.gameObject;
        goResonance.transform.SetParent(goContainerRes.transform);

        // Crear recortes y asignar referencias
        VolumeObjectFactory.SpawnCutoutBox(volumeRendered);
        VolumeObjectFactory.SpawnCrossSectionPlane(volumeRendered);


        goCutoutBox = goContainerRes.transform.GetChild(1).gameObject;
        goCrossSectionPlane = goContainerRes.transform.GetChild(2).gameObject;
        goCrossSectionPlane.SetActive(false);

        SetTransformScaleAndSlice();
    }

    private void SetTransformScaleAndSlice()
    {
        Vector3[] rotations = { Vector3.zero, new Vector3(90.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 90.0f) };

        
        foreach (Vector3 v in rotations)
        {
            SlicingPlane plane = volumeRendered.CreateSlicingPlane();
            GameObject planeObject = plane.gameObject;

            planeObject.transform.localRotation = Quaternion.Euler(v);
            sliceObject.Add(planeObject);
        }

        foreach (Transform currentChild in goCutoutBox.transform)
        {
            if (currentChild.name.Contains("Scaler") && currentChild.gameObject.activeSelf)
            {
                currentChild.gameObject.SetActive(false);
                transformScaleObject.Add(currentChild.gameObject);
            }
        }
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchEndPos = touch.position;
                Vector2 delta = touchEndPos - touchStartPos;
                RotateContainer(delta);
                touchStartPos = touchEndPos;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (!isScaling)
            {
                initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = resonanceContainer.transform.localScale;
                isScaling = true;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                float scaleFactor = currentPinchDistance / initialPinchDistance;
                resonanceContainer.transform.localScale = initialScale * scaleFactor;
            }
        }
        else if (Input.touchCount == 0)
        {
            isScaling = false;
        }
    }

    private void RotateContainer(Vector2 delta)
    {
        float rotationSpeed = 0.1f;
        resonanceContainer.transform.Rotate(new Vector3(-delta.y, delta.x, 0) * rotationSpeed, Space.World);
    }

    public void EnableCrossSectionPlane()
    {
        bool isActive = !goCrossSectionPlane.activeSelf;
        goCrossSectionPlane.SetActive(isActive);

        if (isActive)
        {
            goCrossSectionPlane.transform.SetLocalPositionAndRotation(Vector3.zero,
                Quaternion.Euler(-90, 0, 0));
            SetActiveScalePoints(false);
            SetActiveSlices(false);
        }
    }

    public void EnableScalePoints(bool value)
    {
        bool isActive = !AreAnyActive(transformScaleObject);
        SetActiveScalePoints(isActive);

        EnableCollider(transformScaleObject, value);

        if (isActive)
        {
            goCrossSectionPlane.SetActive(false);
            SetActiveSlices(false);
        }
    }

    public void EnableSlices(bool value)
    {
        bool isActive = !AreAnyActive(sliceObject);
        SetActiveSlices(isActive);

        EnableCollider(sliceObject, value);

        if (isActive)
        {
            goCrossSectionPlane.SetActive(false);
            SetActiveScalePoints(false);
        }
    }

    private void SetActiveScalePoints(bool isActive)
    {
        foreach (GameObject go in transformScaleObject)
        {
            go.SetActive(isActive);
        }
    }

    private void SetActiveSlices(bool isActive)
    {
        foreach (GameObject go in sliceObject)
        {
            go.SetActive(isActive);
        }
    }

    private bool AreAnyActive(List<GameObject> gameObjects)
    {
        foreach (GameObject go in gameObjects)
        {
            if (go.activeSelf) return true;
        }
        return false;
    }

    private void EnableCollider(List<GameObject> gameObjects, bool value)
    {
        foreach (GameObject current in gameObjects)
        {
            current.GetComponent<BoxCollider>().enabled = value;
        }
    }

    public void EnableColliderContainer(bool isActive)
    {
        resonanceContainer.GetComponent<BoxCollider>().enabled = isActive;
    }

    public void HideAllComponent()
    {
        goCrossSectionPlane.SetActive(false);
        SetActiveScalePoints(false);
        SetActiveSlices(false);
    }
}
