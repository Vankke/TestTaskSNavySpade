using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* THIS IS COMMON MOVEMENT CONTROLLER FOR EVERY MOVING ENTITY IN THE GAME
 */

public class MovementController : MonoBehaviour
{
    [SerializeField] Vector3 Target;
    [SerializeField] float Speed;                   //ENTITY MOVE SPEED 
    [SerializeField] float StoppingDist;            //STOPPING DISTANCE TO PREVENT JITTERING
    [SerializeField] bool UpdateRotation = true;    //SHOULD ENTITY FACE THE WAY IT IS GOING?
    CharacterController CController;
    /// <summary>
    /// Set target for entity to move to
    /// </summary>
    /// <param name="targ">
    /// </param>
    public void SetTarget(Vector3 targ)
    {
        targ.y = transform.position.y;
        Target = targ;
    }
    private void Awake()
    {
        CController = GetComponent<CharacterController>();
        SetTarget(transform.position);  //SET TARGET TO POSITION TO AVOID CHARACTER RUNNING AT 0,0,0
    }
    private void Update()
    {
        if (Vector3.Distance(Target, transform.position) > StoppingDist)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget() 
    {
        var MoveDirection = Target - transform.position;
        CController.Move(MoveDirection.normalized * Time.deltaTime * Speed);
        if(UpdateRotation)
            transform.forward = Vector3.Lerp(transform.forward, MoveDirection.normalized, 0.3f);
    }
    /// <summary>
    /// Is Entity currently moving?
    /// </summary>
    /// <returns>
    /// </returns>
    public bool IsMoving()
    {
        bool returnedBool = false;
        if(CController.velocity.magnitude > 0.2f && Vector3.Distance(Target, transform.position) > StoppingDist)
        {
            returnedBool = true;
        }
        return returnedBool;
    } 
}
