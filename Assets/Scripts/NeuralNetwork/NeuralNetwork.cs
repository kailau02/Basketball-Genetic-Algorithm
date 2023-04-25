using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork
{
    public static int ffLayer = 0; // Used by Matrix.dot for live diagram
    public static bool[][] nodeActivations;
    public static bool[][][] weightActivations;

    public Matrix[] weights;
    public Matrix[] biases;

    public int[] layers;

    public float fitness = 0;

    public NeuralNetwork() {}

    public NeuralNetwork(string fpath) {
        loadNN(fpath);
        setupActivationShapes();
    }

    public NeuralNetwork(int[] layers) {
        this.layers = layers;
        createNN();
        setupActivationShapes();
    }

    public void createNN() {
        int nLayers = layers.Length;

        // setup weights
        weights = new Matrix[nLayers - 1];
        biases = new Matrix[nLayers - 1];
        for (int i = 0; i < weights.Length; i++) {
            // setup weights
            weights[i] = new Matrix(layers[i], layers[i + 1]);
            weights[i].randomize();

            // setup biases
            biases[i] = new Matrix(1, layers[i + 1]);
            biases[i].randomize();
        }
    }

    private void setupActivationShapes() {
        int nLayers = layers.Length;
        nodeActivations = new bool[nLayers][];
        weightActivations = new bool[nLayers - 1][][];

        for (int i = 0; i < nLayers; i++) {
            nodeActivations[i] = new bool[layers[i]];
        }

        for (int i = 0; i < weights.Length; i++) {

            weightActivations[i] = new bool[layers[i]][];
            for (int j = 0; j < layers[i]; j++) {
                weightActivations[i][j] = new bool[layers[i + 1]];
            }
        }
    }

    public void loadNN(string fpath) {
        if (File.Exists(Application.persistentDataPath + '/' + fpath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + '/' + fpath, FileMode.Open);
            NeuralNetwork savedNN = (NeuralNetwork)bf.Deserialize(file);
            file.Close();

            // Copy saved data to this neural network
            this.weights = savedNN.weights;
            this.biases = savedNN.biases;
            this.layers = savedNN.layers;
        }
    }
    public void saveNN(string fpath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + '/' + fpath);
        bf.Serialize(file, this);
        file.Close();
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

    public bool WeakEquals(NeuralNetwork other) {
        for (int i = 0; i < weights.Length; i++) {
            Matrix m1 = weights[i];
            Matrix m2 = other.weights[i];

            Matrix b1 = biases[i];
            Matrix b2 = other.biases[i];


            for (int row = 0; row < m1.getRows(); row++) {
                for (int col = 0; col < m1.getCols(); col++) {
                    if (Mathf.Abs(m1.get(row, col) - m2.get(row, col)) > 0.01f) {
                        return false;
                    }
                }
            }

            for (int row = 0; row < b1.getRows(); row++) {
                for (int col = 0; col < b1.getCols(); col++) {
                    if (Mathf.Abs(b1.get(row, col) - b2.get(row, col)) > 0.01f) {
                        return false;
                    }
                }
            }

        }
        return true;
    }
}
