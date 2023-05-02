using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountdownTimer : MonoBehaviour
{
    [SerializeField] private float countdownStartTime;
    [SerializeField] private float countdown;

    private void Start()
    {
        countdown = countdownStartTime;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
    }
}
