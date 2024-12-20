using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AsteroidMovement : NetworkBehaviour
{
    public Transform AsteroidDirection;
    public float _asteroidSpeed = 10f;


    private void Start()
    {
        NewTargetPosition(Vector3.zero);
    }
    private void Update()
    {
        AsteroidMovementServer();
    }

    public void AsteroidMovementServer()
    {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        transform.position += transform.up * _asteroidSpeed * Time.deltaTime;
    }
    private void NewTargetPosition(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}