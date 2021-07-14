using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeerator : MonoBehaviour
{
    /*============Serialized Variables============*/

    [Header("Blocks")]
    [SerializeField] private GameObject originBlock;
    [SerializeField] private GameObject[] levelblocks;
    [SerializeField] private GameObject terminationBlock;

    [Header("Parameters")]
    [SerializeField] private short minimumGenerationSteps;
    [SerializeField] private short maximumGenerationSteps;
    [SerializeField] private short maximumBlocksAllowed;
    [SerializeField] private int generatorSeed;

    [Header("Settings")]
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 originRotation;
    [SerializeField] private bool autoGenerate;

    /*============Global Variables============*/

    private Dictionary<GameObject, Transform[]> connections = new Dictionary<GameObject, Transform[]>();

    /*============Functions & Methods============*/

    private void CalculateOffsets()
    {
        foreach (GameObject block in levelblocks)
            connections.Add(block, block.gameObject.GetComponentsInChildren<Transform>());
    }

    private void GenerateBlocks()
    {
        short totalBlocks = 0;

        GameObject initBlock = Instantiate(originBlock, origin, Quaternion.Euler(originRotation), null);

        for (int i = 0; i < Mathf.RoundToInt(Random.Range(minimumGenerationSteps, maximumGenerationSteps)); i++)
        {
            if (totalBlocks > maximumBlocksAllowed) return;

        }
    }

    private void Awake()
    {
        Random.InitState(generatorSeed);
        CalculateOffsets();
        GenerateBlocks();
        
    }
}
