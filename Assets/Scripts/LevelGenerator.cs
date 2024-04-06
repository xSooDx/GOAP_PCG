using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    public LevelGeneratorSettings generatorSettings;

#if UNITY_EDITOR
    [InspectorButton("GenerateLevel", ButtonWidth = 400)]
    public bool GenerateLevelButton;

    [InspectorButton("CallGeneratorListeners", ButtonWidth = 400)]
    public bool CallGeneratorListenersButton;
#endif

    [Header("Random Seed")]
    public bool CreateRandomSeed;
    public int currentSeed;

    [Header("On Generated Events")]
    public UnityEvent<LevelGenNode> OnLevelGraphGenerated;

    LevelGenNode startNode;

    public void GenerateLevel()
    {
        if (CreateRandomSeed)
            currentSeed = Random.Range(int.MinValue, int.MaxValue);

        GenerateLevelWithSeed(currentSeed);
    }
    public void GenerateLevelWithSeed(int seed)
    {
        Random.InitState(seed);
        startNode = new LevelGenNode();
        startNode.center = transform.position;
        startNode.roomSize = Random.Range(generatorSettings.roomMinSize, generatorSettings.roomMaxSize);

        int length = Random.Range(generatorSettings.levelMinLength, generatorSettings.levelMaxLength);
        LevelGenNode currentNode = startNode;
        for (int i = 1; i < length; i++)
        {
            LevelGenNode newNode = CreateNewLevelGenNode(currentNode);
            newNode.roomSize = GenerateRoomSize();

            float angle = GenerateRoomAngleOffset();
            newNode.angle = angle;
            newNode.center = GenerateNewNodeLocation(currentNode, newNode.roomSize, angle);


            //Debug.DrawLine(currentNode.center, newNode.center, Color.green, 60);
            currentNode = newNode;
        }

        foreach (LevelGenNode n in startNode.ForEachGenNode())
        {
            if (n.childNodes.Count > 0 && Mathf.Abs(n.childNodes[0].angle) > generatorSettings.minAngle && Random.Range(0f, 1f) < generatorSettings.branchingFactor)
            {
                LevelGenNode newNode = CreateNewLevelGenNode(n);
                newNode.roomSize = GenerateBranchRoomSize();
                newNode.angle = -n.childNodes[0].angle;
                newNode.center = GenerateBranchLocation(n, newNode.roomSize, newNode.angle);
                newNode.isBranch = true;
            }
        }
    }

    public void CallGeneratorListeners()
    {
        OnLevelGraphGenerated?.Invoke(startNode);
    }

    private float GenerateRoomSize() => Random.Range(generatorSettings.roomMinSize, generatorSettings.roomMaxSize);
    private float GenerateBranchRoomSize() => Random.Range(generatorSettings.branchRoomMinSize, generatorSettings.branchRoomMaxSize);
    private float GenerateRoomAngleOffset() =>(Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(generatorSettings.minAngle, generatorSettings.maxAngle) ;
    private Vector3 GenerateNewNodeLocation(LevelGenNode parentNode, float currentRoomSize, float angle)
    {
        Vector3 direction = Quaternion.AngleAxis(generatorSettings.baseAngle +  angle, Vector3.up) * Vector3.forward;
        return parentNode.center + (direction * (currentRoomSize + parentNode.roomSize + Random.Range(generatorSettings.connectorMinLength, generatorSettings.connectorMaxLength)));
    }

    private Vector3 GenerateBranchLocation(LevelGenNode parentNode, float currentRoomSize, float angle)
    {
        Vector3 direction = Quaternion.AngleAxis(generatorSettings.baseAngle + angle, Vector3.up) * Vector3.forward;
        return parentNode.center + (direction * (currentRoomSize + parentNode.roomSize + Random.Range(generatorSettings.branchMinLength, generatorSettings.branchMaxLength)));
    }

    private static LevelGenNode CreateNewLevelGenNode(LevelGenNode parentNode)
    {
        LevelGenNode newNode = new LevelGenNode();
        newNode.parentNode = parentNode;
        parentNode.childNodes.Add(newNode);
        return newNode;
    }

    

#if UNITY_EDITOR
    private void OnValidate()
    {
        GenerateLevelWithSeed(currentSeed);
    }

    private void OnDrawGizmosSelected()
    {
        if (startNode == null) return;

        Queue<LevelGenNode> que = new Queue<LevelGenNode>();
        que.Enqueue(startNode);
        while (que.Count > 0)
        {
            LevelGenNode node = que.Dequeue();
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(node.center, node.roomSize);


            if (node.parentNode != null)
            {
                if (node.isBranch)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;
                Gizmos.DrawLine(node.center, node.parentNode.center);
            }

            foreach (LevelGenNode cn in node.childNodes)
                que.Enqueue(cn);
        }
    }

#endif
}



public class LevelGenNode
{
    public LevelGenNode parentNode;
    public List<LevelGenNode> childNodes = new List<LevelGenNode>();

    public Vector3 center;
    public float roomSize;
    public float angle;
    public bool isBranch;

    public IEnumerable<LevelGenNode> ForEachGenNode()
    {
        Queue<LevelGenNode> que = new Queue<LevelGenNode>();
        que.Enqueue(this);

        while (que.Count > 0)
        {
            LevelGenNode node = que.Dequeue();
            foreach (LevelGenNode cn in node.childNodes)
                que.Enqueue(cn);

            yield return node;
        }
    }
}

