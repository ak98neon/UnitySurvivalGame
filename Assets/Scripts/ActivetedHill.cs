/*
 * @author Artem Kudrya
 * @date 05.11.2016
 * @see Назначение: При столкновении игрока с хилкой, жихни игрока увеличиваются на заданное кол-во.
 */

using UnityEngine;
using System.Collections;

public class ActivetedHill : MonoBehaviour {

    private StatusPlayer player;
    private int damageHp;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("UPs");
            player = collision.gameObject.GetComponent<StatusPlayer>();
            player.hpPlayerDamage(damageHp);

            Destroy(gameObject);
        }
    }
}
