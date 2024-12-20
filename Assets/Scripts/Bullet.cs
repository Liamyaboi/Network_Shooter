using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet :NetworkBehaviour
{
    public Transform _firePoint;
    public GameObject _arrowPrefab;
    public float _arrowSpeed = 10f;

    [ServerRpc]
    public void ShootServerRpc()
    {
        GameObject arrow = Instantiate(_arrowPrefab, _firePoint.position, _firePoint.rotation);

        arrow.GetComponent<NetworkObject>().Spawn();

        Quaternion playerRotation = transform.rotation;

        arrow.transform.rotation = playerRotation;

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(_firePoint.up * _arrowSpeed, ForceMode2D.Impulse);
    }
}