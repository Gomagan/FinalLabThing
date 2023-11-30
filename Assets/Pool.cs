using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [Serializable]
    private struct PooledObject
    {
        public BulletScript prefab;
        public int numToSpawn;
    }

    [SerializeField] private PooledObject[] pools;

    private static readonly Dictionary<string, Queue<BulletScript>> pooledObjects = new Dictionary<string, Queue<BulletScript>>();

    private void Awake()
    {
        pooledObjects.Clear();

        foreach (PooledObject pool in pools) 
        {
            string name = pool.prefab.name;
            Transform parent = new GameObject(name).transform;
            parent.SetParent(transform);
            Queue<BulletScript> objectsToSpawn = new();

            for(int i = 0; i < pool.numToSpawn; i++)
            {
                BulletScript rb = Instantiate(pool.prefab, parent);
                rb.gameObject.SetActive(false);
                objectsToSpawn.Enqueue(rb);
            }
            pooledObjects.Add(name, objectsToSpawn);
        }
    }

    public static BulletScript Shoot(string name, Vector3 location, Quaternion rotation)
    {
        if(!pooledObjects.ContainsKey(name))
        {
            Debug.LogAssertion("Does not contain key: " + name);
            return null; 
        }

        BulletScript rb = pooledObjects[name].Dequeue();

        rb.transform.SetPositionAndRotation(location, rotation);
        rb.gameObject.SetActive(true);

        pooledObjects[name].Enqueue(rb);
        return rb;


    }
}
