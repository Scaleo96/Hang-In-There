using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetModel : MonoBehaviour
{
    [SerializeField]
    private Transform follow;
    [SerializeField]
    private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = follow.position + offset;
        transform.rotation = new Quaternion(transform.rotation.x, follow.rotation.y, transform.rotation.z, follow.rotation.w);
    }
}
