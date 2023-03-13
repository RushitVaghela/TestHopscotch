using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    [Space]
    [Header ("Main Settings")]
    [SerializeField] RocketDataSettings settings;
    public RocketData currentRocketsettings;
    [SerializeField] List<Projectile> AllRocketPrefabs;

    [Space]
    [SerializeField] Transform PropRocketPosition;
    [SerializeField] ButtonGroup buttonGroup;
    [Space]
    [Header ("Change Rocket UI")]
    [SerializeField] LeanMovePopup ChangeRocketPopup;

    [Space]
    [Header ("Settings UI")]
    [SerializeField] LeanMovePopup SettingsPopup;
    [SerializeField] TMP_Text RocketName_SettingPopup;

    [SerializeField] Slider Slider_LaunchDuration;
    [SerializeField] TMP_Text Value_LaunchDuration;

    [SerializeField] Slider Slider_Speed;
    [SerializeField] TMP_Text Value_Speed;

    [SerializeField] Slider Slider_ExplosionRadius;
    [SerializeField] TMP_Text Value_ExplosionRadius;

    [SerializeField] Slider Slider_ExplosionForce;
    [SerializeField] TMP_Text Value_ExplosionForce;

    [SerializeField] Slider Slider_CameraShakeDuration;
    [SerializeField] TMP_Text Value_CameraShakeDuration;

    [SerializeField] Slider Slider_CameraShakeFrequncy;
    [SerializeField] TMP_Text Value_CameraShakeFrequncy;
    [HideInInspector] public CameraShake cameraShake;

    private void Awake () {
        if (instance == null) instance = this;
    }
    void Start () {
        SetRocket (GameManager.CurrentSelectedRocketType);
        cameraShake = Camera.main.gameObject.GetComponent<CameraShake> ();
    }

    public void OpenRocketChangePopup () {
        // selectRocketToEdit
        ChangeRocketPopup.Open ();

    }
    public void OpenSettingsPopup () {

        SettingsPopup.Open ();

    }

    public void SetRocket (int typeIndex) {
        RocketType type = (RocketType) typeIndex;
        GameManager.CurrentSelectedRocketType = typeIndex;
        selectRocketToEdit (type);
        GameObject NewRocket = AllRocketPrefabs.Find (rkt => rkt.rocketType == type).gameObject;
        RocketLauncher.instance.ChangeRocket (NewRocket);
        SetPropRocket (NewRocket);
    }

    void SetPropRocket (GameObject prefab) {
        int x = PropRocketPosition.childCount;
        for (int i = 0; i < x; i++) {
            Destroy (PropRocketPosition.GetChild (0).gameObject);
        }

        GameObject g = Instantiate (prefab, PropRocketPosition);
        g.transform.localPosition = Vector3.zero;
        g.transform.GetChild (0).gameObject.SetActive (true);
        // g.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
        g.transform.GetChild (1).gameObject.SetActive (false);
        Destroy (g.GetComponent<Rigidbody> ());
        Destroy (g.GetComponent<Projectile> ());
        g.AddComponent<RotateObject> ();
    }

    void selectRocketToEdit (RocketType type) {
        int EditingRocketIndex = settings.Rockets.FindIndex (x => x.type == type);
        buttonGroup.SetButtonActive (EditingRocketIndex);
        currentRocketsettings = settings.Rockets[EditingRocketIndex];
        // setup settingsPanel
        SetSettingsPopupData (EditingRocketIndex);
    }

    void SetSettingsPopupData (int EditingRocketIndex) {
        RocketName_SettingPopup.text = currentRocketsettings.RocketName;
        Slider_LaunchDuration.value = currentRocketsettings.LaunchDelay;
        Slider_Speed.value = currentRocketsettings.Speed;
        Slider_ExplosionRadius.value = currentRocketsettings.ExplosionRadius;
        Slider_ExplosionForce.value = currentRocketsettings.ExplosionForce;
        Slider_CameraShakeDuration.value = currentRocketsettings.CameraShakeDuration;
        Slider_CameraShakeFrequncy.value = currentRocketsettings.CameraShakeFrequncy;
    }

    public void OnChange_RocketLaunchDuration (Single value) {
        Value_LaunchDuration.text = value.ToString ("f2");
        currentRocketsettings.LaunchDelay = value;
        SaveRocketSettings ();
    }
    public void OnChange_RocketSpeed (Single value) {
        Value_Speed.text = value.ToString ("f2");
        currentRocketsettings.Speed = value;
        SaveRocketSettings ();
    }

    public void OnChange_RocketExplosionRadius (Single value) {
        Value_ExplosionRadius.text = value.ToString ("f2");
        currentRocketsettings.ExplosionRadius = value;
        SaveRocketSettings ();
    }
    public void OnChange_RocketExplosionForce (Single value) {
        Value_ExplosionForce.text = value.ToString ("00");
        currentRocketsettings.ExplosionForce = value;
        SaveRocketSettings ();
    }
    public void OnChange_CameraShakeDuration (Single value) {
        Value_CameraShakeDuration.text = value.ToString ("f2");
        currentRocketsettings.CameraShakeDuration = value;
        SaveRocketSettings ();
    }
    public void OnChange_CameraShakeFrequncy (Single value) {
        Value_CameraShakeFrequncy.text = value.ToString ("f2");
        currentRocketsettings.CameraShakeFrequncy = value;
        SaveRocketSettings ();
    }

    void SaveRocketSettings () {
        settings.Rockets[settings.Rockets.FindIndex (x => x.type == currentRocketsettings.type)] = currentRocketsettings;

    }

    public void shakeCamera () {
        cameraShake.shakeAmount = currentRocketsettings.CameraShakeFrequncy;
        cameraShake.shakeDuration = currentRocketsettings.CameraShakeDuration;
    }

}