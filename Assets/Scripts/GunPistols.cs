using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistols : MonoBehaviour {

	public string nameGun = ""; //Имя оружия (пистолета)
	public bool isGunCurrent = false;
	public float spawnSizeMagazine = 8;
	public float currentSizeMagazine = 8; //Размер магазина
	public float sizeMaxBullets = 32; //Максимальное кол-во патронов
	private float bulletForce = 5000.0f; //Скорость полета пули
	private float gilzaForce = 200.0f;
	public GameObject bulletPrefab; //Префаб пули
	public GameObject gilzaPrefab;
	public Transform stvol; //Направления ствола
	public Transform zatvor;
	public AudioSource audio; //Ауди
    public AudioClip firePistol; //Звук выстрела
    public AudioClip reloadAudio; //Звук перезарядки(3 сек.)
	public GameObject iskaPrefab; //Particle System эффект выстерла

	[SerializeField]
    private int damageHP = 1; //Урок от выстрела
	public bool reloadActive = false; //Активность перезарядки
    public float timeReload = 3.0f; //Время перезарядки
    private float fireRate = 15f; 
    private float nextTimeToFire = 0f;
	
	void Start () {
		bulletPrefab.GetComponent<Rigidbody>();
	}
	
	public void Fire(RaycastHit hit) {	
        if (Input.GetKeyDown(KeyCode.Mouse0) && reloadActive == false && currentSizeMagazine > 0 && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            audio.PlayOneShot(firePistol);
            currentSizeMagazine--;

			GameObject gilza = Instantiate(gilzaPrefab, zatvor.position, zatvor.transform.rotation) as GameObject;
			gilza.GetComponent<Rigidbody>().AddForce(transform.right * gilzaForce);

			if (hit.collider) {
	                GameObject playerFire = Instantiate(bulletPrefab, stvol.position, stvol.transform.rotation) as GameObject;
                    playerFire.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);

                    GameObject iskr = Instantiate(iskaPrefab, hit.point, hit.transform.rotation) as GameObject;
                    iskr.GetComponent<ParticleSystem>().Play();
					Debug.Log("GunPistols: Pitsol Fire");
			}
            
			if (hit.collider.gameObject.GetComponent<StatusPlayer>()) {
                    hit.collider.gameObject.GetComponent<StatusPlayer>().hpPlayerDamage(damageHP);
            }
        }
	}

	public void Reload() {
		if (Input.GetKeyDown(KeyCode.R) && sizeMaxBullets > 0)
        {
            audio.PlayOneShot(reloadAudio);
            reloadActive = true;
            StartCoroutine(Time_reload());

			sizeMaxBullets += currentSizeMagazine;
			currentSizeMagazine = 0;

			if (sizeMaxBullets >= spawnSizeMagazine) {
				sizeMaxBullets -= spawnSizeMagazine;
				currentSizeMagazine += spawnSizeMagazine;
			} else if (sizeMaxBullets < spawnSizeMagazine) {
				spawnSizeMagazine = 0;
				currentSizeMagazine = sizeMaxBullets;
			}
        }
	}

	public IEnumerator Time_reload()
    {
        yield return new WaitForSeconds(timeReload);
        reloadActive = false;
    }

	public float getCurrentBullets() {
		return currentSizeMagazine;
	}

	public float getMaxBullets() {
		return sizeMaxBullets;
	}
}
