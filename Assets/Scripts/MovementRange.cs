using UnityEngine;

public class MovementRange : MonoBehaviour
{
    [SerializeField] private float movementRange = 1f;
    [SerializeField] private GameObject movementRing;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: put in update, have it disappaer when turn = false. Somehow associate it with a single game object. Add bounds (maybe just making it a collider will work in this instance?)
        ScaleMovementRing();
    }

    // Update the size of the movement ring based on the character's movement range
    private void ScaleMovementRing()
    {
        float scaleFactor = movementRange / (movementRing.transform.localScale.x / 2f);
        movementRing.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}