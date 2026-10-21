using UnityEngine;

public class Orbit : MonoBehaviour
{

    public Transform center;
    public float orbitSpeed = 20f;
    public float spinSpeed = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(center != null)
        {
            // Orbit around the center
            transform.RotateAround(center.position, center.up, orbitSpeed * Time.deltaTime);
        }

        // Spin on its own axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime,0f, Space.Self);
    }
}
