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
private bool isAuto = false;
private void Start() {
    currentAmmo = startingAmmo;
}

void Update() {
    if (Input.GetButtonDown("Fire1") && currentAmmo > 0) { // Check if left mouse button is pressed and there is ammo remaining
        Fire();
    }

    if (isAuto && Input.GetButton("Fire1") && currentAmmo > 0) { // Check if gun is automatic and left mouse button is held down
        Fire();
    }
}

void Fire() {
    currentAmmo--; // Decrement the current ammo count

    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Create a raycast from the center of the screen
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit)) { // Cast the raycast and check if it hits anything
        Debug.DrawLine(firePoint.position, hit.point, Color.red, 1f); // Draw a debug line from the fire point to the point of impact
        // Add code here to apply damage to the hit object or perform other actions
    } else {
        Debug.DrawLine(firePoint.position, ray.origin + (ray.direction * 100f), Color.red, 1f); // Draw a debug line from the fire point to the end of the raycast
    }
    StartCoroutine(Reload()); // Start reloading if out of ammo
}

IEnumerator Reload() {
    if (currentAmmo == 0) {
        yield return new WaitForSeconds(1.5f); // Wait 1.5 seconds to reload
        currentAmmo = maxAmmo; // Reset the ammo count to max capacity
    }
}
}
