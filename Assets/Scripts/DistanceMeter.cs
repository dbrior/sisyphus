using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeter : MonoBehaviour
{
    public Image meter;
    public Image icon;
    public List<DistanceGoal> goals;
    private int currGoalIdx;
    private float currGoalDistance;

    void Start() {
        currGoalIdx = 0;
        UpdateUI();
    }
    void Update() {
        UpdateMeter();
    }
    private void UpdateUI() {
        currGoalDistance = goals[currGoalIdx].distance;
        icon.sprite = goals[currGoalIdx].icon;
    }
    public void NextGoal() {
        currGoalIdx = (currGoalIdx + 1) % goals.Count;
        UpdateUI();
    }
    public void UpdateMeter() {
        meter.fillAmount = WorldController.Instance.currScore / currGoalDistance;
    }
}
