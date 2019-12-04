using UnityEngine;

[System.Serializable]
public class ObjectPoolItem {
    public GameObject objectToPool;
    public int amountToPool;

    //If true the pool grows in size when needed
    public bool elasticPool;
}