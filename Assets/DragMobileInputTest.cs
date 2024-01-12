using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMobileInputTest : MonoBehaviour
{
    public Transform cube;
    private Vector3 position;
    private float width;
    private float height;
    Vector2 startPosition, endPosition;
    string direction;
    bool stopTouch;
    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2"));
    }

    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
            }
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                
                endPosition = touch.position;
                float x = endPosition.x - startPosition.x;
                float y = endPosition.y - startPosition.y;
                //if (!stopTouch)
                //{
                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        direction = "Tappad";
                        Debug.Log(direction);

                    }
                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        direction = x > 0 ? "Right" : "Left";
                        Debug.Log(direction);
                    }
                    else
                    {
                        direction = y > 0 ? "Up" : "Down";
                        Debug.Log(direction + "y: " + y);
                    }
              //  }

                /*
                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;

                int h;
                int v;

                position = new Vector3(-pos.x, pos.y, 0.0f);
                Debug.Log(touch.radius);
                */

                /*
                 if (pos.x > 0)
                {
                    h = 1;
                    
                    Debug.Log("h: " + h);
                }
                if (pos.x < 0)
                {
                    h = -1;
                    Debug.Log("h: " + h);
                }
                if (pos.y > 0)
                {
                    v = 1;
                    Debug.Log("v: " + v);
                }
                if (pos.y < 0)
                {
                    v = -1;
                    Debug.Log("v: " + v);
                }
                */
                // Position the cube.
                //cube.position = position;
            }
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                
            }
            if (Input.touchCount == 2)
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    cube.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    cube.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }
}
