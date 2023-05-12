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
    private bool withinDistance = true;
    public enum CharacterPhase
    {
        Idle,
        Movement,
        Fight
    }

    public CharacterPhase currentPhase;
    private Vector3 _inputs;
    public MovementRange movementRange;

    void Start()
    {
        currentPhase = CharacterPhase.Idle;
        // TODO: assign stat values here or ensure that they are properly assigned, maybe pull from JSON file elsewhere for save/load functionality
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Calculate distance from center of character's MovementRange to get the 
        // max distance that the character can move in a turn
        CheckBoundaries();

        if (withinDistance) {
            
        }
        if (currentPhase == CharacterPhase.Movement)
        {
            ProcessInput();
        }
        else if (currentPhase == CharacterPhase.Fight)
        {
            // TODO: Combat logic here
        }
    }

    private void CheckBoundaries()
    {
        
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
