using UnityEngine;

public class MovementRange : MonoBehaviour
{
    [SerializeField] private GameObject characterObject;
    [SerializeField] private float movementRange = 1f;
    [SerializeField] private GameObject movementRing;
    [SerializeField] private Character character;

    // Start is called before the first frame update
    void Start()
    {
        // character = character.GetComponent<Character>();
        // TODO: put in update, have it disappear when turn = false. Somehow associate it with a single game object. Add bounds (maybe just making it a collider will work in this instance?)
        // TODO: Also, the width of the line of the ring needs to stay the same as it scales up, or maybe not? Test both ways. 
        ScaleMovementRing();
    }

    // Update the size of the movement ring based on the character's movement range
    private void ScaleMovementRing()
    {   
        // TODO: Replace movementRange with a stat from the Character
        // TODO: MovementRange sprite needs to be based on the stat, can't expand dynamically in Unity
        float scaleFactor = movementRange / (movementRing.transform.localScale.x / 2f);
        movementRing.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}