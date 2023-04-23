using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public static int ffLayer = 0; // Used by Matrix.dot for live diagram
    public static bool[][] nodeActivations;
    public static bool[][][] weightActivations;
    public Matrix[] weights;
    public Matrix[] biases;

    public float fitness = 0;

    public NeuralNetwork() {}

    public NeuralNetwork(int[] layers) {
        createNN(layers);
    }

    public void createNN(int[] layers) {
        int nLayers = layers.Length;
        nodeActivations = new bool[nLayers][];
        weightActivations = new bool[nLayers - 1][][];

        for (int i = 0; i < nLayers; i++) {
            nodeActivations[i] = new bool[layers[i]];
        }

        // setup weights
        weights = new Matrix[nLayers - 1];
        biases = new Matrix[nLayers - 1];
        for (int i = 0; i < weights.Length; i++) {
            // setup weights
            weights[i] = new Matrix(layers[i], layers[i + 1]);
            weights[i].randomize();

            weightActivations[i] = new bool[layers[i]][];
            for (int j = 0; j < layers[i]; j++) {
                weightActivations[i][j] = new bool[layers[i + 1]];
            }

            // setup biases
            biases[i] = new Matrix(1, layers[i + 1]);
            biases[i].randomize();
        }
    }

    public void loadNN() {

    }

    public float[] feedForward(float[] input) {
        Matrix X = new Matrix(input);

        // first layer activation display state
        ffLayer = 0;
        float[] xArr = X.ToArray();
        for (int j = 0; j < input.Length; j++) {
            nodeActivations[0][j] = xArr[j] > 0.001f;
        }

        // feed forward calculations
        for (int i = 0; i < weights.Length; i++) {
            // calculate feed forward
            Matrix Y = Matrix.dot(X, weights[i]);
            Y = Matrix.add(Y, biases[i]);
            Y.activateReLU();
            X = Y;

            // record curr layer node activation for display
            ffLayer++;
            xArr = X.ToArray();
            for (int j = 0; j < xArr.Length; j++) {
                nodeActivations[i+1][j] = xArr[j] > 0.001f;
            }
        }
        return X.ToArray();
    }

    // increase fitness for networks that did well
    public void powFitness(float pow) {
        fitness = Mathf.Pow(fitness, pow);
    }
}
