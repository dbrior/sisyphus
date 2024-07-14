using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private Rigidbody2D rb;
    public Material glowMaterial;
    public int currPrestige = 0;
    public float maxGlowThickness = 0.03f;
    public float prestigeSpeed;
    public float glowStartPercent;
    private float glowStartSpeed;
    private float lastBelowSpeedTime;
    public AudioSource prestigeDrum;
    public AudioSource prestigeMusic;

    private float currDrumDelta = 0f;
    private float drumInterval = 1f;
    private float atPrestigeDrumInterval = 0.5f;
    private float drumLastPlayedTime = 0f;
    private float belowPrestigeSpeedTime = 0f;
    private bool onCooldown = false;
    private float originalMass;
    public SwipeDetection swipeDetection;
    private ClickShrinkEffect cse;

    public void Jump() {
        Debug.Log("Jumpin");
        if (currPrestige > 0) {
            rb.AddForce(Vector3.up * 60000f * 5f * 10f * currPrestige);
        } else {
            rb.AddForce(Vector3.up * 60000f * 5f);
        }
    }

    public void Touched(Vector3 touchLocation, bool wasAuto = false, int multiplicity= 1) {
        cse.Touched(touchLocation, wasAuto, multiplicity);
    }

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
        rb = GetComponent<Rigidbody2D>();
        cse = GetComponent<ClickShrinkEffect>();
        swipeDetection = GetComponent<SwipeDetection>();

        glowStartSpeed = glowStartPercent * prestigeSpeed;
        belowPrestigeSpeedTime = Time.time;
        originalMass = rb.mass;
    }

    IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        onCooldown = false;
    }

    void Update()
    {
        currDrumDelta += Time.deltaTime;
        if (WorldController.Instance.smoothedSpeed >= glowStartSpeed)
        {
            float currRatio = (WorldController.Instance.smoothedSpeed - glowStartSpeed) / (prestigeSpeed - glowStartSpeed);
            glowMaterial.SetFloat("_Thickness", Mathf.Min(currRatio * maxGlowThickness, maxGlowThickness));

            // Drum sound
            if (!onCooldown && (WorldController.Instance.smoothedSpeed >= prestigeSpeed) && ((Time.time - drumLastPlayedTime) >= atPrestigeDrumInterval) && (currDrumDelta >= atPrestigeDrumInterval)) {
                prestigeDrum.Play();
                drumLastPlayedTime = Time.time;
                currDrumDelta = 0f;
                if (Time.time - belowPrestigeSpeedTime > 5f) {
                    currPrestige += 1;
                    rb.mass = originalMass * 10f * currPrestige;
                    prestigeMusic.Play();
                    onCooldown = true;
                    cse.UpdateScale(gameObject.transform.localScale * 1.2f);
                    Vector3 newPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y * 1.2f, gameObject.transform.localPosition.z);
                    gameObject.transform.localPosition = newPosition;
                    // WorldController.Instance.resetBuffs();
                    StartCoroutine(Cooldown(10f));
                }
            } else if (!onCooldown && ((Time.time - drumLastPlayedTime) >= drumInterval) && (currDrumDelta >= drumInterval)) {
                prestigeDrum.Play();
                drumLastPlayedTime = Time.time;
                currDrumDelta = 0f;
                belowPrestigeSpeedTime = Time.time;
            }
        } else {
            belowPrestigeSpeedTime = Time.time;
            glowMaterial.SetFloat("_Thickness", 0f);
        }
    }
}
