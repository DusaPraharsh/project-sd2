using Unity.Cinemachine;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public CinemachineCamera followCamera;
    public CinemachineCamera aimCamera;

    public int followPriority = 10;
    public int aimPriority = 20;

    void Start()
    {
        followCamera.Priority = followPriority;
        aimCamera.Priority = followPriority - 1;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimCamera.Priority = aimPriority;
            followCamera.Priority = followPriority - 1;
        }
        else
        {
            followCamera.Priority = followPriority;
            aimCamera.Priority = followPriority - 1;
        }
    }
}
