﻿/*
 * @author Artem Kudrya
 * @date 05.11.2016
 * @see Назначение: Скрипт отвечает за стрельбу игрока, и перезарядку
 */

using UnityEngine;
using System.Collections;

/// <remarks>
/// Скрипт стрельба отвечает за стрельбу, перезарядку, и меню настройки
/// </remarks>
public class Strelba : MonoBehaviour
{
    #region Variables
    public enum CurrentWeapon { pistol, rifle } //Состояние оружия(какое сейчас активно в руках у игрока)
    public CurrentWeapon currentGun;

    public enum menuAlive { game = 0, setting = 1 }; //Положение активно ли меню, или нет
    public menuAlive polKursor;

    public Rigidbody bulletPrefab;
    public Transform rukiPlayer;
    public float bulletForce = 500000.0f;

    public Camera mainCamera; //Главная камера
    public ParticleSystem iskra; //Эффект искр от попадания
    public AudioSource audio;
    public AudioClip firePistol; //Звук выстрела
    public AudioClip reloadAudio; //Звук перезарядки(3 сек.)

    [SerializeField]
    private int damageHP = 1; //Урок от выстрела

    [SerializeField]
    private int spawnCountAmmoPistol = 8; //Вместимость обоймы в пистолете
    private int allAmmoPistol = 30; //Кол-во патронов к пистолету, в инвентаре
    private int ammoPistol = 8; //Кол-во патронов к пистолету в обойме

    [SerializeField]
    private int spawnCountAmmoRifle = 30; //Вместимость обоймы в автомате
    private int allAmmoRifle = 90; //Кол-во патронов к автомату, в инвентаре
    private int ammoRifle = 30; //Кол-во патронов к автомату в обойме

    [SerializeField]
    private int countGrenade = 3; //Кол-во гранат
    public GameObject grenadePrefab; //Префаб гранаты
    public Transform ruki; //Положение места от куда будут кидаться гранаты, тоесть руки игрока
    private float forceGrenade = 500.0f; //Сила броска гранаты

    public bool reloadActive = false; //Активность перезарядки
    public float timeReload = 3.0f; //Время перезарядки

    public Texture2D crossHair;
    private Rect positionCrosshair;

    //Animation
    private Animator _animator;
    #endregion

    /// <summary>
    /// Standart method in Unity, launched at the start of the script
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();

        positionCrosshair = new Rect((Screen.width - crossHair.width) / 2, (Screen.height - crossHair.height) / 2, crossHair.width, crossHair.height);

        bulletPrefab.GetComponent<Rigidbody>();
        
        ammoPistol = spawnCountAmmoPistol;
        ammoRifle = spawnCountAmmoRifle;

        currentGun = CurrentWeapon.pistol;
        polKursor = menuAlive.game;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Standart method in Unity, it called each frame/sec
    /// </summary>
    void Update()
    {
        activeSettings();
        vuborGun();
        FirePistol();
        FireRifle();
        useGrenade();
        reload();
    }

    /// <summary>
    /// Panel settings, open/close
    /// </summary>
    public void activeSettings()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && polKursor == menuAlive.game)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.GetComponent<MouseLook>().enabled = false;
            gameObject.GetComponent<FPSInput>().enabled = false;
            mainCamera.GetComponent<MouseLook>().enabled = false;
            polKursor = menuAlive.setting;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && polKursor == menuAlive.setting)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameObject.GetComponent<MouseLook>().enabled = true;
            gameObject.GetComponent<FPSInput>().enabled = true;
            mainCamera.GetComponent<MouseLook>().enabled = true;
            polKursor = menuAlive.game;
        }
    }

    /// <summary>
    /// Switch gun
    /// </summary>
    public void vuborGun()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            currentGun = CurrentWeapon.pistol;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            currentGun = CurrentWeapon.rifle;
        }
    }

    /// <summary>
    /// Resposible for fire with pistol
    /// </summary>
    public void FirePistol()
    {
        rukiPlayer.rotation = mainCamera.GetComponent<Transform>().rotation;

        if (Input.GetKeyDown(KeyCode.Mouse0) && reloadActive == false && ammoPistol > 0 && polKursor == menuAlive.game && currentGun == CurrentWeapon.pistol)
        {
            audio.PlayOneShot(firePistol);
            ammoPistol--;

            Rigidbody playerFire = Instantiate(bulletPrefab, ruki.position, ruki.rotation) as Rigidbody;
            playerFire.AddForce(ruki.forward * bulletForce);
            Physics.IgnoreCollision(bulletPrefab.GetComponent<Collider>(), GetComponent<Collider>()); //Игнорируем коллайдер игрока, чтобы игрок не убивал сам себя
            /*Vector3 point = new Vector3(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2, 0);
            Ray ray = mainCamera.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                StatusPlayer player = hitObject.GetComponent<StatusPlayer>();

                if (target != null)
                {
                    target.ReactToHit();
                }
                else
                {
                    iskra.transform.position = hit.point;
                    iskra.Play();
                    if (player != null)
                    {
                        player.hpPlayerDamage(damageHP);
                    }
                    else
                    {
                        StartCoroutine(SphereIndicator(hit.point));
                    }
                }
            }*/
        }
    }

    /// <summary>
    /// Resposible for fire with rifle
    /// </summary>
    public void FireRifle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && reloadActive == false && ammoRifle > 0 && polKursor == menuAlive.game && currentGun == CurrentWeapon.rifle)
        {
            audio.PlayOneShot(firePistol);
            ammoRifle--;
            int posx = Random.Range(1, 10);
            int posy = Random.Range(1, 10);
            Vector3 point = new Vector3(mainCamera.pixelWidth / 2 - posx + posy, mainCamera.pixelHeight / 2 + posx - posy, 0);
            Ray ray = mainCamera.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                StatusPlayer player = hitObject.GetComponent<StatusPlayer>();

                if (target != null)
                {
                    target.ReactToHit();
                }
                else
                {
                    iskra.transform.position = hit.point;
                    iskra.Play();
                    if (player != null)
                    {
                        player.hpPlayerDamage(damageHP);
                    }
                    else
                    {
                        StartCoroutine(SphereIndicator(hit.point));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Resposible for throw grenades
    /// </summary>
    public void useGrenade()
    {
        if (Input.GetKeyDown(KeyCode.G) && countGrenade > 0)
        {
            if (grenadePrefab != null && ruki != null)
            {
                countGrenade--;
                //Rigidbody throwGrenade = Instantiate(grenadePrefab, ruki.position, ruki.rotation) as Rigidbody;
                //throwGrenade.AddForce(ruki.forward * forceGrenade * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Resposible for reload weapons
    /// </summary>
    public void reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentGun == CurrentWeapon.pistol && ammoPistol != spawnCountAmmoPistol && allAmmoPistol > 0)
        {
            audio.PlayOneShot(reloadAudio);
            reloadActive = true;
            StartCoroutine(Time_reload());
            allAmmoPistol += ammoPistol;

            if (allAmmoPistol == spawnCountAmmoPistol)
            {
                ammoPistol = allAmmoPistol;
            }
            else if (allAmmoPistol > spawnCountAmmoPistol)
            {
                ammoPistol = spawnCountAmmoPistol;
                allAmmoPistol -= spawnCountAmmoPistol;
            }
            else if (allAmmoPistol < spawnCountAmmoPistol)
            {
                ammoPistol = allAmmoPistol;
                allAmmoPistol = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentGun == CurrentWeapon.rifle && ammoRifle != spawnCountAmmoRifle && allAmmoRifle > 0)
        {
            audio.PlayOneShot(reloadAudio);
            reloadActive = true;
            StartCoroutine(Time_reload());
            allAmmoRifle += ammoRifle;

            if (allAmmoRifle == spawnCountAmmoRifle)
            {
                ammoRifle = allAmmoRifle;
            } else if (allAmmoRifle > spawnCountAmmoRifle)
            {
                ammoRifle = spawnCountAmmoRifle;
                allAmmoRifle -= spawnCountAmmoRifle;
            } else if (allAmmoRifle < spawnCountAmmoRifle)
            {
                ammoRifle = allAmmoRifle;
                allAmmoRifle = 0;
            }
        }
    }

    public IEnumerator Time_reload()
    {
        yield return new WaitForSeconds(timeReload);
        reloadActive = false;
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }

    void OnGUI()
    {
        if (mainCamera != null)
        {
            GUI.DrawTexture(positionCrosshair, crossHair);
        }

        if (currentGun == CurrentWeapon.pistol)
        {
            GUI.Label(new Rect(5, 30, 100, 30), "Пистолет: [" + ammoPistol + "/" + allAmmoPistol + "]");
        } else if (currentGun == CurrentWeapon.rifle)
        {
            GUI.Label(new Rect(5, 30, 120, 30), "Автомат: [" + ammoRifle + "/" + allAmmoRifle + "]");
        }

        if (ammoPistol == 0 && allAmmoPistol > 0 && currentGun == CurrentWeapon.pistol) {
            GUI.Label(new Rect(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2 + 200, 150, 30), "Перезарядите [R]");
        } else if (ammoRifle == 0 && allAmmoRifle > 0 && currentGun == CurrentWeapon.rifle)
        {
            GUI.Label(new Rect(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2 + 200, 150, 30), "Перезарядите [R]");
        }
    }
}
