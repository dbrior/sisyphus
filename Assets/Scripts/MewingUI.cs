using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
// using UnityEngine.Experimental.Rendering.Universal;

public class SpriteAnimatorUI : MonoBehaviour
{
    public Sprite[] frames;           // Array of sprites to animate
    public float frameRate = 0.1f;    // Time between frames
    public bool useSecondaryAnimation = false;
    public Sprite[] secondaryFrames;
    public float secondaryFrameRate = 0.1f;
    
    public bool singleShot = false;   // Should the animation play only once?
    public bool isSprite = true;      // Is the component a SpriteRenderer?
    private Image imageComponent;     // Image component to display the sprites
    private SpriteRenderer spriteComponent; // SpriteRenderer to display the sprites

    public int currentFrame;         // Track the current frame index
    private float timer;              // Timer to manage animation speed
    private bool hasFired;            // Flag to check if single-shot animation has played

    void Start()
    {
        if (isSprite) {
            spriteComponent = GetComponent<SpriteRenderer>();
        } else {
            imageComponent = GetComponent<Image>();
        }
        // if (hasSpriteLight) {
        //     spriteLight = GetComponent<Light2D>();
        // }
    }

    void FixedUpdate()
    {
        if ((singleShot && !hasFired) || !singleShot)
        {
            timer += Time.deltaTime;

            if ((!useSecondaryAnimation && timer >= frameRate) || (useSecondaryAnimation && timer >= secondaryFrameRate))
            {
                if ((!useSecondaryAnimation && currentFrame < frames.Length) || (useSecondaryAnimation && currentFrame < secondaryFrames.Length))
                {
                    UpdateSpriteFrame();
                    currentFrame++;
                }

                if ((!useSecondaryAnimation && currentFrame >= frames.Length) || (useSecondaryAnimation && currentFrame >= secondaryFrames.Length))
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

                if (!useSecondaryAnimation) {
                    timer -= frameRate; // Reset the timer
                } else {
                    timer -= secondaryFrameRate; // Reset the timer
                }
            }
        }
    }

    private void UpdateSpriteFrame()
    {
        if (!useSecondaryAnimation) {
            if (isSprite && spriteComponent != null)
            {
                spriteComponent.sprite = frames[currentFrame];
            }
            else if (imageComponent != null)
            {
                imageComponent.sprite = frames[currentFrame];
            }
        } else {
            if (isSprite && spriteComponent != null)
            {
                spriteComponent.sprite = secondaryFrames[currentFrame];
            }
            else if (imageComponent != null)
            {
                imageComponent.sprite = secondaryFrames[currentFrame];
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // Destroy the GameObject
    }
}
