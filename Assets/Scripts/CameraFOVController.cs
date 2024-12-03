using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVController : MonoBehaviour
{
    [SerializeField]
    Camera fpsCamera;
    [SerializeField]
    float defaultFOV = 60f;
    [SerializeField]
    float SprintFOV = 80f;
    public void startSprintFOV(float time) 
    {
        StopAllCoroutines();
        StartCoroutine(IncreaseFOV(fpsCamera.fieldOfView, SprintFOV, time));
    }

    public void stopSprintFOV(float time)
    {
        StopAllCoroutines();
        StartCoroutine(DecreaseFOV(fpsCamera.fieldOfView, defaultFOV, time));
    }

    IEnumerator IncreaseFOV(float startFOV, float NewFOV, float time) 
    {
        while (fpsCamera.fieldOfView < NewFOV) 
        {
            float n = NewFOV - startFOV;
            fpsCamera.fieldOfView += Mathf.Lerp(0,n, time);
            yield return null;
        }
    }

    IEnumerator DecreaseFOV(float startFOV, float NewFOV, float time)
    {
        while (fpsCamera.fieldOfView > NewFOV)
        {
            float n = startFOV - NewFOV;
            fpsCamera.fieldOfView -= Mathf.Lerp(0, n, time);
            yield return null;
        }
    }
    public void resetCameraFOV() 
    {
        fpsCamera.fieldOfView = defaultFOV;
    }
}
