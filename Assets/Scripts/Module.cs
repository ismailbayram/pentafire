using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ModuleType {
    Default,
    Speedy,
    Reflector,
    Balls
}

public class Module : MonoBehaviour {
    public ModuleType module;
    public float countdown = 10f;
    public Slider moduleTimer;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            this.ActivateModule();
        }
    }

    public void ActivateModule() {
        GameManager.instance.player.ChangeMod(this.module, this.countdown);
        this.gameObject.SetActive(false);
        this.moduleTimer.gameObject.SetActive(true);
        GameManager.instance.audioSource.PlayOneShot(GameManager.instance.moduleClip);
    }

    private void OnEnable() {
        StartCoroutine(WaitUntilPick());
    }

    IEnumerator WaitUntilPick() {
        yield return new WaitForSeconds(10f);
        this.gameObject.SetActive(false);
        GameManager.instance.anyActiveModuleExists = false;
    }
}
