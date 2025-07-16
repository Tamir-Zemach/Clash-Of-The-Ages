using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MouseRayCaster : SceneAwareMonoBehaviour<MouseRayCaster>
{
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private float raycastDistance = 100f;

    private GameObject currentHover;
    private Camera rayCamera;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        rayCamera = rayCamera != null ? rayCamera : Camera.main;
    }

    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        HandleHover();
    }

    private void HandleHover()
    {
        GameObject hitObject = GetHitObject();
        if (hitObject != currentHover)
        {
            currentHover = hitObject;
        }
    }

    public GameObject GetHitObject() => GetHit()?.collider.gameObject;

    public RaycastHit? GetHit()
    {
        if (rayCamera == null) return null;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

        return Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayers) ? hit : null;
    }

    public IEnumerator WaitForMouseClick(Action<RaycastHit> onValidHit = null, Action onMissedClick = null)
    {
        yield return new WaitForSeconds(0.1f); // buffer before click

        bool waitingForClick = true;

        while (waitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = GetHit();

                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    onValidHit?.Invoke(hit.Value);
                    yield break;
                }

                if (!hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    onMissedClick?.Invoke();
                    yield break;
                }
            }

            yield return null;
        }
    }
}