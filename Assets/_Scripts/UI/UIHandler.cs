using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text currentCargoText;
    [SerializeField] private TMP_Text maxCargoText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        OnScoreUpdateEvent += UpdateScore;
    }

    private void OnDestroy()
    {
        OnScoreUpdateEvent -= UpdateScore;
    }

    public delegate void OnScoreUpdate(int score);
    public OnScoreUpdate OnScoreUpdateEvent;

    private void UpdateScore(int score)
    {
        scoreText.text = "SCORE " + score.ToString();
    }

    public void UpdateCargoDelivered(int cargoDelivered)
    {
        currentCargoText.text = cargoDelivered.ToString();
    }

    public void SetMaxCargo(int maxCargo)
    {
        maxCargoText.text = "/ " + maxCargo.ToString();
    }
}