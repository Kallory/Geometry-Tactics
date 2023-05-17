using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script needs to be assigned to every character, the Turn manager adds the characters into a List and will determine
// the turn order, who's turn it is, etc. 
public class Character : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    float horizontal;
    float vertical;
    public float moveSpeed = 20.0f;

    // TODO: Decide if the stats are serialized Fields or not, initially.
    private int health;
    private int dexterity;
    private int speed;
    private int strength;
    private bool withinMovementConstraints = true;
    public TurnManager turnManager;
    public enum CharacterPhase
    {
        Idle,
        Movement,
        Fight
    }

    public CharacterPhase currentPhase;
    private Vector3 _inputs;
    public MovementRange movementRange; // movement range script
    EdgeCollider2D ringCollider;
    GameObject movementRangeObj; // movement range object
    private Vector3 center;
    private Vector3 characterPosition;
    private float diameter;
    private float CurrentRadius;
    private float circumference;
    int numPoints = 100; // Increase this for a more accurate circle


    void Start()
    {
        // currentPhase = CharacterPhase.Idle;
        turnManager = FindAnyObjectByType<TurnManager>();
        // TODO: assign stat values here or ensure that they are properly assigned, maybe pull from JSON file elsewhere for save/load functionality
        speed = 10;
        movementRangeObj = GameObject.Find("MovementRangeBlue1");
        center = movementRangeObj.transform.position;
        diameter = movementRangeObj.transform.localScale.x;
        CreateRingCollider();
    }

    void CreateRingCollider()
    {
        Vector2[] edgePoints = new Vector2[numPoints + 1];
        ringCollider = movementRangeObj.AddComponent<EdgeCollider2D>();
        for (int loop = 0; loop <= numPoints; loop++)
        {
            float angle = (Mathf.PI * 2.0f / numPoints) * loop;
            edgePoints[loop] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * diameter;
        }

        ringCollider.points = edgePoints;
        CurrentRadius = diameter;
    }

    public float GetSpeed
    {
        get { return speed; }
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log("current phase: " + this.name + " " + currentPhase);
        // make a key that bypasses everything and ends the turn no matter what
        // TODO: make this key kind of hard to press, maybe two keys so its not an accident
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // End the current turn talking to the TurnManager
            turnManager.SetEndMovementKey(true);
        }

        
        if (currentPhase == CharacterPhase.Movement && this.tag == "Player")
        {
            // Debug.Log("movementPhase");
            ProcessInput();
        }
        else if (currentPhase == CharacterPhase.Fight)
        {
            // TODO: Combat logic here
        }

        if (currentPhase == CharacterPhase.Movement && this.tag == "AI")
        {
            Debug.Log("movementPhase for AI");
            // TODO: AI movement Logic
        }
        else if (currentPhase == CharacterPhase.Fight)
        {
            // TODO: Combat logic here
        }
    }

    private void CheckBoundaries()
    {
        // If the character's distance from the center of the circle is less than or equal to
        // the radius, it can move
        // radius = 0;
        // if (radius >= 0)
        // {
        //     radius = 1;
        // }
    }

    private void ProcessInput()
    {
        this._inputs = Vector2.zero;
        this._inputs.x = Input.GetAxisRaw("Horizontal");
        this._inputs.y = Input.GetAxisRaw("Vertical");
        this._inputs = Vector2.ClampMagnitude(this._inputs, 1f);
    }

    private void FixedUpdate()
    {
        if (currentPhase == CharacterPhase.Movement)
        {
            //body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            //rigidBody.AddForce(this._inputs * Time.deltaTime * moveSpeed);
            rigidBody.velocity = new Vector2(this._inputs.x * moveSpeed, this._inputs.y * moveSpeed) * Time.deltaTime;
        }
        else if (currentPhase == CharacterPhase.Fight)
        {
            // TODO: Combat logic here
        }
    }
}
