using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private float maxY = 2;
    private float minY = -0.7f;
    private float maxX = 3.4f;
    private float minX = -3.2f;

    private float speed = 0.0002f;

    private Vector3 destination;
    private float totalDistance;
    private float startTime;

    private SpriteRenderer fish_sr;

    // Start is called before the first frame update
    void Start()
    {
        fish_sr = gameObject.GetComponent<SpriteRenderer>();
        DetermineDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (AtDestination())
        {
            DetermineDestination();
        }

        Move();

        FlipSprite();
    }

    private void FlipSprite()
    {
        if (transform.position.x < destination.x)
        {
            fish_sr.flipX = true;
        }
        else
        {
            fish_sr.flipX = false;
        }
    }

    private bool AtDestination()
    {
        float distanceThreshold = 0.1f;

        if (Vector3.Distance(transform.position, destination) < distanceThreshold)
        {
            return true;
        }
        return false;
    }

    private void DetermineDestination()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        destination = new Vector3(x, y, 0);
        startTime = Time.time;
        totalDistance = Vector3.Distance(transform.position, destination);
    }

    private void Move()
    {
        float distanceCovered = (Time.time - startTime) * speed;
        Vector3 oldPosition = transform.position;
        transform.position = Vector3.Lerp(oldPosition, destination, distanceCovered / totalDistance);
    }

}
