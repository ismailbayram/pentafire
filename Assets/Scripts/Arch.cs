using UnityEngine;

public class Arch : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Enemy") {
            other.transform.GetComponent<Enemy>().Die();
        }
    }
}
