using System.Collections;
using UnityEngine;

public class Energy : MonoBehaviour {
    public float energy = 10f;
    public float countdown = 3f;
    public int score = 10;

    private void OnEnable() {
        StartCoroutine(WaitUntilPick());
        this.energy = 10f;
        this.transform.localScale = Vector3.one;
    }

    IEnumerator WaitUntilPick() {
        yield return new WaitForSeconds(this.countdown);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameManager.instance.ChangeEnergy(this.energy);
            GameManager.instance.AddScore(this.score);
            this.gameObject.SetActive(false);
        }
    }
}
