using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    protected Animator animator;

    [SerializeField]
    private Transform rightHandObj = null;
    [HideInInspector]
    public bool rightHandOn = false;
    
    private bool ikActive = false;
    private Transform lefttHandObj = null;
    private Vector3 offset = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIK(bool active, Vector3 newOffset, Transform target = null)
    {
        ikActive = active;
        lefttHandObj = target;
        offset = newOffset;
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the right hand target position and rotation, if one has been assigned
                if (lefttHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    Vector3 pos = lefttHandObj.position + offset;
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, pos);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(pos - transform.position));

                    animator.SetLookAtWeight(1, 1, 0, 0);
                    animator.SetLookAtPosition(lefttHandObj.position + offset);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }

            if (rightHandOn && rightHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(rightHandObj.position - transform.position));
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }
}
