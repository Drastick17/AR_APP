using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class ARVolumeRendered : MonoBehaviour
{
    private GameObject planeObject;

    public  VolumeRenderedObject volumeRendered;

    //Vector3[] rotations = { Vector3.zero, new Vector3(90.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 90.0f) };

    Vector3 XZPlane = new(0.0f, 45.0f, 0.0f);  // Rotation in XZ plane
    Vector3 YZPlane = new(30.0f, 0.0f, 0.0f);  // Rotation in YZ plane
    Vector3 XYPlane = new(0.0f, 0.0f, 90.0f);  // Rotation in XY plane

    public void SetupVolume()
    {
        volumeRendered = GlobalState.Instance.loadedFile;
    }

    public void CreateXZPlane()
    {
        CreatePlane(XZPlane);
    }

    public void CreateYZPlane()
    {
        CreatePlane(YZPlane);
    }

    public void CreateXYPlane()
    {
        CreatePlane(XYPlane);
    }

    public void CreateCutBox()
    {
        VolumeObjectFactory.SpawnCutoutBox(volumeRendered);
    }

    public void CreateCrossSectionPlane()
    {
        VolumeObjectFactory.SpawnCrossSectionPlane(volumeRendered);
    }

    private void EnableXZSlicing(){
        CreateXZPlane();
    }

    private void EnableYZSlicing(){
        CreateYZPlane();
    }

    private void EnableXYSlicing(){
        CreateXYPlane();
    }

    private void EnableCutSectionPanel(){
        CreateCutBox();
        CreateCrossSectionPlane();
    }

    private void TransformMeshSlice(){
        Material material = currentMeshRendered();
        PaintSlice(material);
    }

    // Pinta la rawImage que pasa como parametro
    private void PaintSlice(Material material) {
        imgResonance.material = material;
        imgResonance.gameObject.SetActive(isActive);
    }

    private void CreatePlane(Vector3 v)
    {
        SlicingPlane plane = volumeRendered.CreateSlicingPlane();
        planeObject = plane.gameObject;

        planeObject.transform.localRotation = Quaternion.Euler(v);
        
        Debug.Log(planeObject);
    }
    
    private Material currentMeshRendered(){
        if(planeObject.TryGetComponent(out MeshRenderer meshRenderer)) return;
        return meshRenderer.sharedMaterial
    }

}
