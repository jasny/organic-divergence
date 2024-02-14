using UnityEngine;
using System.Collections;

public class CameraTap : MonoBehaviour
{
    public float tapZoom = 0.95f; // How much the camera zooms in on tap
    public float tapSpeed = 5f; // Speed of the zoom motion
    private float originalZoom;
    private bool isTapping = false;

    private Camera _camera;
        
    void Start()
    {
        _camera = GetComponent<Camera>();
        originalZoom = _camera.orthographicSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTapping)
        {
            StartCoroutine(TapCamera());
        }
    }

    IEnumerator TapCamera()
    {
        isTapping = true;
        float targetZoom = originalZoom * tapZoom;

        // Zoom in
        while (_camera.orthographicSize > targetZoom)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, targetZoom, tapSpeed * Time.deltaTime);
            yield return null;
        }

        // Zoom out
        while (_camera.orthographicSize < originalZoom)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, originalZoom, tapSpeed * Time.deltaTime);
            yield return null;
        }

        isTapping = false;
    }
}
