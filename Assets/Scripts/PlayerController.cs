using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private AIManager aiController;

    [Header("Var")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float cameraSpeed = 30;
    [SerializeField] private float camMoveThreshold = 1;
    [SerializeField] private bool middleClickPressed;

    [Header("Limit")]
    [SerializeField] private Vector2 lowerLimit = new Vector2(30,20);
    [SerializeField] private Vector2 maxLimit = new Vector2(500,500);

    [Header("Storage")]
    [SerializeField] private Vector3 rayPos;
    [SerializeField] private RaycastHit rayhit;
    [SerializeField] private Vector2 moveDir;
    [SerializeField][Range(0,1)] private float stopLerp = 0.03f;
    [SerializeField][Range(0,1)] private float accelerationLerp = 0.3f;

    [Header("Storage Selection")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private bool selecting;
    [SerializeField] private Vector3 startSelectionPos;
    [SerializeField] private Vector3 endSelectionPos;

    void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.instance.InMenu) return;
        CheckMoveCamera();

        if (Input.GetMouseButtonDown(0))
        {
            StartSelection();
        }
        else if(selecting)
        {
            UpdateSelection();
            if (Input.GetMouseButtonUp(0))
            {
                EndSelection();
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            AskForAction();
        }

        if (Input.GetMouseButtonDown(2))
        {
            middleClickPressed = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            middleClickPressed = false;
        }
    }

    // SELECTION
    private void StartSelection()
    {
        RaycastHit hit = GetMouseRaycast();
        startSelectionPos = endSelectionPos = hit.point;
        selecting = true;
    }

    private void UpdateSelection()
    {
        RaycastHit hit = GetMouseRaycast();
        endSelectionPos = hit.point;
    }

    private void EndSelection()
    {
        selecting = false;

        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        Vector3 center = (startSelectionPos + endSelectionPos) * 0.5f;
        Vector3 size = startSelectionPos - endSelectionPos;
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y) + 5;
        size.z = Mathf.Abs(size.z);

        RaycastHit[] result = Physics.BoxCastAll(center, size*0.5f, forward, Quaternion.identity, 150, playerLayerMask);

        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log($"Raycasted {result[i].transform.name}");
        }
    }

    // ACITON
    private void AskForAction()
    {
        RaycastHit hit = GetMouseRaycast();
        rayhit = hit;
        rayPos = rayhit.point;

        // Check the hitted object
        if(hit.transform.gameObject.layer != 3)
        {
            if (hit.transform.TryGetComponent(out Ressource ressource))
            {
                aiController.MoveAgentsTo(ressource);
            }
        }
        else // TERRAIN
        {
            aiController.MoveAgentsTo(rayPos);
        }
    }

    private RaycastHit GetMouseRaycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            return hit;
        }
        else
        {
            Debug.LogError("Ray hitted nothing");
            return new RaycastHit();
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

        if (selecting)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(startSelectionPos, 0.3f);
            Gizmos.DrawSphere(endSelectionPos, 0.3f);

            Vector3 camRot = cam.transform.eulerAngles;
            camRot.x = 0;
            camRot.z = 0;
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(camRot), Vector3.one);
            Gizmos.matrix = matrix;

            Vector3 center = (startSelectionPos + endSelectionPos) * 0.5f;
            Vector3 size = startSelectionPos - endSelectionPos;
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);
            size.z = Mathf.Abs(size.z);

            Gizmos.DrawWireCube(center, size);
        }
    }

}
