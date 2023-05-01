using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance { get; private set; }

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

    [SerializeField] private TMP_Text scoreText;

    public delegate void OnScoreUpdate(int score);
    public OnScoreUpdate OnScoreUpdateEvent;

    private void UpdateScore(int score)
    {
        scoreText.text = "SCORE " + score.ToString();
    }
}
