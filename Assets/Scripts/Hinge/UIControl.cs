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
}
