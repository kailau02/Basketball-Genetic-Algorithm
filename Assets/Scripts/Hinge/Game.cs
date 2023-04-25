using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static GeneticAlgorithm geneticAlg;
    private bool began = false;
    public TargetControl targetControl;
    GameObject ball;
    public GameObject target;
    float minDist = float.MaxValue;

    private const int MAX_TRIALS = 100;
    public const int trialsPerGeneration = 100;
    public static int currNetworkIndex;
    int currTrial;
    int currGeneration = 0;
    float timer = 0f;

    int greenCounter = 0;

    
    [SerializeField]
    UIControl ui;
    
    // Start is called before the first frame update
    void Start()
    {
        geneticAlg = new GeneticAlgorithm();
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10f) {
            minDist = 100f;
            StartCoroutine(waitForNextRound(false));
        }

        minDist = Mathf.Min(minDist, Vector2.Distance(ball.transform.position, target.transform.position));
        if (ball.transform.position.y < -15f) {
            StartCoroutine(waitForNextRound(false));
        }
    }

    void SpawnBall() {
        Destroy(ball);
        ball = Instantiate(Resources.Load("Ball"), new Vector3(-15, 0, 0), Quaternion.identity) as GameObject;

    }

    public IEnumerator waitForNextRound(bool won) {

        if (won) {
            minDist = -0.15f;
            greenCounter++;
        }

        if (!began) {
            began = true;            
            yield return new WaitForSeconds(won ? 1f : 0f);
            EndRound();
            began = false;
        }
    }

    void EndRound() {
        timer = 0f;
        // next network
        geneticAlg.networks[currNetworkIndex].fitness += getFitness();
        currNetworkIndex++;
        targetControl.resetColor();

        // next target position
        if (currNetworkIndex >= GeneticAlgorithm.POPULATION) {
            targetControl.randomizePosition();
            currNetworkIndex = 0;
            currTrial++;
        }

        // next generation
        if (currTrial >= trialsPerGeneration) {
            float accuracy = (((float)greenCounter) / (GeneticAlgorithm.POPULATION * trialsPerGeneration) * 100f);
            Debug.Log(accuracy.ToString("F2") + "% accuracy");
            saveElite();
            greenCounter = 0;
            geneticAlg.nextGeneration();
            currTrial = 0;
            currGeneration++;
        }

        // start round
        SpawnBall();
        minDist = float.MaxValue;
        ui.SetGenerationText(currGeneration);
        ui.SetTrialText(currTrial, trialsPerGeneration);
        ui.SetNetworkText(currNetworkIndex, GeneticAlgorithm.POPULATION);
        GameObject.Find("hinge").GetComponent<HingeControl>().resetHinge();
    }

    void saveElite() {
        
        float maxFit = 0;
        int bestIndex = 0;
        for (int i = 0; i < geneticAlg.networks.Length; i++) {
            if (geneticAlg.networks[i].fitness > maxFit) {
                maxFit = geneticAlg.networks[i].fitness;
                bestIndex = i;
            }
        }

        float fitness = geneticAlg.networks[bestIndex].fitness;
        string fitStr = fitness.ToString("F2");

        string path = fitStr + "f-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        geneticAlg.networks[bestIndex].saveNN(path);
    }

    public float getFitness() {
        return 1 / (minDist + 0.5f);
    }
}
