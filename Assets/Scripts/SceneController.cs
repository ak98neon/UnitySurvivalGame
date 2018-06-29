using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
    [SerializeField]
    private GameObject enemyPrefab;

    //private GameObject _enemy;
    private GameObject[] _enemyVrag;
    public int countVrag = 1;

    private GameObject vrag;
    private GameObject player;

	void Start () {
        _enemyVrag = new GameObject[countVrag];
	}
	
	void Update () {
        spawnVrag();
	}

    void FixedUpdate()
    {
        vrag = GameObject.FindGameObjectWithTag("Vrag");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void spawnVrag()
    {
        /*if (_enemy == null && countVrag != 0)
        {
            countVrag--;
            _enemy = Instantiate(enemyPrefab) as GameObject;
            _enemy.transform.position = gameObject.transform.position;

            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }*/

        for (int i = 0; i < _enemyVrag.Length; i++)
        {
            if (_enemyVrag[i] == null && countVrag != 0)
            {
                countVrag--;
                _enemyVrag[i] = Instantiate(enemyPrefab) as GameObject;
                _enemyVrag[i].transform.position = gameObject.transform.position;

                float angle = Random.Range(0, 360);
                _enemyVrag[i].transform.Rotate(0, angle, 0);
            }
        }
    }

    IEnumerator zadershka()
    {
        yield return new WaitForSeconds(3);
    }

    public void OnGUI()
    {
        if (vrag == null)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 150), "Player WIN!");
        }

        if (player == null)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 150), "Player LOSE!");
        }
    }
}
