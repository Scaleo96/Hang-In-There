using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player_Manager : Entity
{
    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float airMaxSpeed;
    [SerializeField]
    [Range(0, 1)]
    private float airDrag;
    [SerializeField]
    private float groundDragAmount;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float height;
    [SerializeField]
    private float heightPadding;
    [SerializeField]
    private float maxGroundAngle = 120;

    [Header("Misc")]
    [SerializeField]
    private int[] useLayers;
    [SerializeField]
    public string animName;

    private float jumpleway = 0.5f;
    private float jumpTimer = 0;
    private float animVertical = 0;
    private float animHorizontal = 0;
    private float vertical = 0;
    private float horizontal = 0;
    private bool playAnim;

    private Vector3 movementForce;
    private Vector3 forward;
    private Vector3 right;
    private RaycastHit hitInfo;
    private float groundAngleForward;
    private float groundAngleRight;
    private bool onGround = true;
    private bool detectGround = true;
    private bool canJump = true;
    private bool currentlyJumping = false;

    [SerializeField]
    private LayerMask groundLayer;

    private Vector3 oldVelocity = Vector3.zero;

    [SerializeField]
    private Game_Manager gm;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private IKController ik; 

    private Rigidbody rb;
    private GrappleHook rope;
    private Camera cam;
    private ProjectileShooter ps;

    private int[] layersmasks;
    private int layers;

    [HideInInspector]
    public bool active = true;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rope = GetComponent<GrappleHook>();
        cam = Camera.main;
        ps = GetComponent<ProjectileShooter>();

        layersmasks = new int[useLayers.Length];
        layers = 0;

        for (int i = 0; i < useLayers.Length; i++)
        {
            layersmasks[i] = 0;
            layersmasks[i] = 1 << useLayers[i];
            layers = layers | layersmasks[i];
        }

        active = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (active)
        {
            if (Input.anyKey)
            {
                PlayerJump();
                FireRope();
                PlayerShoot();
                ActivateEntity();
            }

            UpdateAnimations();

            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0)
                {
                    currentlyJumping = false;
                }
            }
        }

        DebugDrawLines();
    }

    private void FixedUpdate()
    {
        if (active)
        {
            PlayerMovment();
            CheckGround();
            CalculateGroundAngle();
            CalculateDirections();
        }

        if (rope.RopeActive)
            rope.UpdatePositions();

    }

    public override void EntityDeath()
    {
        Debug.Log("Lose");
    }

    private void UpdateAnimations()
    {
        animVertical = Mathf.MoveTowards(animVertical, vertical, 0.05f);
        animHorizontal = Mathf.MoveTowards(animHorizontal, horizontal, 0.05f);
        anim.SetFloat("Vertical", animVertical);
        anim.SetFloat("Horizontal", animHorizontal);
    }

    private void PlayerMovment()
    {
        if (!onGround)
        {

            movementForce = Vector3.zero;

            if (Input.GetAxis("Horizontal") != 0)
            {
                movementForce += transform.right * Input.GetAxis("Horizontal");
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                movementForce += transform.forward * Input.GetAxis("Vertical");
            }

            rb.velocity += new Vector3(movementForce.x * speed * airDrag * 10, 0, movementForce.z * speed * airDrag * 10);

            rb.velocity = (rb.velocity * (1 - airDrag));
        }
        else
        {

            movementForce = Vector3.zero;
            if (groundAngleRight < maxGroundAngle)
            {
                horizontal = Input.GetAxis("Horizontal");
                movementForce += right * horizontal;
            }

            if (groundAngleForward < maxGroundAngle)
            {
                vertical = Input.GetAxis("Vertical");
                movementForce += forward * vertical;
            }


            //rb.velocity = new Vector3(movmentForce.x * speed, movmentForce.y * speed + rb.velocity.y + Physics.gravity.y * Time.deltaTime, movmentForce.z * speed);


            transform.position += (movementForce.normalized * speed) * 0.01f;
            rb.velocity = Vector3.zero;

            if (rb.velocity.magnitude > maxSpeed && onGround)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        }
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && canJump && !rope.RopeActive)
        {
            movementForce = Vector3.zero;


            movementForce += transform.right * Input.GetAxis("Horizontal");
            movementForce += transform.forward * Input.GetAxis("Vertical");

            LeaveGround("Jump");
            canJump = false;
            currentlyJumping = true;
            jumpTimer = jumpleway;
            rb.AddForce(Vector3.up * jumpForce + movementForce.normalized * jumpForce, ForceMode.Impulse);
            //anim.Play("Jump");
        }
    }

    private void PlayerShoot()
    {
        if (Input.GetButton("Fire1"))
        {
            ps.ShootProjectiles(cam.transform.forward, cam.transform.position);
            ik.rightHandOn = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            ik.rightHandOn = false;
        }
    }

    private void FireRope()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (rope.RopeActive)
            {
                rope.DestroyRope();
                ik.SetIK(false, Vector3.zero);
            }
            else
            {
                Debug.Log("fire rope");
                RaycastHit hit;
                Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);
                if (hit.transform != null)
                {
                    Vector3 offset = hit.point - hit.transform.position;
                    rope.BuildRope(hit.transform, offset);
                    canJump = true;
                    ik.SetIK(true, offset, hit.transform);
                }
                else
                {
                    Debug.LogError("No target");
                }

            }
        }
    }

    private void ActivateEntity()
    {
        if (Input.GetButtonDown("Use"))
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10, layers);

            if (hit.collider != null)
            {
                Entity entity = hit.collider.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.UseEntity(gm);
                }
            }
        }
    }

    public void PauseMovement()
    {
        oldVelocity = rb.velocity;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        animHorizontal = 0;
        animVertical = 0;
        horizontal = 0;
        vertical = 0;
    }

    public void ResumeMovement()
    {
        rb.velocity = oldVelocity;
        rb.useGravity = true;
    }

    private void CalculateDirections()
    {
        if (!onGround)
        {
            forward = transform.forward;
            right = transform.right;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
        right = Vector3.Cross(hitInfo.normal, transform.forward);
    }

    private void CalculateGroundAngle()
    {
        if (!onGround)
        {
            groundAngleForward = 90;
            groundAngleRight = 90;
            return;
        }

        groundAngleForward = Vector3.Angle(hitInfo.normal, transform.forward);
        groundAngleRight = Vector3.Angle(transform.right, hitInfo.normal);
    }

    private void CheckGround()
    {
        //if (Physics.Raycast(transform.position + (transform.forward * 0.5f), -Vector3.up, out hitInfo, height + heightPadding, groundLayer)){
        if (!currentlyJumping)
        {
            if (Physics.BoxCast(transform.position, new Vector3(0.3f, 0.1f, 0.3f), -Vector3.up, out hitInfo, Quaternion.identity, height + heightPadding, groundLayer))
            {
                if (!onGround)
                {
                    onGround = true;
                    //canJump = true;
                    anim.Play("EndJump");
                }
                if (transform.position.y - hitInfo.point.y <= height)
                {
                    canJump = true;
                    DisableGravity();

                }
                else
                {
                    ApplyGravity();
                }
            }
            else
            {
                ApplyGravity();

                if (onGround)
                {
                    LeaveGround("MidJump");
                }
            }
        }
    }

    private void LeaveGround(string playAnim)
    {
        onGround = false;
        vertical = 0;
        horizontal = 0;
        anim.Play(playAnim);
    }

    private void ApplyGravity()
    {
        if (!onGround)
        {
            rb.useGravity = true;
        }
    }

    private void DisableGravity()
    {
        if (onGround)
        {
            rb.useGravity = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //DisableGravity();
        //if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        //{
        //    onGround = true;
        //    canJump = true;
        //    anim.Play("EndJump");
        //    if (rope.RopeActive)
        //        rope.DestroyRope();
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //ApplyGravity();
        //if (onGround)
        //{
        //    onGround = false;

        //}
    }

    private void DebugDrawLines()
    {
        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position + right * height * 2, Color.red);
    }
}
