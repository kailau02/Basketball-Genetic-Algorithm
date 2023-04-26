using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{

    public TextMeshProUGUI generationText;
    public TextMeshProUGUI trialText;
    public TextMeshProUGUI networkText;
    public TextMeshProUGUI scoreText;

    public Slider timeSlider;
    public void SetGenerationText(int generation) {
        generationText.text = "Generation " + (generation + 1);
    }

    public void SetTrialText(int trial, int maxTrials) {
        trialText.text = "Trial " + (trial + 1) + "/" + maxTrials;
    }

    public void SetNetworkText(int network, int maxNetworks) {
        networkText.text = "Network " + (network + 1) + "/" + maxNetworks;
    }

    public void OnTimeScaleUpdate() {
        Time.timeScale = timeSlider.value * 98 + 1;
    }

    public void UpdateScore(int score, int total) {
        scoreText.text = "Score: " + score + "/" + total + "\n" + "Accuracy: " + ((float) (score + 1) / (float) (total + 1) * 100).ToString("F2") + "%";
    }
}
