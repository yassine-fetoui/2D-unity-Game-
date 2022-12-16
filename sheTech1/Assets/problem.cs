using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class problem : MonoBehaviour
{
    // Start is called before the first frame update
    private player p ;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 d = p.GetComponent<player>().Direction;
        transform.position -= d;


    }
}
