using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Matrix
{
    float[,] data;
    int rows;
    int cols;

    public Matrix(int rows, int cols) {
        this.data = new float[rows, cols];
        this.rows = rows;
        this.cols = cols;
    }

    public Matrix(Matrix m) {
        this.rows = m.rows;
        this.cols = m.cols;
        this.data = new float[rows, cols];
        for(int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                this.data[row,col] = m.data[row,col];
            }
        }
    }

    public Matrix(float[,] data) {
        this.rows = data.GetLength(0);
        this.cols = data.GetLength(1);
        this.data = new float[this.rows,this.cols];
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                this.data[row,col] = data[row,col];
            }
        }
    }

    public Matrix(float[] arr) {
        this.rows = 1;
        this.cols = arr.Length;
        this.data = new float[this.rows,this.cols];
        for (int col = 0; col < cols; col++) {
            this.data[0,col] = arr[col];
        }
    }

    public void randomize() {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                data[row, col] = Random.Range(-1f, 1f);
            }
        }
    }

    public void activateReLU() {
        if (rows > 1) {
            Debug.LogWarning("ReLU applying to Matrix with >1 rows");
        }
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                if (data[row,col] < 0f) {
                    data[row,col] = 0f;
                }
            }
        }
    }

    public override string ToString()
    {
        string res = "";
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                res += data[row, col].ToString() + "\t";
            }
            res += "\n";
        }
        return res;
    }

    public float[] ToArray() {
        if (rows > 1) {
            Debug.LogWarning("ToArray() calling on Matrix with >1 rows");
        }
        float[] res = new float[cols];
        for (int col = 0; col < cols; col++) {
            res[col] = data[0,col];
        }
        return res;
    }

    public static Matrix dot(Matrix m1, Matrix m2) {
        if (m1.cols != m2.rows) {
            throw new System.Exception("M1 Columns do not match M2 Rows.");
        }

        Matrix res = new Matrix(m1.rows, m2.cols);
        for (int i = 0; i < m1.rows; i++) {
            for (int j = 0; j < m2.cols; j++) {
                for (int k = 0; k < m1.cols; k++) {
                    float prod = m1.data[i,k] * m2.data[k,j];
                    res.data[i,j] += prod;
                    NeuralNetwork.weightActivations[NeuralNetwork.ffLayer][k][j] = prod > 0f;
                }
            }
        }
        return res;
    }

    public static Matrix add(Matrix m1, Matrix m2) {
        Matrix res = new Matrix(m1.rows, m1.cols);
        for (int row = 0; row < m1.rows; row++) {
            for (int col = 0; col < m1.cols; col++) {
                res.data[row,col] = m1.data[row,col] + m2.data[row,col];
            }
        }
        return res;
    }

    public static Matrix randomJoin(Matrix m1, Matrix m2) {
        Matrix res = new Matrix(m1.rows, m1.cols);

        for (int row = 0; row < m1.rows; row++) {
            for (int col = 0; col < m1.cols; col++) {
                res.data[row,col] = (Random.value > 0.5f) ? m1.data[row,col] : m2.data[row,col];
            }
        }

        return res;
    }

    public static Matrix mutate(Matrix m, float volatility) {
        Matrix res = new Matrix(m);
        for (int row = 0; row < m.rows; row++) {
            for (int col = 0; col < m.cols; col++) {
                if (Random.value < volatility) {
                    res.data[row,col] += Random.Range(-5f, 5f);
                }
            }
        }
        return res;
    }

    public int getRows() {
        return rows;
    }

    public int getCols() {
        return cols;
    }

    public float get(int row, int col) {
        return data[row,col];
    }
}
