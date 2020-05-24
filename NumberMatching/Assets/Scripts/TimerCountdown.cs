using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdown : MonoBehaviour {

    [Header("All Timers")]
    public bool timerStarted = false;
    public bool isPaused = false;

    [Header("Next Timer")]
    //private TextMeshProUGUI textTimer;
    public float nextCountdownDuration = 5f;
    public float nextTimeLeft = 0f;
    private Coroutine nextCoroutine;

    [Header("Blocker Timer")]
    [SerializeField] GameBoardMechanics gameboard = default;
    public float blockerCountdownDuration = 5f;
    public float blockerCountdownDurationMin = 1f;
    public float blockerTimeLeft = 0f;
    private Coroutine blockerCoroutine;

    public void StartTimerCountdown() {
        timerStarted = true;
        blockerCoroutine = StartCoroutine(BlockerCountdown());
    }

    IEnumerator BlockerCountdown() {
        blockerTimeLeft = blockerCountdownDuration;
        while (blockerTimeLeft > 0) {
            while (isPaused) {
                yield return null;
            }
            blockerTimeLeft -= Time.deltaTime;
            yield return null;
        }
        BlockerCountDownCompleted();
    }

    private void BlockerCountDownCompleted() {
        Debug.Log("Add A New Blocker to the GameBoard");
        blockerCoroutine = StartCoroutine(BlockerCountdown());
        gameboard.AddBlockerToBoard();
    }

    public void ReduceBlockerCountdownDuration() {
        Debug.Log("Blocker Timer Reduced: " + blockerCountdownDuration);
        blockerCountdownDuration--;
        if (blockerCountdownDuration < blockerCountdownDurationMin) {
            blockerCountdownDuration = blockerCountdownDurationMin;
            Debug.Log("Blocker Timer At Minimum: " + blockerCountdownDuration);
        }
    }

    public void StopTimer() {
        StopCoroutine(blockerCoroutine);
    }

}
