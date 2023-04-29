using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private AIController aiController;

    [Header("Var")]
    [SerializeField] private float cameraSpeed = 30;
    [SerializeField] private float camMoveThreshold = 1;

    [Header("Limit")]
    [SerializeField] private Vector2 lowerLimit = new Vector2(30,20);
    [SerializeField] private Vector2 maxLimit = new Vector2(500,500);

    [Header("Storage")]
    [SerializeField] private Vector3 rayPos;
    [SerializeField] private RaycastHit rayhit;
    [SerializeField] private Vector2 moveDir;
    [SerializeField][Range(0,1)] private float stopLerp = 0.03f;
    [SerializeField][Range(0,1)] private float accelerationLerp = 0.3f;

    void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.instance.InMenu) return;
        CheckMoveCamera();

        if (Input.GetMouseButtonDown(0))
        {
            Selection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AskForAction();
        }

    }

    private void Selection()
    {

    }

    private void AskForAction()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get needed data
            rayhit = hit;
            rayPos = rayhit.point;

            // Check the hitted object
            //hit.transform;

            // Result
            aiController.MoveAgentsTo(rayPos);
        }
    }

    private void CheckMoveCamera()
    {
        //moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopLerp);
        Vector2 mousePosition = Input.mousePosition;

        if (mousePosition.x <= camMoveThreshold)
        {
            moveDir.x = Mathf.Lerp(moveDir.x, -1, accelerationLerp);
        }
        else if (mousePosition.x >= Screen.width - 1 - camMoveThreshold)
        {
            moveDir.x = Mathf.Lerp(moveDir.x, 1, accelerationLerp);
        }
        else
        {
            moveDir.x = Mathf.Lerp(moveDir.x, 0, stopLerp);
        }

        if (mousePosition.y <= camMoveThreshold)
        {
            moveDir.y = Mathf.Lerp(moveDir.y, -1, accelerationLerp);
        }
        else if (mousePosition.y >= Screen.height - 1 - camMoveThreshold)
        {
            moveDir.y = Mathf.Lerp(moveDir.y, 1, accelerationLerp);
        }
        else
        {
            moveDir.y = Mathf.Lerp(moveDir.y, 0, stopLerp);
        }

        // Move
        if (moveDir.magnitude > 0.01f) MoveCamera(moveDir);
    }

    private void MoveCamera(Vector2 dir)
    {
        //Cam forward and right vectors:
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        //Project forward and right vectors on the horizontal plane
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Direction in the world space
        Vector3 desiredMoveDirection = forward * dir.y + right * dir.x;

        //Apply the movement
        transform.Translate(desiredMoveDirection * cameraSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, lowerLimit.x, maxLimit.x);
        pos.z = Mathf.Clamp(pos.z, lowerLimit.y, maxLimit.y);
        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rayPos, 0.3f);
    }

}
