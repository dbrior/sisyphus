using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    public ParticleSystem particleSystem;

    void Start()
    {
        // Access the main module
        var main = particleSystem.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 10f);
        main.startSize = 0.1f;
        main.startColor = Color.yellow;
        main.gravityModifier = 0.5f;
        main.loop = true; // Enable looping

        // Access the shape module
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;
        shape.arc = 360f;

        // Access the emission module
        var emission = particleSystem.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 50),
            new ParticleSystem.Burst(0.5f, 50),
            new ParticleSystem.Burst(1.0f, 50),
            new ParticleSystem.Burst(1.5f, 50)
        });

        // Access the renderer module
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.lengthScale = 2f;
        renderer.velocityScale = 1f;

        // Ensure the particles are rendered in 2D correctly
        renderer.sortingLayerName = "Foreground"; // Set the appropriate sorting layer
        renderer.sortingOrder = 10; // Set the appropriate order within the layer
    }
}
