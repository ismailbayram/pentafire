using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject wallEffectPrefab;

    private GameObject impactEffect;
    private GameObject wallEffect;

    private Rigidbody rgdbody;

    private void Awake() {
        this.rgdbody = this.GetComponent<Rigidbody>();
        this.wallEffect = Instantiate(this.wallEffectPrefab);
        this.wallEffect.SetActive(false);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Enemy") {
            other.transform.GetComponent<Enemy>().Die();
            this.Deactivate();
        } else if(other.transform.tag == "Player") {
            GameManager.instance.CheckPlayerDeath();
            this.Deactivate();
        } else if (other.transform.tag == "Reflector") {
            
        } else {
            this.wallEffect.transform.position = this.transform.position;
            this.wallEffect.SetActive(true);
            this.Deactivate();
        }
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        this.rgdbody.velocity = Vector3.zero;
    }
}
