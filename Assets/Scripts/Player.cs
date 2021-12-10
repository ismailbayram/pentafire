using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public ModuleType currentMod;
    public GameObject speedEffect;
    public GameObject reflectorEffect;
    public GameObject ballsEffect;
    public Slider moduleTimer;
    public Collider boxCollider;
    public MeshRenderer meshRenderer;

    [HideInInspector]
    public GameObject arch;
    
    private PlayerMovement movement;

    private float currentModuleCountdown;
    private float currentModExpireDuration;

    private Coroutine coroutine;

    private void Start() {
        this.movement = this.GetComponent<PlayerMovement>();
        this.currentMod = ModuleType.Default;
    }

    private void Update() {
        if(this.currentMod != ModuleType.Default) {
            this.currentModuleCountdown -= Time.deltaTime;
            this.moduleTimer.value = this.currentModuleCountdown / this.currentModExpireDuration * 100;
        }
    }

    public void ChangeMod(ModuleType mod, float expireDuration) {
        this.currentMod = mod;
        this.currentModuleCountdown = expireDuration;
        this.currentModExpireDuration = expireDuration;
        switch (mod) {
            case ModuleType.Default:
                break;
            case ModuleType.Speedy:
                this.speedEffect.SetActive(true);
                this.movement.speed = 9f;
                break;
            case ModuleType.Reflector:
                this.reflectorEffect.SetActive(true);
                break;
            case ModuleType.Balls:
                this.ballsEffect.SetActive(true);
                break;
            default:
                break;
        }
        this.coroutine = StartCoroutine(DeactivateModule(expireDuration));
    }

    IEnumerator DeactivateModule(float duration) {
        yield return new WaitForSeconds(duration);
        this.ResetModule();
    }

    public void ResetModule() {
        this.currentMod = ModuleType.Default;
        this.speedEffect.SetActive(false);
        this.movement.speed = this.movement.baseSpeed;
        this.reflectorEffect.SetActive(false);
        this.ballsEffect.SetActive(false);
        GameManager.instance.anyActiveModuleExists = false;
        this.moduleTimer.value = 100f;
        this.moduleTimer.gameObject.SetActive(false);
    }

    public void Reset() {
        this.movement.Reset();
        this.arch.SetActive(false);
        if (this.coroutine != null)
            StopCoroutine(this.coroutine);
        this.ResetModule();
    }

    public void Revive(float duration) {
        StartCoroutine(Reviving(duration));
    }

    IEnumerator Reviving(float duration) {
        this.boxCollider.enabled = false;
        while (duration >= 0f) {
            duration -= 0.25f;
            this.meshRenderer.enabled = !this.meshRenderer.enabled;
            yield return new WaitForSeconds(0.25f);
        }
        this.boxCollider.enabled = true;
        this.meshRenderer.enabled = true;
    }
}
