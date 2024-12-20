using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _camera;
    public float moveSpeed;
    public float scrollRate;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void Move(Vector2 dir)
    {
        direction = dir;
    }

    private void _ZoomCamera(float value){
        float resultantValue = Mathf.Clamp(
            _camera.orthographicSize + Time.deltaTime * this.scrollRate * value,
            5, 10);
        _camera.orthographicSize = resultantValue;
    }

    public void FocusOn(Vector3 location)
    {
        transform.position = location;
        _camera.orthographicSize = 1.75f;
    }

    public void Unfocus()
    {
        _camera.orthographicSize = 5.0f;
    }
}
