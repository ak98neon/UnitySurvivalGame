using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour {

    public float speed = 10.0f;
    public int damageHP = 1;
	
	void Update () {
        transform.Translate(0, 0, speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        StatusPlayer player = other.GetComponent<StatusPlayer>();
        if (player != null)
        {
            player.hpPlayerDamage(damageHP);
        }
        Destroy(this.gameObject);
    }
}
