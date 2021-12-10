using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int poolAmount;

    private void Start() {
        this.pooledObjects = new List<GameObject>();
        for(int i = 0; i < this.poolAmount; i++) {
            GameObject obj = (GameObject)Instantiate(this.objectToPool);
            obj.SetActive(false);
            this.pooledObjects.Add(obj);
        }
    }

    public GameObject GetObjectFromPool() {
        for (int i = 0; i < this.poolAmount; i++) {
            if(!this.pooledObjects[i].activeInHierarchy) {
                return this.pooledObjects[i];
            }
        }
        return null;
    }

    public void RemoveAllObjects() {
        for (int i = 0; i < this.poolAmount; i++) {
            if(this.pooledObjects[i].activeInHierarchy) {
                this.pooledObjects[i].SetActive(false);
            }
        }
    }
}
