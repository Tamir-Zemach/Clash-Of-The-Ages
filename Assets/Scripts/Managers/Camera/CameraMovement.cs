using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : SceneAwareMonoBehaviour<CameraMovement>
{
    private CinemachineSplineDolly _dollyCart;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float edgeThreshold = 50f;
    [SerializeField] private float pathLength = 1f; // Set to 1 for normalized mode; adjust if using distance-based position

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        _dollyCart = GameObject.FindFirstObjectByType<CinemachineSplineDolly>();
    }

    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        EdgeScrollWithMouse();
        EdgeScrollWithKeyboard();
    }


    private void EdgeScrollWithMouse()
    {
        if (_dollyCart == null) return;
        float mouseX = Input.mousePosition.x;
        float newPosition = _dollyCart.CameraPosition;

        if (mouseX < edgeThreshold)
        {
            newPosition -= moveSpeed * Time.deltaTime;
        }
        else if (mouseX > Screen.width - edgeThreshold)
        {
            newPosition += moveSpeed * Time.deltaTime;
        }

        _dollyCart.CameraPosition = Mathf.Clamp(newPosition, 0f, pathLength);
    }

    private void EdgeScrollWithKeyboard()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            float newPosition = _dollyCart.CameraPosition + horizontalInput * moveSpeed * Time.deltaTime;
            _dollyCart.CameraPosition = Mathf.Clamp(newPosition, 0f, pathLength);
        }
    }
}
