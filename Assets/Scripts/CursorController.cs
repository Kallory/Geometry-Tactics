using UnityEngine;
using TMPro;

public class CursorController : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask selectableLayer; // Layer for selectable objects
    public Texture2D cursor;
    private string objectInfo;
    public TMP_Text infoText;

    private void Awake()
    {
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Start()
    {
        mainCamera = Camera.main;
        objectInfo = "Object: " + gameObject.name;
    }

    void Update()
    {
        // Update the cursor position to follow the mouse
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, selectableLayer);

        if (hit.collider != null)
        {
            HoverInfo hoverInfo = hit.collider.gameObject.GetComponent<HoverInfo>();
            Debug.Log("Selected object: " + hoverInfo);
            if (hoverInfo != null)
            {
                // Get the object's information and display it in the UI Text component
                infoText.text = hoverInfo.objectInfo;
                Debug.Log("Selected object: " + infoText.text);
            }
        }
        else
        {
            // Clear the UI Text when not hovering over an object
            infoText.text = "";
        }

        // // Check for mouse click
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("Left mouse button = " + Input.GetMouseButtonDown(0));
        //     RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, selectableLayer);

        //     if (hit.collider != null)
        //     {

        //         GameObject selectedObject = hit.collider.gameObject;
        //         Debug.Log("Selected object: " + selectedObject.name);

        //         // Perform actions with the selected object (e.g., show info, select for movement, etc.)
        //     }
        // }
        // else if (Input.GetMouseButtonDown(1))
        // {
        //     Debug.Log("Right mouse button = " + Input.GetMouseButtonDown(1));

        // }
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
}