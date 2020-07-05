using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    public GameObject[] prefab;
    public GameObject NPCContainer;

    [Header("Prefab array range.")]
    public int minRange = 0;
    public int maxRange = 5;

    [Header("Area Range.")]
    public float maxValue = 70.0f;
    public float minValue = -70.0f;

    [Range(10, 300)]
    public int DesiredFishAmount = 150;


    void Start()
    {
        InitSpawner();
    }

    public void InitSpawner()
    {
        int npcIndex;
        for (int i = 0; i < DesiredFishAmount; i++)
        {
            npcIndex = Random.Range(minRange, prefab.Length);
            SpawnNPC(prefab[npcIndex]);
        }
    }

    public void SpawnNPC(GameObject prefabNPC)
    {
        GameObject go = Instantiate(
                prefabNPC,
                new Vector3(transform.position.x + Random.Range(minValue, maxValue), transform.position.y,
                transform.position.z + Random.Range(minValue, maxValue)),
                Quaternion.Euler(Vector3.up * Random.Range(0, 360))
                );
        go.transform.SetParent(NPCContainer.transform);
    }
}
