using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* THIS SCRIPT CONTROLS ENEMY ENTITIES
 */
public class EnemyController : MonoBehaviour
{

    MovementController mover;
    PlayerController PC;
    GameController GC;
    
    private void Awake()
    {
        mover = GetComponent<MovementController>();
        SetRandomTarget();
        PC = FindObjectOfType<PlayerController>();
        GC = FindObjectOfType<GameController>();
    }
    private void Update()
    {
        if (!mover.IsMoving())
        {
            SetRandomTarget();
        }     
    }
    void SetRandomTarget()
    {
        var randTarget = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        mover.SetTarget(randTarget);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Player")
        {
            PC.TakeDamage(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GEM")
        {
            OnEnemyHitGem(other.gameObject);
        }
    }

    void OnEnemyHitGem(GameObject gem)
    {
        GC.DestroyGem(gem);
    }
}
