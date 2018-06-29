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

    public void alive()
    {
        if (hp_Player <= 0)
        {
            Destroy(gameObject);
        }
    }
}
