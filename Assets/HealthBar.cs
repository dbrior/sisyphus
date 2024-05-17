using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    public Image healthBarBackground;
    private float healthPercentage = 1f;

    public void SetHealth(float newHealthPercentage)
    {
        healthPercentage = newHealthPercentage;
        healthBarFill.fillAmount = healthPercentage;
    }
}
