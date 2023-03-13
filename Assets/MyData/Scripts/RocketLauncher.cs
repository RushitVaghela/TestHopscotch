using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RocketLauncher : MonoBehaviour {

    public static RocketLauncher instance;
    Camera mainCamera;
    
    [SerializeField] GameObject WarningPrefab;
    [SerializeField] Transform WarningContainer;
    [SerializeField] GameObject RocketPrefab;
    [SerializeField] Transform RocketContainer;
    [SerializeField] List<GameObject> PooledRockets;
    [SerializeField] List<GameObject> PooledWarnings;
    [SerializeField] int DefaultPoolCount = 5;

    private void Awake () {
        if (instance == null) instance = this;
        mainCamera = Camera.main;
    }
    void Start () {
    }

    void Update () {
        if (Input.GetMouseButtonDown (0) && !IsPointerOverUIObject ()) {
            setTargetAndLaunch ();
        }
    }

    public void ChangeRocket (GameObject NewRocket) {
        RocketPrefab = NewRocket;
        CreateRocketPool ();
    }

   
    #region Pooling Warnings
    void CreateWarningPool () {
        PooledWarnings.ForEach (warning => Destroy (warning));
        PooledWarnings = new List<GameObject> ();

        for (int i = 0; i < DefaultPoolCount; i++) {
            PooledWarnings.Add (CreateNewWarningObj ());
        }
    }

    GameObject CreateNewWarningObj () {
        GameObject newWarning = Instantiate (WarningPrefab, WarningContainer);
        newWarning.gameObject.SetActive (false);
        return newWarning;
    }

    GameObject GetWarningObjToPlace () {
        for (int i = 0; i < PooledWarnings.Count; i++) {
            if (!PooledWarnings[i].activeSelf) {
                return PooledWarnings[i];
            }
        }

        GameObject warning = CreateNewWarningObj ();
        PooledWarnings.Add (warning);
        return warning;
    }
    #endregion

    #region Pooling Rockets
    void CreateRocketPool () {
        PooledRockets.ForEach (rocket => Destroy (rocket));
        PooledRockets = new List<GameObject> ();

        for (int i = 0; i < DefaultPoolCount; i++) {
            PooledRockets.Add (CreateNewRocket ());
        }
    }

    GameObject CreateNewRocket () {
        GameObject newRocket = Instantiate (RocketPrefab, RocketContainer);
        // newRocket.SetActive (false);
        newRocket.GetComponent<Projectile>().ResetRocket ();
        return newRocket;
    }

    GameObject GetRocketToLaunch () {
        for (int i = 0; i < PooledRockets.Count; i++) {
            if (!PooledRockets[i].activeSelf) {
                Projectile p= PooledRockets[i].GetComponent<Projectile>();
                p.SetupRocket(UIManager.instance.currentRocketsettings);
                return PooledRockets[i];
            }
        }

        GameObject roket = CreateNewRocket ();
        PooledRockets.Add (roket);
        return roket;
    }
    #endregion

    void setTargetAndLaunch () {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out hit, 100)) {
            // Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log ("Did Hit");
            LaunchRocketWithWarning (hit.point);
        } else {
            // Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * 1000, Color.white);
            Debug.Log ("Did not Hit");
            // LaunchRocket (ray.direction);
        }
    }

    void LaunchRocketWithWarning (Vector3 HitPoint) {
        GameObject warning = GetWarningObjToPlace ();
        warning.SetActive (true);
        warning.transform.position = HitPoint;

        GameObject rocket = GetRocketToLaunch ();
        rocket.SetActive (true);
        rocket.transform.position = transform.position;
        rocket.transform.LookAt (HitPoint);
        rocket.GetComponent<Projectile> ().Launch (warning);

    }
    void LaunchRocket (Vector3 targetDirection) {
        GameObject rocket = GetRocketToLaunch ();
        rocket.SetActive (true);
        rocket.transform.forward = targetDirection;
        rocket.GetComponent<Projectile> ().Launch (null);
    }

    public bool IsPointerOverUIObject () {

        if (EventSystem.current == null) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}