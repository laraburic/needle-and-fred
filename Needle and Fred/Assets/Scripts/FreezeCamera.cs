using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FreezeCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vCamera;
    
    void Start() {
        // Find the camera and turn off the noise
        DeactivateNoise();
    }

    public void DeactivateNoise() {
        if (GameObject.FindObjectOfType<CinemachineVirtualCamera>() != null) {
            vCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>(); // find the camera
            vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f; // turn off shake
        }
    }

    public void ReactivateNoise() {
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.3f;
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.7f;
    }
}
