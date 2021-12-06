using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* THIS SCRIPT CONTROLS PLAYABLE CHARACTER
 */
 

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask MouseRaycastMask;
    Camera cam;
    MovementController mover;
    GameController GC;
    InterfaceController IC;
    Animator animator;

    [SerializeField] GameObject ShieldObj;
    [SerializeField] GameObject PointObject;       //HIGHLIGHTS THE POINT THAT PLAYER CLICKED
    [SerializeField] int HP;
    [SerializeField] int MaxHP;                    //MODIFY THIS VARIABLE TO CHANGE MAX HP
    [SerializeField] int PointsForGem;             //MODIFY THIS VARIABLE TO CHANGE POINTS FOR GEM
    [SerializeField] float InvulnerabilityTime;    //MODIFY THIS VARIABLE TO CHANGE TIME OF INVULNERABILITY

    const string RUN = "IsRun";

    bool isInvulnerable;

    private void Awake()
    {
        cam = Camera.main;
        mover = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        GC = FindObjectOfType<GameController>();
        IC = FindObjectOfType<InterfaceController>();
        HP = MaxHP;
    }
    void Update()
    {
        if (GC.GameStarted)
        {
            MouseMethods();
            ControlAnimation();
        }
    }
    
    void MouseMethods()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, MouseRaycastMask))
            {
                mover.SetTarget(hitInfo.point);
                PointObject.transform.position = hitInfo.point;
            }
        }
    }
    void ControlAnimation()
    {
        var isMoving = mover.IsMoving();
        if (animator.GetBool(RUN) != isMoving)
        {
            animator.SetBool(RUN, isMoving);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var tag = other.tag;
        if(tag == "GEM")
        {
            CollectGem(other.gameObject);
        }
    }

    public void CollectGem(GameObject gem)
    {
        HP = Mathf.Clamp(HP + 1, 0, MaxHP);
        GC.CurrentScore += PointsForGem;
        GC.DestroyGem(gem);
        IC.UpdateHearts(HP);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "ENEMY")
        {
            var control = hit.gameObject.GetComponent<EnemyController>();
            TakeDamage(control);
        }
    }

    
    public void TakeDamage(EnemyController enemy)
    {
        if (isInvulnerable)
        {
            return;
        }
        HP -= 1;
        IC.UpdateHearts(HP);
        GC.DestroyEnemy(enemy.gameObject);
        if(HP <= 0)
        {
            mover.SetTarget(transform.position); //Stop movement
            GC.EndGameMethod();
        }
        else
        {
            BecomeInvulnerable();
        }
    }
    public void BecomeInvulnerable()
    {
        isInvulnerable = true;
        ShieldObj.SetActive(true);
        StartCoroutine(WaitForInvulnerability());
    }

    IEnumerator WaitForInvulnerability()
    {
        yield return new WaitForSeconds(InvulnerabilityTime);
        isInvulnerable = false;
        ShieldObj.SetActive(false);
    }
}
