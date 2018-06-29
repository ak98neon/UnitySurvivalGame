using UnityEngine;
using System.Collections;

public class DestroyBullets : MonoBehaviour {

	void Start () {
        Destroy(gameObject, 1.5f);
    }
}
