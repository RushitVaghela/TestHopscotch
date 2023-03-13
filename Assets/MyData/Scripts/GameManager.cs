using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {

  

    public static int CurrentSelectedRocketType {
        get {
            return PlayerPrefs.GetInt ("CurrentSelectedRocketType", 0);
        }
        set {
            PlayerPrefs.SetInt ("CurrentSelectedRocketType", value);
        }
    }

    public static void SaveSettings () {

    }

    public static void LoadSettings () {

    }

}
public enum RocketType {
    Small = 0,
    Long,
    Big,
}

[System.Serializable]
public struct RocketDataSettings {
    public List<RocketData> Rockets;
}

[System.Serializable]
public struct RocketData {
    public string RocketName;
    public RocketType type;
    [Range (5f, 20f)]
    public float Speed;
    [Range (0f, 5f)]
    public float LaunchDelay;
    [Range (0f, 20f)]
    public float ExplosionRadius;
    [Range (0f, 5000f)]
    public float ExplosionForce;
    [Range (10f, 30f)]
    public float DefaultResetTime;
    [Range (0f, 1f)]
    public float CameraShakeDuration;
    [Range (0f, .5f)]
    public float CameraShakeFrequncy;

}