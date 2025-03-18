using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterController characterController;
    //player move speed
    private float speed=6.0f;
    private float mouseSensitivity=3.5f;

    //Camera container
    Transform cameraTrans;
    float cameraPitch=0.0f;
    float gravityValue=Physics.gravity.y;
    float jumpHeight=-2f;
    float currentYVelocity;
    [SerializeField]
    Transform gunPoint;    
    void Start()
    {
        characterController=GetComponent<CharacterController>();
        cameraTrans=Camera.main.transform;
        //lock camera
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta= new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y"));
        transform.Rotate(Vector3.up,mouseDelta.x*mouseSensitivity);
        cameraPitch-=mouseDelta.y*mouseSensitivity;
        cameraPitch=Mathf.Clamp(cameraPitch,-90.0f,90.0f);
        cameraTrans.localEulerAngles=Vector3.right*cameraPitch;

        Vector3 move=transform.rotation*new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        if(characterController.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                currentYVelocity+=Mathf.Sqrt(jumpHeight*2f*gravityValue);
            }
            else
            {
                currentYVelocity=-0.5f;
            }
        }
        else{
            currentYVelocity+=gravityValue*Time.deltaTime;
        }
        move.y=currentYVelocity;
        characterController.Move(move*speed*Time.deltaTime);
        if (Input.GetMouseButtonUp(0))
        {
            //shoot
            SpawnManager.instance.SpawnBullets(gunPoint.position, cameraTrans.rotation  );
        }

    }
    private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Enemy"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
