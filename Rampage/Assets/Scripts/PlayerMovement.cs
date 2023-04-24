using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    [Header("Character Movement Stats")]
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Animator playerAnimator;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        //playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void shootingMode()
    {

    }
    private void Move()
    {
        //Get Player Input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        //Move the player
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        cc.SimpleMove(-moveDirection * moveSpeed);

        //Rotate the player
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
        }

        //Trigger Animation
        if (moveDirection != Vector3.zero)
        {
            playerAnimator.SetBool("isRunning", true);
            Debug.Log("true");
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
            Debug.Log("false");
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
}
