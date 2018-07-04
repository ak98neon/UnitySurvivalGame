/*
 * @author Artem Kudrya
 * @date 05.11.2016
 * @see Назначение: Скрипт который отвечает за инвентарь игрока, жизни, броню
 */

using UnityEngine;
using System.Collections;

public class StatusPlayer : MonoBehaviour
{

    [SerializeField]
    private int hp_Player = 10;
    [SerializeField]
    private int armor_Player = 10;

    public int statusHpPlayer { get; set; }
    public int statusArmorPlayer { get; set; }

    void Update()
    {
        alive();
    }

    public void hpPlayerDamage(int damage)
    {
        this.hp_Player -= damage;
    }

    public void armorPlayerDamage(int damage)
    {
        this.armor_Player -= damage;
    }

    public bool isAlive() {
        return hp_Player > 0 ? true : false;
    }

    public void alive()
    {
        if (hp_Player <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnGUI() {
        if (isAlive()) {
            GUI.Label(new Rect(Screen.width/2-5, 10, 150, 30), "HP PLAYER: " + hp_Player);
        }
    }
}
