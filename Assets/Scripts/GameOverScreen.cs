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

        // High-yield 
        // prestigePoints += (points / 10000f);
        prestigePoints += (gemCount * 5000);
        if (bossKills > 0) {
            prestigePoints += Mathf.Pow(2, bossKills-1);
        }
        if (boulderPrestige > 0) {
            prestigePoints += Mathf.Pow(5, boulderPrestige);
        }

        // Low-yield
        // prestigePoints = gemCount + bossKills + boulderPrestige;


        prestigePoints = Mathf.Floor(prestigePoints);
        Debug.Log("Prestige Points Gained: " + prestigePoints.ToString());

        pointsText.text = points.ToString("F0");
        gemsText.text = gemCount.ToString("F0");
        killsText.text = bossKills.ToString("F0");
        boulderPrestigeText.text = boulderPrestige.ToString("F0");

        prestigeText.text = prestigePoints.ToString("F0");

        PlayerPrefs.SetFloat("Prestige Points", prestigePoints + WorldController.Instance.GetPrestigePoints());
        PlayerPrefs.Save();
    }

}
