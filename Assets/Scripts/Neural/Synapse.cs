using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse : MonoBehaviour
{
    [SerializeField] Neuron out_neuron;
    [SerializeField] float weight;

    private void Start()
    {
        weight = Random.Range(0.0f, 1.0f);
        GetComponentInChildren<UnityEngine.UI.Text>().text = weight.ToString("C2");
    }

    public void sendSignal(float signal_in)
    {
        out_neuron.receiveSignal(signal_in * weight);
    }
}
