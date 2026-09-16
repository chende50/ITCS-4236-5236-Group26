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
    [SerializeField] public float maxSpeed = 6;
    [SerializeField] public float minSpeed = 3;
    public float speed;
    public Vector2 velocity = Vector2.zero;
    public CircleCollider2D range;

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
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        range.Overlap(filter, closeBirds);
        Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i < closeBirds.Count; i++)
        {
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

        if (neighborBirds > 0)
        {
            avgVelocity /= neighborBirds;
            avgPosition /= neighborBirds;
        }

        velocity += (avgVelocity - velocity).normalized * matchingFactor;
        velocity += (avgPosition - new Vector2(transform.position.x, transform.position.y)).normalized * centeringFactor;
        velocity += close.normalized * avoidFactor;

        Rect boundary = Camera.main.pixelRect;
        if (!boundary.Contains(thisPosition))
        {
            if(thisPosition.x > boundary.xMax)
            {
                velocity.x -= turnFactor;
            }
            if (thisPosition.x < boundary.xMin)
            {
                velocity.x += turnFactor;
            }
            if (thisPosition.y > boundary.yMax)
            {
                velocity.y -= turnFactor;
            }
            if (thisPosition.y < boundary.yMin)
            {
                velocity.y += turnFactor;
            }
        }

        speed = Pythagorean(thisPosition, velocity);
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        Vector3 displacement = velocity * speed * (1 + Time.deltaTime);

        float dotProduct = Vector2.Dot(new Vector2(transform.position.x, transform.position.y), displacement);
        transform.Rotate(new Vector3(0,0,dotProduct));

        transform.position += displacement;
        Debug.DrawLine(transform.position, transform.position + displacement * 50);
    }

    private float Pythagorean(Vector2 position, Vector2 otherPosition)
    {
        float a = otherPosition.x - position.x;
        float b = otherPosition.y - position.y;
        float c = Mathf.Sqrt(a * a + b * b);
        return c;
    }
}
