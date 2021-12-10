using UnityEngine;

public enum NotificationType {
    DoubleKill,
    TripleKill,
    QuadraKill,
    PentaKill,
    MasterKill,
}

public class NotificationItem : MonoBehaviour {
    public NotificationType type;
    public float duration = 3f;
    public AudioClip clip;

    public void Show() {
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}
