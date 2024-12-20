using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroyArrow : NetworkBehaviour
{
    public float _maxDistance = 10f;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTraveled = Vector3.Distance(_startPosition, transform.position);

        if(distanceTraveled >= _maxDistance )
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!IsOwner)
        {
            if (collision.gameObject.tag == "Asteroids")
            {
                Destroy(gameObject);
            }
        }
    }
}
