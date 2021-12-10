using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour {
    public AudioSource audioSource;
    public List<NotificationItem> notificationItems;
    
    private Queue<NotificationItem> notificationQueue = new Queue<NotificationItem>();
    private NotificationItem currentNotificationItem = null;
    private Coroutine coroutine;

    private void Update() {
        if (this.notificationQueue.Count > 0 && this.currentNotificationItem == null) {
            this.coroutine = StartCoroutine(ShowItem());
        }
    }
    
    public void AddNotificationItem(NotificationType type) {
        this.notificationQueue.Enqueue(this.notificationItems.Find(notification => notification.type == type));
    }

    IEnumerator ShowItem() {
        this.currentNotificationItem = this.notificationQueue.Dequeue();
        this.currentNotificationItem.Show();
        if(this.currentNotificationItem.clip) {
            this.audioSource.PlayOneShot(this.currentNotificationItem.clip);
        }
        yield return new WaitForSeconds(this.currentNotificationItem.duration);
        this.currentNotificationItem.Hide();
        this.currentNotificationItem = null;
    }

    public void Reset() {
        if (this.coroutine != null)
            StopCoroutine(this.coroutine);
        if (this.currentNotificationItem) {
            this.currentNotificationItem.Hide();
            this.currentNotificationItem = null;
        }
        this.notificationQueue.Clear();
    }
}
