using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimatorUI : MonoBehaviour
{
    public Sprite[] frames;           // Array of sprites to animate
    public float frameRate = 0.1f;    // Time between frames
    public bool singleShot = false;   // Should the animation play only once?
    public bool isSprite = true;      // Is the component a SpriteRenderer?
    private Image imageComponent;     // Image component to display the sprites
    private SpriteRenderer spriteComponent; // SpriteRenderer to display the sprites

    private int currentFrame;         // Track the current frame index
    private float timer;              // Timer to manage animation speed
    private bool hasFired;            // Flag to check if single-shot animation has played

    void Start()
    {
        if (isSprite) {
            spriteComponent = GetComponent<SpriteRenderer>();
        } else {
            imageComponent = GetComponent<Image>();
        }
    }

    void FixedUpdate()
    {
        if ((singleShot && !hasFired) || !singleShot)
        {
            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                if (currentFrame < frames.Length)
                {
                    UpdateSpriteFrame();
                    currentFrame++;
                }

                if (currentFrame >= frames.Length)
                {
                    if (singleShot)
                    {
                        hasFired = true;
                        DestroySelf(); // Destroy the GameObject at the end of the animation
                    }
                    else
                    {
                        currentFrame = 0; // Loop back to the start
                    }
                }

                timer -= frameRate; // Reset the timer
            }
        }
    }

    private void UpdateSpriteFrame()
    {
        if (isSprite && spriteComponent != null)
        {
            spriteComponent.sprite = frames[currentFrame];
        }
        else if (imageComponent != null)
        {
            imageComponent.sprite = frames[currentFrame];
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // Destroy the GameObject
    }
}
