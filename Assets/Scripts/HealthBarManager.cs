using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : Singleton<HealthBarManager>
{
    public HealthBar healthBarPrefab;
    public Transform healthBarCanvas;

    private Dictionary<GameObject, HealthBar> healthBars = new Dictionary<GameObject, HealthBar>();

    public void CreateHealthBar(GameObject character)
    {
        Debug.Log("Creating Healthbar");
        HealthBar healthBar = Instantiate(healthBarPrefab, healthBarCanvas);
        healthBars[character] = healthBar;
    }

    public void DestroyHealthBar(GameObject character) {
        Destroy(healthBars[character].gameObject);
        healthBars.Remove(character);
    }

    public void UpdateHealthBar(GameObject character, float healthPercentage)
    {
        if (healthBars.ContainsKey(character))
        {
            healthBars[character].SetHealth(healthPercentage);
        }
    }

    private void LateUpdate()
    {
        List<GameObject> keysToRemove = new List<GameObject>();

        foreach (var entry in healthBars)
        {
            GameObject character = entry.Key;
            HealthBar healthBar = entry.Value;
            if (character != null)
            {
                // Vector3 screenPosition = Camera.main.WorldToScreenPoint(character.transform.position + Vector3.up * 2); // Adjust the offset as needed
                // screenPosition.z = 0;
                // healthBar.transform.position = screenPosition;
                healthBar.transform.position = character.transform.position + Vector3.up * 0.5f;
            }
            else
            {
                Destroy(healthBar.gameObject);
                keysToRemove.Add(character);
            }
        }

        foreach (var key in keysToRemove)
        {
            healthBars.Remove(key);
        }
    }
}
