using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (countdown <= 0)
        {
            countdown = 0;
            SceneManager.LoadScene("Loss");
        }

        UIHandler.Instance.UpdateCountdown(countdown);
    }
}
