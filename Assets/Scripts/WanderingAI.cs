using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour {

    public float speed = 5.0f;
    public float obstacleRange = 6.0f;
    public float forceFire = 3000.0f;

    private bool alive;

    public AudioSource audio;
    public AudioClip audioFireBot;

    [SerializeField]
    public GameObject _bullet;
    private GameObject playerPosition;
    private Rigidbody botRB;
    public Transform ruki;

    private int damagePlayerHp = 1;

    void Start()
    {
        botRB = GetComponent<Rigidbody>();
        if (botRB != null)
        {
            botRB.freezeRotation = false;
        }

        playerPosition = GameObject.FindWithTag("Player");

        alive = true;
    }

    void Update()
    {
        Move();
        AttackPlayer();
    }

    public void Move()
    {
        Ray rayForward = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(rayForward, 0.75f, out hit, 100.0f))
        {
            GameObject hitObject = hit.transform.gameObject;
            StatusPlayer player = hitObject.GetComponent<StatusPlayer>();
            if (player != null)
            {
                //player.hpPlayerDamage(damagePlayerHp);
            }

            if (hit.distance < obstacleRange && player == null)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, playerPosition.transform.position) > 10.0f)
        {
            if (alive)
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
        } else if (Vector3.Distance(transform.position, playerPosition.transform.position) <= 10.0f && Vector3.Distance(transform.position, playerPosition.transform.position) > 8.0f)
        {
            transform.LookAt(playerPosition.transform);
            transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
        }

        if (Vector3.Distance(transform.position, playerPosition.transform.position) <= 10.0f)
        {
            if (audioFireBot != null)
            {
               // audio.PlayOneShot(audioFireBot);
            }

            transform.LookAt(playerPosition.transform);
            //Rigidbody fireBot = Instantiate(_bullet, ruki.position, ruki.rotation) as Rigidbody;
            //fireBot.AddForce(ruki.forward * forceFire);
        }
    }

    public void SetAlive(bool alive)
    {
        this.alive = alive;
    }
}