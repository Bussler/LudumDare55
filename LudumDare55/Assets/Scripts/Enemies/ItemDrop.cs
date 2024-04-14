using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage the drop of items from enemies.
/// </summary>
public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _itemPrefabs = new List<GameObject>();

    [SerializeField]
    private float _dropChance = 0.5f;

    /// <summary>
    /// Drop an item with _dropChance probability.
    /// </summary>
    public void Drop()
    {
        if (Random.Range(0f, 1f) <= _dropChance)
        {
            GameObject itemPrefab = _itemPrefabs[Random.Range(0, _itemPrefabs.Count)];

            spawnPassiveItem(itemPrefab);
        }
    }

    protected void spawnPassiveItem(GameObject itemPrefab)
    {
        ObjectPoolManager.Instance.SpawnObject(itemPrefab, this.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Item);
    }
}
