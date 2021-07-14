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

    private void ConnectionsInit()
    {
        connections.Clear();

        connections.Add(originBlock, originBlock.GetComponentsInChildren<Transform>());
        connections.Add(terminationBlock, terminationBlock.GetComponentsInChildren<Transform>());
        
        foreach (GameObject block in levelblocks)
            connections.Add(block, block.GetComponentsInChildren<Transform>());
    }

    private Vector3 CalculatePosition(Vector3 lastBlockPosition, Vector3 thisBlockConnectorPosition)
    {
        Debug.Log("Last Block: " + lastBlockPosition + " This Connector: " + thisBlockConnectorPosition);
        return thisBlockConnectorPosition - lastBlockPosition;
    }

    private Quaternion CalculateRotation(Vector3 lastBlockEuler, Vector3 thisConnectorEuler)
    {
        return Quaternion.Euler(new Vector3(270f, lastBlockEuler.y + thisConnectorEuler.y, 0f));
    }

    private void GenerateBlocks()
    {
        short totalBlocks = 0;
        int thisBlockID = 0;

        GameObject lastBlock = Instantiate(originBlock, origin, Quaternion.Euler(originRotation), null);

        for (int i = 0; i < Mathf.RoundToInt(Random.Range(minimumGenerationSteps, maximumGenerationSteps)); i++)
        {
            if (totalBlocks > maximumBlocksAllowed) return;

            Transform[] objectConnectors = connections[levelblocks[thisBlockID]];
            Debug.Log(objectConnectors[thisBlockID]);

            foreach(Transform objectConnector in objectConnectors)
            {
                objectConnectors = connections[levelblocks[thisBlockID]];
                Debug.Log(objectConnectors[thisBlockID]);

                thisBlockID = Mathf.RoundToInt(Random.Range(0f, levelblocks.Length - 1));

                GameObject thisBlock = levelblocks[Mathf.RoundToInt(Random.Range(0, levelblocks.Length))];

                Vector3 thisBlockPosition = CalculatePosition(lastBlock.transform.position, objectConnector.localPosition);
                Quaternion thisBlockRotation = CalculateRotation(lastBlock.transform.rotation.eulerAngles, objectConnector.rotation.eulerAngles);

                lastBlock = Instantiate(thisBlock, thisBlockPosition, thisBlockRotation);
            }
        }
    }

    private void Awake()
    {
        Random.InitState(generatorSeed);

        ConnectionsInit();

        GenerateBlocks();
        
    }
}
