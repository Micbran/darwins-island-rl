using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteraction : MonoBehaviour
{
    private Transform baseTransform;
    [SerializeField] private LayerMask layerToSearch;

    private void Awake()
    {
        this.baseTransform = this.transform;
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.Period))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.baseTransform.position, 1.0f, this.layerToSearch);
            foreach (Collider hit in hitColliders)
            {
                LevelChange levelChange = hit.GetComponent<LevelChange>();
                if (levelChange)
                {
                    if (GameManager.Instance.Floor > 6)
                    {
                        GameManager.Instance.EndGame();
                    }
                    else
                    {
                        GameManager.Instance.TransitionLevel();
                    }
                }
            }
        }
    }
}
