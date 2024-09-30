using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _camera;
    public float moveSpeed;
    public float scrollRate;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
        transform.position = new Vector3(7,5, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
             _TranslateCamera(0);
        }
        if (Input.GetKey(KeyCode.A)){
             _TranslateCamera(1);
        }
        if (Input.GetKey(KeyCode.D)){
             _TranslateCamera(2);
        }
        if(Input.GetKey(KeyCode.S)){
             _TranslateCamera(3);
        }
    }

    private void _TranslateCamera(int dir){
        int localMaxX = 13;
        int localMaxY = 20;

        if(dir == 0){
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Min(transform.position.y + Time.deltaTime*this.moveSpeed, localMaxY),
                transform.position.z);
        } else if (dir == 1){
            transform.position = new Vector3(
                Mathf.Max(7,transform.position.x - Time.deltaTime * this.moveSpeed),
                transform.position.y, 
                transform.position.z);
        } else if (dir == 2){
            transform.position = new Vector3(
                Mathf.Min(transform.position.x + Time.deltaTime * this.moveSpeed, localMaxX),
                transform.position.y, 
                transform.position.z);
        } else if (dir == 3){
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Max(transform.position.y - Time.deltaTime*this.moveSpeed, 5),
                transform.position.z);
        }
    }

    private void _ZoomCamera(float value){
        float resultantValue = Mathf.Clamp(
            _camera.orthographicSize + Time.deltaTime * this.scrollRate * value,
            5, 10);
        _camera.orthographicSize = resultantValue;
    }

    public void OnGUI()
    {
        if(Event.current.type == EventType.ScrollWheel)
            // do stuff with  Event.current.delta
            _ZoomCamera(Event.current.delta.y);
    }
}
