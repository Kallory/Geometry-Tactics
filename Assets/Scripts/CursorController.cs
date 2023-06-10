using UnityEngine;
using TMPro;

public class CursorController : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask selectableLayer; // Layer for selectable objects
    public Texture2D cursor;
    private string objectInfo;
    public TMP_Text hoverInfoText;
    public TMP_Text turnInfoText;
    TurnManager turnManager;

    private void Awake()
    {
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
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
                hoverInfoText.text = hoverInfo.objectInfo;
                Debug.Log("Selected object: " + hoverInfoText.text);
            }
        }
        else
        {
            // Clear the UI Text when not hovering over an object
            hoverInfoText.text = "";
        }

        if (turnManager.getCurrentPhase() == Character.CharacterPhase.Movement && turnManager.GetCharacter().tag == "Player")
        {
            turnInfoText.text = "Press enter to end move phase or ctrl+enter to end turn";
        }
        else if (turnManager.getCurrentPhase() == Character.CharacterPhase.Fight && turnManager.GetCharacter().tag == "Player")
        {
            turnInfoText.text = "Attack an enemy with A or press enter to end turn";
        }
        else if (turnManager.getCurrentPhase() == Character.CharacterPhase.Idle && turnManager.GetCharacter().tag == "Player")
        {
            turnInfoText.text = "Enemy turn";
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