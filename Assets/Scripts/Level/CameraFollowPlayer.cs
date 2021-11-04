using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 followBase;
    private Transform objectToFollow;
    private Vector3 objectOffset;

    public void AcquirePlayer()
    {
        this.objectToFollow = FindObjectOfType<Player>().transform;
        this.transform.position = objectToFollow.position + followBase;
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