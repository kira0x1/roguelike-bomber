using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private List<ObjectPoolItem> itemsToPool;

    private void Awake () {
        if (instance != null) {
            Destroy (gameObject);
        } else {
            instance = this;
        }
    }

    private void Start () {
        pooledObjects = new List<GameObject> ();

        foreach (ObjectPoolItem item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                SpawnPoolObject (item.objectToPool);
            }
        }
    }

    public GameObject GetPooledObject (string tag) {
        foreach (var poolObj in pooledObjects) {
            if (!poolObj.activeInHierarchy && poolObj.tag == tag) {
                return poolObj;
            }
        }

        //If the pool is elastic then instantiate a new object
        foreach (var item in itemsToPool) {
            if (item.elasticPool && item.objectToPool.tag == tag) {
                return SpawnPoolObject (item.objectToPool);
            }
        }

        //if not elastic then return null
        return null;
    }

    private GameObject SpawnPoolObject (GameObject objectToPool) {
        var obj = Instantiate (objectToPool);
        obj.transform.SetParent (transform);
        obj.SetActive (false);
        pooledObjects.Add (obj);
        return obj;
    }
}