using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 6f;
    [HideInInspector]
    public float baseSpeed;

    private CharacterController controller; 
    private Vector3 movement = Vector3.zero;
    private Quaternion lookRotation;
    private Vector3 rotation;

    private void Start() {
        this.controller = this.GetComponent<CharacterController>();
        this.baseSpeed = this.speed;
    }

    private void Update() {
        this.movement.x = SimpleInput.GetAxis("Horizontal");
        this.movement.z = SimpleInput.GetAxis("Vertical");
        this.controller.Move(this.movement * Time.deltaTime * this.speed);

        if (this.movement.magnitude > 0f) {
            this.lookRotation = Quaternion.LookRotation(this.movement);
            this.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime * 8f).eulerAngles;
            this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    public void Reset() {
        this.transform.rotation = Quaternion.identity;
        this.controller.enabled = false;
        this.controller.transform.position = Vector3.zero;
        this.controller.enabled = true;
    }
}
