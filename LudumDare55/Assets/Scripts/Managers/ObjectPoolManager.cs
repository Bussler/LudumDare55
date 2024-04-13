using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Class to manage object pooling: Instead of instantiating and destroying objects, we can just disable and enable them from a list.
/// Implements the Singleton pattern.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    public List<PooledObject> ObjectPools = new List<PooledObject>();

    public GameObject _objectPoolHierarchyHolder;
    public GameObject _enemyPoolHolder;
    public GameObject _enemyBulletPoolHolder;
    public GameObject _bulletPoolHolder;

    public enum PoolType
    {
        Enemy,
        EnemyBullet,
        Bullet,
        None
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        setupHierarchyHolders();
    }

    /// <summary>
    /// Creates the hierarchy holders for the object pools as empty game objects in unity.
    /// </summary>
    private void setupHierarchyHolders()
    {
        _objectPoolHierarchyHolder = new GameObject("Pooled Objects");

        _enemyPoolHolder = new GameObject("Enemy Pool");
        _enemyPoolHolder.transform.SetParent(_objectPoolHierarchyHolder.transform);

        _enemyBulletPoolHolder = new GameObject("Enemy Bullet Pool");
        _enemyBulletPoolHolder.transform.SetParent(_objectPoolHierarchyHolder.transform);

        _bulletPoolHolder = new GameObject("Bullet Pool");
        _bulletPoolHolder.transform.SetParent(_objectPoolHierarchyHolder.transform);
    }

    /// <summary>
    /// Spawn an object from the pool, or instantiate a new one if none are available.
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObject currentPool = ObjectPools.Find(pool => pool.name == objectToSpawn.name);

        // instantiate pool if it doesn't exist
        if (currentPool == null)
        {
            currentPool = new PooledObject() { name = objectToSpawn.name };
            ObjectPools.Add(currentPool);
        }

        GameObject currentObject = currentPool.InactiveObjects.FirstOrDefault();

        // if there are no inactive objects, instantiate a new one
        if (currentObject == null)
        {
            currentObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            GameObject parentObject = getParentObject(poolType);
            if (parentObject != null)
            {
                currentObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            currentObject.transform.position = spawnPosition;
            currentObject.transform.rotation = spawnRotation;
            currentPool.InactiveObjects.Remove(currentObject);
            currentObject.SetActive(true);
        }

        return currentObject;
    }

    /// <summary>
    /// Spawn an object from the pool, or instantiate a new one if none are available. Spawn on parent object
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="parentTransform"></param>
    /// <returns></returns>
    public GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObject currentPool = ObjectPools.Find(pool => pool.name == objectToSpawn.name);

        // instantiate pool if it doesn't exist
        if (currentPool == null)
        {
            currentPool = new PooledObject() { name = objectToSpawn.name };
            ObjectPools.Add(currentPool);
        }

        GameObject currentObject = currentPool.InactiveObjects.FirstOrDefault();

        // if there are no inactive objects, instantiate a new one
        if (currentObject == null)
        {
            currentObject = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            currentPool.InactiveObjects.Remove(currentObject);
            currentObject.SetActive(true);
        }

        return currentObject;
    }

    /// <summary>
    /// Set the object inactive and add it to the pool according to its name.
    /// </summary>
    /// <param name="objectToDespawn"></param>
    public void DespawnObject(GameObject objectToDespawn)
    {
        if (objectToDespawn.name.Length <= 7)
        {
            Debug.Log("Object was not spawned with ObjectPoolManager " + objectToDespawn.name);
            return;
        }

        string goName = objectToDespawn.name.Substring(0, objectToDespawn.name.Length - 7); // remove "(Clone)" from the name
        PooledObject currentPool = ObjectPools.Find(pool => pool.name == goName);

        if (currentPool == null)
        {
            Debug.Log("Object pool not found for " + objectToDespawn.name);
            return;
        }

        currentPool.InactiveObjects.Add(objectToDespawn);
        objectToDespawn.SetActive(false);
    }

    private GameObject getParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Enemy:
                return _enemyPoolHolder;
            case PoolType.EnemyBullet:
                return _enemyBulletPoolHolder;
            case PoolType.Bullet:
                return _bulletPoolHolder;
            default:
                return null;
        }
    }
}

/// <summary>
/// Pool of objects of the same type.
/// Expects tht all objects have the same name.
/// </summary>
public class PooledObject
{
    public string name;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
