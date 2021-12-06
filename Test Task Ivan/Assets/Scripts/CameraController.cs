using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  EASY WAY TO DO THIS IS TO PARENT CAMERA TO THE PLAYER
 *  HOWEVER HERE WE CAN ADD MOVEMENT EASE TO THE CAMERA AND AVOID ERRORS IF WE WANT PLAYER OBJECT TO BE DESTROYED AT SOME POINT
 */ 
public class CameraController : MonoBehaviour
{
    Transform followedObject;
    Vector3 offset;
    [SerializeField] float FollowEase = 1; // VALUE FROM  near 0 to 1;
    private void Awake()
    {
        followedObject = FindObjectOfType<PlayerController>().transform;
        offset = transform.position - followedObject.transform.position;
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, followedObject.transform.position + offset, FollowEase);
    }
}
