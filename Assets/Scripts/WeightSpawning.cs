using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSpawning : MonoBehaviour
{
    [Header("Weight Prefabs")]
    [SerializeField] private GameObject lowWeight;
    [SerializeField] private GameObject mediumWeight;
    [SerializeField] private GameObject heavyWeight;

    [Header("Weight Spawn Point")]
    [SerializeField] private Transform weightSpawnPoint;

    private GameObject currentWeight;

    public void SpawnLowWeight()
    {
        if (currentWeight != null)
        {
            Destroy(currentWeight);
        }
        currentWeight = Instantiate(lowWeight, weightSpawnPoint.position, weightSpawnPoint.rotation);
    }
}
