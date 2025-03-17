using System;
using Unity.Netcode;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NetworkTransformTest : NetworkBehaviour
{
    Transform target;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player1");
        target = player.transform;
    }
    void Update()
    {
        if (IsServer)
        {
            float theta = Time.time;
            transform.position = new Vector3((float)Math.Cos(theta), 0.0f, (float)Math.Sin(theta));
            target = GameObject.FindWithTag("Player1").transform;
            target.position = new Vector3(target.position.x, target.position.y, 5);
            transform.position = target.position;
        }
    }
}
