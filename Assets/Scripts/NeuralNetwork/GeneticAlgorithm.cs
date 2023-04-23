using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{
    private const float MUTATION_VOLATILITY_MAX = 0.008f;
    private const float MUTATION_VOLATILITY_MIN = 0.001f;
    public const int POPULATION = 100;
    public static readonly int[] NETWORK_SHAPE = {8,16,16,8,5,1};

    public NeuralNetwork[] networks;

    // Generate new networks upon construction
    public GeneticAlgorithm() {
        networks = new NeuralNetwork[POPULATION];
        for (int i = 0; i < POPULATION; i++) {
            networks[i] = new NeuralNetwork(NETWORK_SHAPE);
        }
    }

    // send all the current networks to the next generation, considering fitness
    public void nextGeneration() {
        int n = networks.Length;

        // Determine fitness distribution
        float fitnessSum = calculateTotalFitness();

        // Create new array of networks
        NeuralNetwork[] res = new NeuralNetwork[n];

        // keep best network in next generation
        NeuralNetwork bestNetwork = getElite();
        res[0] = bestNetwork;
        res[0].fitness = 0f;

        // Generate next generation
        for (int i = 1; i < n; i++) {
            NeuralNetwork[] parents = selectParents(fitnessSum);
            NeuralNetwork offspring = breed(parents[0], parents[1]);
            res[i] = offspring;
        }

        // reset the carried over network's fitness
        networks = res;
    }

    // return the strongest neural network
    private NeuralNetwork getElite() {
        float max = networks[0].fitness;
        NeuralNetwork eliteNN = networks[0];
        for (int i = 1; i < networks.Length; i++) {
            if (networks[i].fitness > max) {
                max = networks[i].fitness;
                eliteNN = networks[i];
            }
        }
        return eliteNN;
    }

    private float calculateTotalFitness() {
        float fitnessSum = 0f;
        for (int i = 0; i < networks.Length; i++) {
            networks[i].powFitness(2);
            fitnessSum += networks[i].fitness;
        }
        return fitnessSum;
    }

    private NeuralNetwork[] selectParents(float fitnessSum) {
        // select two parents ------
        NeuralNetwork p1 = null;
        int p1Index = 0;
        NeuralNetwork p2 = null;

        // first parent
        float randDistributionPoint = Random.Range(0f, fitnessSum);
        float curr = 0f;
        for (int j = 0; j < networks.Length; j++) {
            curr += networks[j].fitness;
            if (randDistributionPoint < curr + 0.001f) {
                p1 = networks[j];
                p1Index = j;
                break;
            }
        }

        if (p1 == null) p1 = networks[0];

        // second parent (cannot be the same as p1)
        randDistributionPoint = Random.Range(0f, fitnessSum - p1.fitness);
        curr = 0f;
        for (int j = 0; j < networks.Length; j++) {
            if (j == p1Index) continue;
            curr += networks[j].fitness;
            if (randDistributionPoint < curr + 0.001f) {
                p2 = networks[j];
                break;
            }
        }

        if (p2 == null) p2 = networks[0];
        return new NeuralNetwork[] {p1, p2};
    }

    // combine two networks using crossover and mutation
    private static NeuralNetwork breed(NeuralNetwork nn1, NeuralNetwork nn2) {
        NeuralNetwork child = new NeuralNetwork(NETWORK_SHAPE);

        float volatility = Random.Range(MUTATION_VOLATILITY_MIN, MUTATION_VOLATILITY_MAX);

        for (int i = 0; i < nn1.weights.Length; i++) {
            Matrix weights = Matrix.randomJoin(nn1.weights[i], nn2.weights[i]);
            weights = Matrix.mutate(weights, volatility);
            child.weights[i] = weights;

            Matrix biases = Matrix.randomJoin(nn1.biases[i], nn2.biases[i]);
            biases = Matrix.mutate(biases, volatility);
            child.biases[i] = biases;
        }

        return child;
    }
}
