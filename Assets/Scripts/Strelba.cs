/*
 * @author Artem Kudrya
 * @date 05.11.2016
 * @see Назначение: Скрипт отвечает за стрельбу игрока, и перезарядку
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <remarks>
/// Скрипт отвечает за стрельбу, перезарядку, и меню настройки
/// </remarks>
public class Strelba : MonoBehaviour
{
    #region Variables
    public enum menuAlive { game = 0, setting = 1 }; //Положение активно ли меню, или нет
    public menuAlive polKursor;
    public Camera mainCamera; //Главная камера
    private int countGrenade = 3; //Кол-во гранат
    public GameObject grenadePrefab; //Префаб гранаты
    public Transform ruki; //Положение места от куда будут кидаться гранаты, тоесть руки игрока
    private float forceGrenade = 500.0f; //Сила броска гранаты

    public Texture2D crossHair;
    private Rect positionCrosshair;

    //Animation
    private Animator _animator;

    [SerializeField]
    public GameObject []listGun = new GameObject[3];
    public GameObject currentGun;
    #endregion

    /// <summary>
    /// Standart method in Unity, launched at the start of the script
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();

        positionCrosshair = new Rect((Screen.width - crossHair.width) / 2, (Screen.height - crossHair.height) / 2, crossHair.width, crossHair.height);

        polKursor = menuAlive.game;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (listGun.Length != 0) {
            currentGun = listGun[0];
            listGun[0].SetActive(true);
            listGun[1].SetActive(false);
        }
    }

    /// <summary>
    /// Standart method in Unity, it called each frame/sec
    /// </summary>
    void Update()
    {
        FireCurrentGun();
        ReloadCurrentGun();
        activeSettings();
        switchGun();
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
    public void switchGun()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            currentGun = listGun[0];
            listGun[0].SetActive(true); //Pistol
            listGun[1].SetActive(false); //Rifle
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            currentGun = listGun[1];
            listGun[0].SetActive(false); //Pistol
            listGun[1].SetActive(true); //Rifle
        }

        if (Input.GetKey(KeyCode.Alpha0)) {
            currentGun = null;
            for (int i = 0; i < listGun.Length; i++) {
                listGun[i].SetActive(false); //All guns deactivate
            }
        }
    }

    /// <summary>
    /// Resposible for fire with pistol
    /// </summary>
    public void FireCurrentGun()
    {
        Debug.Log(currentGun);
        if (Input.GetKey(KeyCode.Mouse0) && polKursor == menuAlive.game && currentGun != null)
        {
            Vector3 point;
            if (currentGun.GetComponent<GunPistols>()) {
                point = new Vector3(mainCamera.pixelWidth/2, mainCamera.pixelHeight/2, 0);
            } else {
                int posx = Random.Range(1, 10);
                int posy = Random.Range(1, 10);
                point = new Vector3(mainCamera.pixelWidth/2, mainCamera.pixelHeight/2 + posx - posy, 0);
            }
            Ray ray = mainCamera.ScreenPointToRay(point);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit)) {
                Debug.Log("Fire Pistol");
                if (currentGun.GetComponent<GunPistols>()) {
                    currentGun.GetComponent<GunPistols>().Fire(hit);
                }

                if (currentGun.GetComponent<GunRifle>()) {
                    currentGun.GetComponent<GunRifle>().Fire(hit);
                }
            }
        }
    }

    public void ReloadCurrentGun() {
        if (currentGun != null) {
            if (currentGun.GetComponent<GunPistols>()) {
                currentGun.GetComponent<GunPistols>().Reload();
            }

            if (currentGun.GetComponent<GunRifle>()) {
                currentGun.GetComponent<GunRifle>().Reload();
            }
        }
    }

    void OnGUI()
    {
        if (mainCamera != null)
        {
            GUI.DrawTexture(positionCrosshair, crossHair);
        }

        if (currentGun.GetComponent<GunPistols>())
        {
            float currentBullets = currentGun.GetComponent<GunPistols>().getCurrentBullets();
            float sizeMaxBullets = currentGun.GetComponent<GunPistols>().getMaxBullets();

            GUI.Label(new Rect(5, 30, 120, 30), "Пистолет: [" + currentBullets + "/" + sizeMaxBullets + "]");

            if (currentBullets == 0) {
                GUI.Label(new Rect(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2 + 200, 150, 30), "Перезарядите [R]");
            }
        }
        
        if (currentGun.GetComponent<GunRifle>())
        {
            float currentBullets = currentGun.GetComponent<GunRifle>().getCurrentBullets();
            float sizeMaxBullets = currentGun.GetComponent<GunRifle>().getMaxBullets();

            GUI.Label(new Rect(5, 30, 120, 30), "Автомат: [" + currentBullets + "/" + sizeMaxBullets + "]");

            if (currentBullets == 0) {
                GUI.Label(new Rect(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2 + 200, 150, 30), "Перезарядите [R]");
            }
        }
    }
}
    /*
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
    */
