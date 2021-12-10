using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public Vector3 offset;

    private Vector3 offsetBase;
    
    private bool stop = false;

    private void Start() {
        this.offsetBase = this.offset;
    }
    
    private void LateUpdate() {
        this.transform.position = this.target.position + this.offset;
    }

    public void ShakeCamera(float duration, float shakeAmount = 0.2f) {
        this.stop = false;
        StartCoroutine(Shake(duration, shakeAmount));
    }

    public void Reset() {
        this.stop = true;
    }

    IEnumerator Shake(float duration, float shakeAmount) {
        while (duration >= 0f) {
            if (this.stop) {
                break;
            }
            this.offset = this.offsetBase + Random.insideUnitSphere * shakeAmount;
            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.offset = this.offsetBase;
    }
}
