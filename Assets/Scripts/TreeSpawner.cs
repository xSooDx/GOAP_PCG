using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public float gridSize;
    public int cellCount;
    public GameObject TreePrefab;

    public void GenTrees(LevelGenNode node)
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        float halfGridSize = gridSize / 2f;
        float cellSize = gridSize / cellCount;
        float halfCellSize = cellSize / 2f;
        Vector3 origin = transform.position - new Vector3(halfGridSize, 0, halfGridSize);
        for (int i = 0; i < cellCount; i++)
        {
            for (int j = 0; j < cellCount; j++)
            {
                Vector2 offset = Random.insideUnitCircle * halfCellSize;
                Vector3 position = origin + new Vector3((cellSize * i) + offset.x, 0, (cellSize * j) + offset.y);
                bool skip = false;
                foreach (LevelGenNode n in node.ForEachGenNode())
                {
                    if((position - n.center).sqrMagnitude < n.roomSize* n.roomSize)
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;
                
                Transform t = Instantiate(TreePrefab).transform;
                t.position = position;
                t.parent = transform;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, 0, gridSize));
    }
}
