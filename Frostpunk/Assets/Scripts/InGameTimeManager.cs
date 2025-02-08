using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InGameTimeManager : MonoBehaviour
{
    public static InGameTimeManager Instance;

    public event Action OnTimeStep;

    private float timeElapsed = 0f;
    private float timeStep = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeStep)
        {
            timeElapsed -= timeStep;
            OnTimeStep?.Invoke();
        }
    }
}
