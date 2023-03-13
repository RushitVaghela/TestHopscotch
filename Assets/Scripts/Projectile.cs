using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Transform _transform;
    Rigidbody _RB;
    public RocketType rocketType;
    [SerializeField] GameObject RocketModel;
    [SerializeField] ParticleSystem ExplosionParticles;
    [SerializeField] float _currentLaunchDelay = 2f;
    [SerializeField] bool isLaunched = false;
    [SerializeField] bool exploded = false;
    Coroutine launchCoroutine = null;
    GameObject ProjectileHitPoint = null;

    private void Awake () {
        _transform = transform;
        _RB = GetComponent<Rigidbody> ();

    }

    void Update () {
        if (isLaunched && !exploded) {

            _currentLaunchDelay -= Time.deltaTime;
            if (_currentLaunchDelay <= 0) {
                _RB.velocity = _transform.forward * UIManager.instance.currentRocketsettings.Speed;
            }
        }
    }

    public void SetupRocket (RocketData data) {
       
        gameObject.SetActive (true);
    }
    private void OnTriggerEnter (Collider other) {
        // Destroy (gameObject);

        Explode ();
    }

    public void Launch (GameObject projectileHitPoint) {

        isLaunched = true;
        ProjectileHitPoint = projectileHitPoint;
        launchCoroutine = StartCoroutine (RecollectAfter ());
    }
    IEnumerator RecollectAfter () {
        yield return new WaitForSeconds (UIManager.instance.currentRocketsettings.DefaultResetTime);
        ResetRocket ();
    }

    void Explode () {
        exploded = true;
        _RB.velocity = Vector3.zero;
        _RB.isKinematic = true;
        RocketModel.SetActive (false);
        ExplosionParticles.gameObject.SetActive (true);
        UIManager.instance.shakeCamera();
        ExplosionParticles.Play ();
        if (ProjectileHitPoint != null) {
            ProjectileHitPoint.SetActive (false);
            ProjectileHitPoint = null;
        }

        Collider[] colliders = Physics.OverlapSphere (_transform.position, UIManager.instance.currentRocketsettings.ExplosionRadius);
        foreach (Collider obj in colliders) {
            Rigidbody rb = obj.GetComponent<Rigidbody> ();
            if (rb != null) {
                rb.AddExplosionForce (UIManager.instance.currentRocketsettings.ExplosionForce, _transform.position, UIManager.instance.currentRocketsettings.ExplosionRadius);
            }
        }
        StartCoroutine (ResetRocketAfterExposion ());
    }

    IEnumerator ResetRocketAfterExposion () {
        yield return new WaitForSeconds (ExplosionParticles.main.duration); // explosion delay
        yield return new WaitForSeconds (1); // extra delay
        ResetRocket ();
    }

    public void ResetRocket () {
        isLaunched = false;
        exploded = false;
        _RB.isKinematic = false;
        _RB.velocity = Vector3.zero;
        _currentLaunchDelay = UIManager.instance.currentRocketsettings.LaunchDelay;
        ExplosionParticles.gameObject.SetActive (false);
        gameObject.SetActive (false);
        RocketModel.SetActive (true);
        if (ProjectileHitPoint != null) {
            ProjectileHitPoint.SetActive (false);
            ProjectileHitPoint = null;
        }
        if (launchCoroutine != null) {
            StopCoroutine (launchCoroutine);
            launchCoroutine = null;
        }
    }
}