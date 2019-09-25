using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatingEffect : MonoBehaviour
{
    public float beatingAmplitude = 0.05f;
    public float beatingFrequency = 10.0f;

    private DotBehaviour dotBehaviour;

    private void Start() {
        dotBehaviour = transform.parent.GetComponent<DotBehaviour>();
    }
    // Update is called once per frame
    void FixedUpdate()//Vector3.One
    {
        if(dotBehaviour.selected)
            transform.localScale = Vector3.one + Vector3.one*beatingAmplitude * Mathf.Abs(Mathf.Sin(beatingFrequency * 0.5f * Time.time));
    }
}
