using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FreezeCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vCamera;
    private float originalNoiseValue = 0.3f;
    
    void Start() {
        // Find the camera and turn off the noise
        if (GameObject.FindObjectOfType<CinemachineVirtualCamera>() != null) {
            vCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>(); // find the camera
            originalNoiseValue = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain; // store the original noise value
            vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f; // turn off shake
        }
    }

    public void ReactivateNoise() {
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = originalNoiseValue;
    }
}
