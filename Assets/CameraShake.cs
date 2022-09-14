using System;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    // public delegate void ScreenShake(float duration, float amount);
    // public event ScreenShake e_ScreenShake;
    public static CameraShake i { get; private set; }

    CinemachineVirtualCamera cinemachineCamera;
    CinemachineBasicMultiChannelPerlin noise;

    float shakeDuration, shakeDurationTotal, startingIntensity;

    void Start() {
        if(!i)
            i = this;

        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float duration) {
        noise.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeDuration = duration;
        shakeDurationTotal = duration;
    }

    void Update() {
        if(shakeDuration <= 0) { return; }
        shakeDuration -= Time.deltaTime;
        noise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1f - (shakeDuration / shakeDurationTotal));
    }

}
