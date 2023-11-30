using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float lifetime;
    
    public void Init(Vector3 force, float lt)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
        lifetime = lt;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime < 0) gameObject.SetActive(false);
    }
}
