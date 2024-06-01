using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float mass = 1.0f;
    public float tapDecayMps2;
    public Material glowMaterial;
    private float currPrestige = 0f;
    public float maxGlowThickness = 0.03f;
    public float prestigeSpeed;
    public float glowStartPercent;
    private float glowStartSpeed;
    private float lastBelowSpeedTime;
    public AudioSource prestigeDrum;

    private float currDrumDelta = 0f;
    private float drumInterval = 1f;
    private float atPrestigeDrumInterval = 0.5f;
    private float drumLastPlayedTime = 0f;


    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.Log("Collision:");
            Debug.Log(contact);
            Debug.DrawRay(contact.point, contact.normal, Color.blue);
        }
    }
    void Start()
    {
        glowStartSpeed = glowStartPercent * prestigeSpeed;
    }

    void Update()
    {
        currDrumDelta += Time.deltaTime;
        if (WorldController.Instance.smoothedSpeed >= glowStartSpeed)
        {
            float currRatio = (WorldController.Instance.smoothedSpeed - glowStartSpeed) / (prestigeSpeed - glowStartSpeed);
            glowMaterial.SetFloat("_Thickness", Mathf.Min(currRatio * maxGlowThickness, maxGlowThickness));

            // Drum sound
            if ((WorldController.Instance.smoothedSpeed >= prestigeSpeed) && ((Time.time - drumLastPlayedTime) >= atPrestigeDrumInterval) && (currDrumDelta >= atPrestigeDrumInterval)) {
                prestigeDrum.Play();
                drumLastPlayedTime = Time.time;
                currDrumDelta = 0f;
            } else if (((Time.time - drumLastPlayedTime) >= drumInterval) && (currDrumDelta >= drumInterval)) {
                prestigeDrum.Play();
                drumLastPlayedTime = Time.time;
                currDrumDelta = 0f;
            }
        } else {
            glowMaterial.SetFloat("_Thickness", 0f);
        }
    }
}
