using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

public float fireRate = 10f;
public int maxAmmo = 30;
public int startingAmmo = 10;
public float bulletSpeed = 100f;
public Transform firePoint;
private int currentAmmo;
[SerializeField] GameObject muzzle;
[SerializeField] AudioSource gunSound;
private bool isAuto = false;
private void Start() {
    currentAmmo = startingAmmo;
}

void Update() {
    if (Input.GetButtonDown("Fire1") && currentAmmo > 0) {
        Fire();
    }

    if (isAuto && Input.GetButton("Fire1") && currentAmmo > 0) {
        Fire();
    }
}

void Fire() {
    currentAmmo--; // Decrement the current ammo count
    GameObject muzzleOb = Instantiate(muzzle,firePoint.position,Quaternion.identity);
    Destroy(muzzleOb,1f);
    gunSound.Play();
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit)) {
        Debug.DrawLine(firePoint.position, hit.point, Color.red, 1f);
        // Add code here to apply damage to the hit object or perform other actions
    } else {
        Debug.DrawLine(firePoint.position, ray.origin + (ray.direction * 100f), Color.red, 1f);
    }
    StartCoroutine(Reload());
}

IEnumerator Reload() {
    if (currentAmmo == 0) {
        yield return new WaitForSeconds(1.5f);
        currentAmmo = maxAmmo;
    }
}
}
