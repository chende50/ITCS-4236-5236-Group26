using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BoidBird : MonoBehaviour
{
    [SerializeField] public int visibleRange = 10;
    [SerializeField] public int protectedRange = 2;
    [SerializeField] public float avoidFactor = .0005f; // Separation
    [SerializeField] public float matchingFactor = .05f; // Alignment
    [SerializeField] public float centeringFactor = .05f; // Cohesion
    [SerializeField] public float turnFactor = .05f; // Avoid obstacles
    [SerializeField] public CircleCollider2D range;
    public Vector2 velocity = Vector2.zero;
    private int speed = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        range = GetComponent<CircleCollider2D>();
        // Set range to the light of light radius
        range.radius = visibleRange;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;
        // Store direction used to move away from close birds
        Vector2 close = Vector2.zero;

        Vector2 avgVelocity = Vector2.zero;

        Vector2 avgPosition = Vector2.zero;

        int neighborBirds = 0;

        // Get an array of all birds within the range
        List<Collider2D> closeBirds = new List<Collider2D>();
        range.Overlap(closeBirds);
        for (int i = 0; i < closeBirds.Count; i++)
        {
            Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 otherPosition = new Vector2(closeBirds[i].transform.position.x, closeBirds[i].transform.position.y);
            if (Pythagorean(thisPosition, otherPosition) < protectedRange)
            {
                close += thisPosition - otherPosition;
            } 
            else
            {
                avgVelocity += closeBirds[i].gameObject.GetComponent<BoidBird>().velocity;
                avgPosition += otherPosition;
                neighborBirds++;
            }
        }
        velocity += close.normalized * avoidFactor;

        if (neighborBirds > 0)
        {
            avgVelocity /= neighborBirds;
            avgPosition /= neighborBirds;
        }

        velocity += (avgVelocity - velocity).normalized * matchingFactor;
        velocity += (avgPosition - new Vector2(transform.position.x, transform.position.y)).normalized * centeringFactor;

        Vector3 displacement = new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime * speed;

        //transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, 0));

        transform.position += displacement;
    }

    private float Pythagorean(Vector2 position, Vector2 otherPosition)
    {
        float a = otherPosition.x - position.x;
        float b = otherPosition.y - position.y;
        float c = Mathf.Sqrt(a * a + b * b);
        return c;
    }
}
