using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimatorUI : MonoBehaviour
{
    public Sprite[] frames;        // Array of sprites to animate
    public float frameRate = 0.1f; // Time between frames
    private Image imageComponent;  // Image component to display the sprites

    private int currentFrame;      // Track the current frame index
    private float timer;           // Timer to manage animation speed

    void Start()
    {
        imageComponent = GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError("SpriteAnimatorUI requires an Image component on the same GameObject.");
            enabled = false; // Disable script if no Image component is found
            return;
        }

        if (frames.Length == 0)
        {
            Debug.LogError("Frames array is empty. Please assign sprite frames in the inspector.");
            enabled = false; // Disable script if no frames are assigned
            return;
        }

        imageComponent.sprite = frames[0]; // Set the initial sprite frame
    }

    void Update()
    {
        if (frames.Length > 1)
        {
            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                currentFrame = (currentFrame + 1) % frames.Length; // Loop back to start
                imageComponent.sprite = frames[currentFrame];      // Update the sprite frame
                timer -= frameRate;                                // Reset the timer
            }
        }
    }
}
