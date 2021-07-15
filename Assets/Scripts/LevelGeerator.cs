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
        return lastBlockPosition - thisBlockConnectorPosition;
        //return thisBlockConnectorPosition;
    }

    private Quaternion CalculateRotation(Vector3 lastBlockEuler, Vector3 thisConnectorEuler)
    {
        return Quaternion.Euler(new Vector3(lastBlockEuler.x, lastBlockEuler.y + thisConnectorEuler.y, thisConnectorEuler.z + 180f));
    }

    private void GenerateBlocks()
    {
        short totalBlocks = 0; //Create a variable to count the total blocks generated, currently zero
        int thisBlockIndex = 0; //Create a variable to store the index for the current block, currently zero

        GameObject lastBlock = Instantiate(originBlock, origin, Quaternion.Euler(originRotation), null); //Generate origin block

        Transform[] objectConnectors = connections[levelblocks[thisBlockIndex]]; //Get the Transforms for the object connectors of the origin object

        for (int i = 0; i < Mathf.RoundToInt(Random.Range(minimumGenerationSteps, maximumGenerationSteps)); i++) //For each generation step
        {
            if (totalBlocks > maximumBlocksAllowed) break; //If there are too many blocks already generated, stop generating

            foreach(Transform objectConnector in objectConnectors) //For each connector's Transform on the last block...
            {
                thisBlockIndex = Mathf.RoundToInt(Random.Range(0f, levelblocks.Length - 1)); //Choose a block index from the block array
                GameObject thisBlock = levelblocks[Mathf.RoundToInt(Random.Range(0, levelblocks.Length))]; //Get the block specified by the block index

                //Calculate the position and orientation of where the generated block should be placed...
                Vector3 thisBlockPosition = CalculatePosition(lastBlock.transform.position, objectConnector.localPosition);
                Quaternion thisBlockRotation = CalculateRotation(lastBlock.transform.rotation.eulerAngles, objectConnector.rotation.eulerAngles);


                lastBlock = Instantiate(thisBlock, thisBlockPosition, thisBlockRotation); //Generate the block
                lastBlock.name = "Block: " + totalBlocks.ToString(); //Name the block for easier debugging
                totalBlocks++; //Add one to the totalBlocks count
            }

            objectConnectors = connections[levelblocks[thisBlockIndex]];
        }
    }

    private void Awake()
    {
        Random.InitState(generatorSeed);

        ConnectionsInit();
        GenerateBlocks();
        
    }
}
