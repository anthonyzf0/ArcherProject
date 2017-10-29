using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour {
    
    // Use this for initialization
    public float sensitivity;
    public float maxLookAngle = 89;
    private Vector3 rotaionLock = new Vector3(1, 1, 0.9f);

    private static float lastZ = 0, currentZ = 0;

    // Use this for initialization
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

    }

    //Sets the z-rotation values
    public static void setZValues(float newZ)
    {
        currentZ = newZ - lastZ;
        lastZ = newZ;
    }
    
    float scale(float f) {

        if (f > 180) return scale(f - 360);
        if (f < -180) return scale(f + 360);
        if (Mathf.Abs(f) < 0.1) return 0;
        return f;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = (Vector3.left * Input.GetAxis("Mouse Y")) + (Vector3.up * Input.GetAxis("Mouse X"));
        mouse *= sensitivity;

        Vector3 newRotation = transform.localRotation.eulerAngles + mouse;
        
        if (!(newRotation.x < maxLookAngle || newRotation.x > 360 - maxLookAngle))
        {
            newRotation.x -= mouse.x;
        }

        //Do rotation fixes
        if (currentZ != 0)
        {
            currentZ = scale(currentZ);
            currentZ *= 0.9f;
            newRotation.z = currentZ;
        }

        transform.localRotation = Quaternion.Euler(newRotation);
        
    }
}
