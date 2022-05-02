using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 targetPos;
    public float speed = 2.0f;

    void Start()
    {
        targetPos = transform.position;

    }

    void Update()
    {

        float distance = transform.position.z + Camera.main.transform.position.z;
        Vector3 targetPos = Input.mousePosition;
        targetPos.z = Camera.main.nearClipPlane;
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);

        Vector3 followXonly = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, followXonly, speed * Time.deltaTime);
    }
}
