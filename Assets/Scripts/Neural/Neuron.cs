using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Type
{
    INPUT,
    HIDDEN,
    OUTPUT
}

public class Neuron : MonoBehaviour
{
    [SerializeField] Type type;
    [SerializeField] Synapse[] synapses;
    [SerializeField] float strength;
    bool signal_sent = false;

    private void Start()
    {
        strength = Random.Range(0.0f, 1.0f);
        GetComponentInChildren<UnityEngine.UI.Text>().text = strength.ToString("C2");
        if(type == Type.INPUT)
        {
            sendSignal();
        }
    }

    void sendSignal()
    {
        if (!signal_sent)
        {
            signal_sent = true;
            foreach (Synapse s in synapses)
            {
                s.sendSignal(strength);
            }
        }
    }

    public void receiveSignal(float signal)
    {
        strength += signal;
        GetComponentInChildren<UnityEngine.UI.Text>().text = strength.ToString("C2");
        if (type == Type.HIDDEN && strength > 1)
        {
            sendSignal();
        }
    }
}
