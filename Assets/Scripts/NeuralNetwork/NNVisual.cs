using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNVisual : MonoBehaviour
{

    GameObject[][] nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new GameObject[GeneticAlgorithm.NETWORK_SHAPE.Length][];
        for (int i = 0; i < GeneticAlgorithm.NETWORK_SHAPE.Length; i++) {
            nodes[i] = new GameObject[GeneticAlgorithm.NETWORK_SHAPE[i]];
            for (int j = 0; j < GeneticAlgorithm.NETWORK_SHAPE[i]; j++) {
                float yOffset = (GeneticAlgorithm.NETWORK_SHAPE[i] / 2f);
                Vector3 nodePos = new Vector3(transform.position.x + (i * 1.5f), transform.position.y - (j * 1f) + yOffset);
                nodes[i][j] = Instantiate(Resources.Load("node"), nodePos, Quaternion.identity) as GameObject;
                nodes[i][j].transform.SetParent(transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int layer = 0; layer < GeneticAlgorithm.NETWORK_SHAPE.Length; layer++) {
            for (int node = 0; node < GeneticAlgorithm.NETWORK_SHAPE[layer]; node++) {
                bool active = NeuralNetwork.nodeActivations[layer][node];
                nodes[layer][node].GetComponent<SpriteRenderer>().color = active ? Color.white : Color.gray;
            }
        }

        for (int layer = 0; layer < GeneticAlgorithm.NETWORK_SHAPE.Length - 1; layer++) {
            for (int ni = 0; ni < GeneticAlgorithm.NETWORK_SHAPE[layer]; ni++) {
                for (int nj = 0; nj < GeneticAlgorithm.NETWORK_SHAPE[layer + 1]; nj++) {
                    Vector3 start = nodes[layer][ni].transform.position;
                    Vector3 end = nodes[layer + 1][nj].transform.position;
                    bool active = NeuralNetwork.weightActivations[layer][ni][nj];
                    Debug.DrawLine(start, end, active ? Color.white : Color.gray);
                }
            }
        }
    }
}
