using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform objectToFollow;
    private Vector3 objectOffset;

    private void Awake()
    {
        this.objectToFollow = FindObjectOfType<Player>().transform;
        objectOffset = this.transform.position - objectToFollow.position;
    }

    private void LateUpdate()
    {
        if (objectToFollow != null)
        {
            this.transform.position = objectToFollow.position + objectOffset;
        }
    }

    public void ChangeFollow(Transform newFollow)
    {
        objectToFollow = newFollow;
        this.transform.position = objectToFollow.position + objectOffset;
    }
}