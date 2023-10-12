using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPosition;
    public float repeatDistance;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        repeatDistance = GetComponent<BoxCollider>().size.y * 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < startPosition.y - repeatDistance)
        {
            transform.position = startPosition;
        }
    }
}
