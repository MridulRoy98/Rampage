using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator playerAnimator;
    private CharacterController cc;

    [Header("Character Movement Stats")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float detectionRadius = 5f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private bool isMoving()
    {
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Debug.Log("can move");
            return true;
        }
        else
        {
            return false;
        }
    }
    void Update()
    {
        if (isMoving())
        {
            Move();
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
            ShootingMode();
        }
    }

    private void ShootingMode()
    {
        if (Physics.CheckSphere(transform.position, 5))
        {
            
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, detectionRadius);
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , rotateSpeed);
        }

        //Trigger Animation
        if(moveDirection != Vector3.zero)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else playerAnimator.SetBool("isRunning", false);
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
}
