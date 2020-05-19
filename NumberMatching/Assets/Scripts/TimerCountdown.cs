using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdown : MonoBehaviour {

    [Header("All Timers")]
    public bool started = false;

    [Header("Next Timer")]
    //private TextMeshProUGUI textTimer;
    public float nextCountdownDuration = 5f;
    public float nextTimeLeft = 0f;
    [SerializeField] NextBoardMechanics nextBoard = default;
    private Coroutine nextCoroutine;

    [Header("Blocker Timer")]
    [SerializeField] GameBoardMechanics gameboard = default;
    public float blockerCountdownDuration = 5f;
    public float blockerCountdownDurationMin = 1f;
    public float blockerTimeLeft = 0f;
    private Coroutine blockerCoroutine;

    public void StartTimerCountdown() {
        started = true;
        //nextCoroutine = StartCoroutine(StartCountdown());
        blockerCoroutine = StartCoroutine(BlockerCountdown());
    }

    IEnumerator StartCountdown() {
        nextTimeLeft = nextCountdownDuration;
        while (nextTimeLeft > 0) {
            yield return new WaitForSeconds(1.0f);
            nextTimeLeft--;
        }
        CountDownCompleted();
    }

    IEnumerator BlockerCountdown() {
        blockerTimeLeft = blockerCountdownDuration;
        while (blockerTimeLeft > 0) {
            yield return new WaitForSeconds(1.0f);
            blockerTimeLeft--;
        }
        BlockerCountDownCompleted();
    }

    private void CountDownCompleted() {
        Debug.Log("Add A New Square to Next Board");
        nextCoroutine = StartCoroutine(StartCountdown());
        nextBoard.AddOneNextSquareNumber();
    }

    private void BlockerCountDownCompleted() {
        Debug.Log("Add A New Blocker to the GameBoard");
        blockerCoroutine = StartCoroutine(BlockerCountdown());
        gameboard.AddBlockerToField();
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
        //StopCoroutine(nextCoroutine);
        StopCoroutine(blockerCoroutine);
    }
}
