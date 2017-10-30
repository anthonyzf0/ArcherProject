using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Rotation stuff
    private Dictionary<string, GravityInfo> rotationData = new Dictionary<string, GravityInfo>();

    //Position info
    public int maxJumps = 3;
    private int jumps = 3;
    public float jumpPower;
    public string currentDerection = "GravityDown";

    //Movement data
    public float movementSpeed = 70;
    public float gravityPower = 70;
    
    // Use this for initialization
    void Start ()
    {
        rotationData.Add("GravityDown", new GravityInfo(Vector3.down, new Vector3(0, 0, 0)));
        rotationData.Add("GravityUp", new GravityInfo(Vector3.up, new Vector3(-180, 0, 0)));

        rotationData.Add("GravityForward", new GravityInfo(Vector3.right, new Vector3(0, 0, 90)));
        rotationData.Add("GravityBack", new GravityInfo(Vector3.left, new Vector3(0, 0, -90)));

        rotationData.Add("GravityLeft", new GravityInfo(Vector3.back, new Vector3(90, 0, 0)));
        rotationData.Add("GravityRight", new GravityInfo(Vector3.forward, new Vector3(-90, 0, 0)));

    }
	
	// Update is called once per frame
	void Update () {
        
        //Jumps
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            GetComponent<Rigidbody>().velocity = -rotationData[currentDerection].gravity * jumpPower;
        }

        //Movement
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            movement -= Camera.main.transform.right;
        if (Input.GetKey(KeyCode.D))
            movement += Camera.main.transform.right;
        if (Input.GetKey(KeyCode.W))
            movement += Camera.main.transform.forward;
        if (Input.GetKey(KeyCode.S))
            movement -= Camera.main.transform.forward;
        
        movement.Scale(rotationData[currentDerection].inverseGravity);
        transform.position += movement.normalized * Time.deltaTime * movementSpeed;
    }
    

    private void OnTriggerEnter(Collider collision)
    {
        jumps = maxJumps;

        //If you hit a block that changes rotationa and its not the same rotation
        if (rotationData.ContainsKey(collision.gameObject.tag) && 
            currentDerection!= collision.gameObject.tag)
        {
            //Reset derections
            currentDerection = collision.gameObject.tag;

            //Save current derection
            Vector3 forward = Camera.main.transform.forward;
            
            //Do rotation of block
            transform.rotation = Quaternion.Euler(rotationData[currentDerection].rotation);

            //Re-set Current rotation so that camera faces same derection
            Camera.main.transform.forward = forward;
            
            //set how the z-axis changed so that we can smoothly translate
            FirstPersonLook.setZValues(Camera.main.transform.localEulerAngles.z);

            //Set gravity
            GetComponent<ConstantForce>().force = rotationData[currentDerection].gravity * gravityPower;
        }
    }


}

class GravityInfo{

    public Vector3 gravity, rotation, inverseGravity;

    public GravityInfo (Vector3 gravity, Vector3 rotation)
    {
        this.gravity = gravity;
        this.rotation = rotation;

        //Used in calculations
        this.inverseGravity = (new Vector3( (gravity.x == 0) ? 1 : 0, 
                                            (gravity.y == 0) ? 1 : 0, 
                                            (gravity.z == 0) ? 1 : 0));
        
    }


}