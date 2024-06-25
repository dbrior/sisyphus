using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDetection : MonoBehaviour
{
    public UnityEvent OnSwipe;
    private Dictionary<int, Vector2> touchStartPositions = new Dictionary<int, Vector2>();
    public float swipeThreshold, swipeAngleMin, swipeAngleMax;
    private Collider2D collider;
    private Sisyphus sisyphus;

    void Start() {
        collider = GetComponent<Collider2D>();
        sisyphus = WorldController.Instance.sisyphus;

        if (OnSwipe == null) OnSwipe = new UnityEvent();
    }

    void Update()
    {
        DetectSwipes();
    }

    // TODO: Make more robust to different swiping directions
    void DetectSwipes()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchOnObject(touch)) touchStartPositions[touch.fingerId] = touch.position;
                    break;

                case TouchPhase.Ended:
                    if (touchStartPositions.TryGetValue(touch.fingerId, out Vector2 startTouchPosition))
                    {
                        Vector2 endTouchPosition = touch.position;
                        if (IsSwipeUp(startTouchPosition, endTouchPosition))
                        {
                            OnSwipe.Invoke();
                        }
                        touchStartPositions.Remove(touch.fingerId);
                    }
                    break;
            }
        }
    }

    float ToAngle(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return angle;
    }


    bool IsSwipeUp(Vector2 startTouchPosition, Vector2 endTouchPosition)
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;
        float swipeAngle = ToAngle(swipeDirection);

        Debug.Log("Swipe: " + swipeDirection.ToString() + " | " + swipeAngle.ToString());

        return swipeDirection.y > swipeThreshold && swipeAngle >= swipeAngleMin && swipeAngle <= swipeAngleMax;
    }

    bool IsTouchOnObject(Touch touch)
    {
        return collider.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position));
    }
}
