using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
public int maxAmmo = 30;
public int startingAmmo = 10;
public float bulletSpeed = 100f;
public Transform firePoint;
private int currentAmmo;
[SerializeField] int damage;
[SerializeField] float range=20f;
[SerializeField] GameObject muzzle;
[SerializeField] AudioClip shootingSound,reloadSound;
[SerializeField] GameObject impactPrefab;
[SerializeField] LayerMask vitalsLayer;
AudioSource gunSound;
private Animator an;
private bool isReloading = false;
public float reloadTime = 1.5f;

private void Start() {
    currentAmmo = startingAmmo;
    an = GetComponent<Animator>();
    gunSound = GetComponent<AudioSource>();
}

void Update() {
    if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && !isReloading) {
        Fire();
    }
}

void Fire() {
    currentAmmo--;
    GameObject muzzleOb = Instantiate(muzzle,firePoint.position,Quaternion.identity);
    Destroy(muzzleOb,1f);
    an.SetTrigger("shoot");
    PlaySound(shootingSound);
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    RaycastHit hit;
    if (Physics.Raycast(ray,out hit,range,vitalsLayer)){
        hit.transform.GetComponentInChildren<VitalsAnimator>().Damage(damage);
    }
    else if(Physics.Raycast(ray,out hit,range)) {
        GameObject impact = Instantiate(impactPrefab,hit.point,Quaternion.LookRotation(hit.normal));
        impact.transform.position += impact.transform.forward/1000;
        Destroy(impact,5f);
    }
    StartCoroutine(Reload());
}

IEnumerator Reload() {
    if(currentAmmo==0){
        isReloading = true;
        an.SetTrigger("reload");
        PlaySound(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
void PlaySound(AudioClip clip){
    gunSound.clip = clip;
    gunSound.Play();
}
}
