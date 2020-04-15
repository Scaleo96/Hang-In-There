using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    private LineRenderer line;
    private Rigidbody rb;

    [SerializeField]
    private float ropeRetractSpeed = 0.2f;

    private Transform anchorPos;
    private Vector3 offset;
    private float lastYPos;
    private float minimunDistanceToAnchor;

    [SerializeField]
    private Transform model;
    [SerializeField]
    private Transform lineTarget;
    [SerializeField]
    private float rotateSpeed;

    private bool ropeActive = false;

    public bool RopeActive { get { return ropeActive; } }

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public void DestroyRope()
    {
        line.enabled = false;
        ropeActive = false;
        model.rotation = transform.rotation;
    }

    public void BuildRope(Transform target, Vector3 offset)
    {
        this.offset = offset;
        minimunDistanceToAnchor = Vector3.Distance(target.position + this.offset, transform.position);
        line.enabled = true;
        line.positionCount = 2;
        anchorPos = target;
        ropeActive = true;
    }

    private void LateUpdate()
    {
        if (ropeActive)
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = lineTarget.position;
            positions[1] = anchorPos.position + offset;
            line.SetPositions(positions);
        }
    }

    public void UpdatePositions()
    {
        if (anchorPos != null)
        {
            float distance = Vector3.Distance(anchorPos.position + offset, transform.position);

            if (distance > minimunDistanceToAnchor)
            {
                float correction = distance - minimunDistanceToAnchor;
                Vector3 dir = (anchorPos.position + offset) - transform.position;
                dir.Normalize();
                float dot = Vector3.Dot(rb.velocity, dir);
                Vector3 dirStr = dir * dot;
                rb.velocity = rb.velocity - dirStr;
                transform.position += dir * correction;
            }
            else
            {
                minimunDistanceToAnchor = distance;
            }
        }
        else
        {
            DestroyRope();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ropeActive)
        {
            Vector3 dir = (anchorPos.position + offset) - transform.position;
            dir.Normalize();
            Quaternion leanRotation = Quaternion.LookRotation(Vector3.Cross(dir, transform.position), dir);
            model.rotation = Quaternion.RotateTowards(model.rotation, leanRotation, rb.velocity.magnitude * 0.05f);
            //float angle = model.rotation.z - transform.rotation.z;

            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    RaycastHit hit;
                    if (!rb.SweepTest(dir, out hit, 2))
                    {
                        minimunDistanceToAnchor -= ropeRetractSpeed;
                        rb.velocity = rb.velocity + (dir * ropeRetractSpeed);
                    }
                }
            }
        }

    }
}
