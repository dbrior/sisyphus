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
        meter.fillAmount = (WorldController.Instance.currScore - goals[currGoalIdx].startDistance) / (goals[currGoalIdx].distance - goals[currGoalIdx].startDistance);
    }
    public void UpdateGoalEndDistance(DistanceGoal.Type targetGoalType, float newEndDistance) {
        for (int i = 0; i < goals.Count; i++) {
            if (goals[i].type == targetGoalType) {
                goals[i].distance = newEndDistance;
            }
        }
    }
    public void UpdateGoalStartDistance(DistanceGoal.Type targetGoalType, float newStartDistance) {
        for (int i = 0; i < goals.Count; i++) {
            if (goals[i].type == targetGoalType) {
                goals[i].startDistance = newStartDistance;
            }
        }
    }

    public void UpdateBothGoalDistances(DistanceGoal.Type targetGoalType, float newStartDistance, float newEndDistance) {
        UpdateGoalStartDistance(targetGoalType, newStartDistance);
        UpdateGoalEndDistance(targetGoalType, newEndDistance);
    }
}
