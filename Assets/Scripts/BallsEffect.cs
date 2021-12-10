using UnityEngine;

public class BallsEffect : MonoBehaviour {
    public float angularSpeed = 3f;
    public float circleRad = 3f;

    [Header("Balls")]
    public Transform ball1;
    public Transform ball2;
 
    private float currentAngle;
    private Vector3 offset1 = Vector3.zero;
    private Vector3 offset2 = Vector3.zero;
 
    void Update () {
        this.currentAngle += angularSpeed * Time.deltaTime;
        this.offset1.x = Mathf.Sin(this.currentAngle);
        this.offset1.z = Mathf.Cos(this.currentAngle);
        this.offset1 *= circleRad;
        this.ball1.position = this.transform.position + offset1;

        this.offset2.x = Mathf.Sin(this.currentAngle);
        this.offset2.z = Mathf.Cos(this.currentAngle);
        this.offset2 *= circleRad * -1;
        this.ball2.position = this.transform.position + offset2;
    }
}
