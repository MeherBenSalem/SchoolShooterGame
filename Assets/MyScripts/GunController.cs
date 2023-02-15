using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

[Header("Ammo Settings")]
[SerializeField] int maxMagazine = 30;
[SerializeField] int startingAmmo = 30;

[Header("Bullet Settings")]
[SerializeField] int damage;
[SerializeField] float range=20f;

[Header("Weapon Settings")]
[SerializeField] Transform firePoint;
[SerializeField] float reloadTime = 1.5f;
[SerializeField] LayerMask vitalsLayer;
[SerializeField] LayerMask deadBodyLayer;

[Header("Cosmetics Settings")]
[SerializeField] GameObject muzzle;
[SerializeField] AudioClip shootingSound,reloadSound;
[SerializeField] GameObject impactPrefab;
AudioSource gunSound;
private Animator an;
private bool isReloading = false;
private int currentAmmo;

private void Start() {
    currentAmmo = startingAmmo;
    an = GetComponent<Animator>();
    ItemChange.instance.updateBulletCount(currentAmmo);
    gunSound = GetComponent<AudioSource>();
}

void Update() {
    if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && !isReloading) {
        Fire();
    }
}

void Fire() {
    currentAmmo--;
    ItemChange.instance.updateBulletCount(currentAmmo);
    GameObject muzzleOb = Instantiate(muzzle,firePoint.position,Quaternion.identity);
    Destroy(muzzleOb,1f);
    an.SetTrigger("shoot");
    PlaySound(shootingSound);
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    RaycastHit hit;
    if (Physics.Raycast(ray,out hit,range,vitalsLayer)){
        if(hit.transform.GetComponentInChildren<VitalsAnimator>())
        hit.transform.GetComponentInChildren<VitalsAnimator>().Damage(damage);
        if(hit.transform.GetComponentInChildren<BloodControl>())
        hit.transform.GetComponentInChildren<BloodControl>().Bleed(hit);
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
        currentAmmo = maxMagazine;
        ItemChange.instance.updateBulletCount(currentAmmo);
        isReloading = false;
    }
}
void PlaySound(AudioClip clip){
    gunSound.clip = clip;
    gunSound.Play();
}
}
