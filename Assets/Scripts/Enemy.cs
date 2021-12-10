using UnityEngine;
using UnityEngine.AI;

public enum BulletType {
    Bullet,
    Missile
}

public class Enemy : MonoBehaviour {
    public Color startColor;
    public Transform firePoint;
    public AudioSource audioSource;
    public GameObject impactEffectPrefab;
    public BulletType bulletType;

    private GameObject impactEffect;

    private NavMeshAgent agent;
    private Vector3 movement = Vector3.zero;
    private GameObject player;
    private Renderer _renderer;
    private GameObject bullet;
    private GameObject energy;

    private bool running = false;
    private bool firing = false;
    private float focusTime = 3f;
    private float focusCountDown = 3f;
    private Color currentColor;

    private Vector3 lookDir;
    private Quaternion lookRotation;
    private Vector3 newRotation;

    private void Awake() {
        this.impactEffect = Instantiate(this.impactEffectPrefab);
        this.impactEffect.SetActive(false);
        this.agent = this.GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag("Player");
        this._renderer = this.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            this.running = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            this.running = false;
        }
    }

    private void LateUpdate() {
        if (this.firing)
            return;
        if (this.running) {
            this.Run();
        } else {
            this.Focus();
        }
    }

    private void Run() {
        this.Reset();
        this.movement = (this.transform.position - this.player.transform.position) * Time.deltaTime * 30f;
        this.movement.y = 0;
        this.agent.SetDestination(this.movement + this.transform.position);
    }

    private void Focus() {
        this.lookDir = this.player.transform.position - this.transform.position;
        this.lookRotation = Quaternion.LookRotation(this.lookDir);
        this.newRotation = Quaternion.Lerp(this.transform.rotation, this.lookRotation, Time.deltaTime * 10f).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, this.newRotation.y, 0f);

        if (this.lookDir.magnitude > 8f) {
            this.movement = this.lookDir * Time.deltaTime * 5f;
            this.movement.y = 0;
            this.agent.SetDestination(this.movement + this.transform.position);
        }

        this.focusCountDown -= Time.deltaTime;
        this.currentColor.r += Time.deltaTime / 3;
        this._renderer.material.SetColor("_Color", this.currentColor);
        if (this.focusCountDown <= 0f) {
            this.firing = true;
            this.Fire();
        }
    }

    public void Fire() {
        this.audioSource.Play();
        this.running = false;
        Vector3 force = (this.player.transform.position - this.firePoint.position);
        if (this.bulletType == BulletType.Bullet) {
            force = force / force.magnitude * 50f;
            this.bullet = GameManager.instance.bulletPool.GetObjectFromPool();
        } else if(this.bulletType == BulletType.Missile) {
            force = force / force.magnitude * 70f;
            this.bullet = GameManager.instance.missilePool.GetObjectFromPool();
        }
        this.bullet.transform.position = this.firePoint.position;
        this.bullet.transform.LookAt(this.player.transform);
        this.bullet.SetActive(true);
        this.bullet.GetComponent<Rigidbody>().AddForce(force);
        this.Reset();
    }

    private void Reset() {
        this.focusCountDown = this.focusTime;
        this.currentColor = this.startColor;
        this._renderer.material.SetColor("_Color", this.startColor);
        this.firing = false;
    }

    public void Die() {
        this.impactEffect.transform.position = this.transform.position;
        this.impactEffect.SetActive(true);
        this.gameObject.SetActive(false);
        this.energy = GameManager.instance.energyPool.GetObjectFromPool();
        this.energy.transform.position = this.transform.position;
        if (this.bulletType == BulletType.Missile) {
            this.energy.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            this.energy.GetComponent<Energy>().energy = 20f;
        }
        this.energy.SetActive(true);
        this.gameObject.SetActive(false);
        this.running = false;
        this.Reset();
        GameManager.instance.EnemyDied(this.transform.position);
    }
}
