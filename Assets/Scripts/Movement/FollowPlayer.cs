using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform pTransform;
    public float speed=0.2f;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position=Vector3.Lerp(transform.position, pTransform.position, speed);
    }
}
