using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : Singleton<GameOverScreen>
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI boulderPrestigeText;
    public TextMeshProUGUI prestigeText;

    public void Activate() {
        float points = WorldController.Instance.points;
        int gemCount = WorldController.Instance.gemCount;
        int bossKills = WorldController.Instance.bossKills;
        int boulderPrestige = WorldController.Instance.boulder_b.currPrestige;


        float prestigePoints = 0f;
        // prestigePoints += (points / 10000f);
        // prestigePoints += (gemCount / 10f);
        // if (bossKills > 0) {
        //     prestigePoints += Mathf.Pow(2, bossKills-1);
        // }
        // if (boulderPrestige > 0) {
        //     prestigePoints += Mathf.Pow(5, boulderPrestige);
        // }
        prestigePoints = gemCount + bossKills + boulderPrestige;
        prestigePoints = Mathf.Floor(prestigePoints);

        pointsText.text = points.ToString("F0");
        gemsText.text = gemCount.ToString("F0");
        killsText.text = bossKills.ToString("F0");
        boulderPrestigeText.text = boulderPrestige.ToString("F0");

        prestigeText.text = prestigePoints.ToString("F0");
    }

}
