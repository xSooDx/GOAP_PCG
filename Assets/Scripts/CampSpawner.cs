using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSpawner : MonoBehaviour
{
    public float size = 100;
    public GameObject CampPrefab;
    public GameObject SoldierPrefab;
    public GameObject WorkerPrefab;
    public GameObject axePrefab;
    public GameObject SwordPrefab;
    public GameObject BowPrefab;
    public float EnemyDensity = 0.5f;
    public int MinEnemyCount = 6;
    public int AdditionalEnemies = 6;

    // Start is called before the first frame update

    public void SpawnCamps(LevelGenNode node)
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int EnemyCount = MinEnemyCount + Mathf.RoundToInt(AdditionalEnemies * EnemyDensity);
        int BranchEnemeyCount = MinEnemyCount / 2;

        int i = 0;
        foreach (LevelGenNode n in node.ForEachGenNode())
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            if (Mathf.Abs(n.center.x) > size / 2 || Mathf.Abs(n.center.y) > size / 2)
            {
                continue;
            }
            Transform t = Instantiate(CampPrefab, transform).transform;
            t.position = n.center;
            if (n.isBranch)
            {
                SingleSpawnCamp(BranchEnemeyCount, n);
            }
            else
            {
                SingleSpawnCamp(EnemyCount, n);
            }
        }
    }

    private void SingleSpawnCamp(int EnemyCount, LevelGenNode n)
    {
        for (int c = 0; c < EnemyCount; c++)
        {
            switch (c % 3)
            {
                case 0:
                    SpawnItem(WorkerPrefab, n);
                    SpawnItem(axePrefab, n);
                    break;
                case 1:
                    SpawnItem(WorkerPrefab, n);
                    break;
                case 2:
                    SpawnItem(SoldierPrefab, n);
                    if (c % 6 == 5)
                        SpawnItem(Random.Range(0, 2) == 0 ? BowPrefab : SwordPrefab, n);
                    break;
            }

        }
    }

    Transform SpawnItem(GameObject prefab, LevelGenNode n)
    {
        Transform t = Instantiate(prefab, transform).transform;
        Vector3 offset = Random.insideUnitSphere;
        offset.y = 0;
        offset = offset.normalized * n.roomSize / 2f;
        t.position = n.center + offset;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, 0, size));
    }
}
