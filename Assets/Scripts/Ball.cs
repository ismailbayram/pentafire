using UnityEngine;

public class Ball : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Enemy") {
            other.transform.GetComponent<Enemy>().Die();
        }
    }
}
