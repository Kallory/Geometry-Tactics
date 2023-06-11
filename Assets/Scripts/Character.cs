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

    /************************
     *                      *
     *        STATS         *
     *                      *
    /************************/
    // TODO: Default values for each character. 
    private int health;
    private int dexterity;
    private int speed;
    private int strength;
    private float coneAngle;
    private float angleStep;
    // TODO: the following 2 should not be serialized fields. 
    [SerializeField] private float attackRange; // this is based on the weapon. Maybe every character could have a different base attackRange, like their arm lengths are different.

    // TODO: numAttackMoves is derived from other, more fundamental stats
    [SerializeField] private int numAttackMoves;
    // TODO: Refactor stats into enums or something else? maybe an object of their own?
    // public enum CharacterStats
    // {

    // }
    // public enum CharacterSkills
    // {

    // }
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
    private EdgeCollider2D ringCollider;
    public GameObject movementRangeObj;
    private Vector2 center;
    private float diameter;

    private Vector2 characterPosition;
    private float CurrentRadius;
    private float circumference;
    int numTotalCirclePoints = 100; // Increase this for a more accurate circle
    int numberOfPoints = 50;
    private Vector2[] conePoints;
    private float startingAngle;

    void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        // TODO: Refactor tags into a component of strings so that I can use multiple tags on one object
        // Or I suppose I could keep the existing tag system and just use the component. Basically
        // the default stats will be different depending on the character. This is an opportunity for 
        // a production hack
        // TODO: assign stat values here or ensure that they are properly assigned, maybe pull from JSON file elsewhere for save/load functionality
        health = 10;
        dexterity = 10;
        strength = 10;
        speed = 10;
        conePoints = new Vector2[numberOfPoints];
        coneAngle = 90 * Mathf.Deg2Rad;

    }

    void Awake()
    {
        center = movementRangeObj.transform.position;
        diameter = movementRangeObj.transform.localScale.x;
        ringCollider = null;
        CreateRingCollider(center, diameter, movementRangeObj, ringCollider);
    }

    void CreateRingCollider(Vector2 center, float diameter, GameObject movementRange, EdgeCollider2D ringCollider)
    {
        Vector2[] edgePoints = new Vector2[numTotalCirclePoints + 1];
        ringCollider = movementRange.AddComponent<EdgeCollider2D>();
        for (int i = 0; i <= numTotalCirclePoints; i++)
        {
            float angle = (Mathf.PI * 2.0f / numTotalCirclePoints) * i;
            edgePoints[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * diameter;
        }

        ringCollider.points = edgePoints;
        Debug.Log("ring collider - " + ringCollider.name);
        CurrentRadius = diameter;
    }

    public float GetSpeed
    {
        get { return speed; }
    }

    void Update()
    {
        // Debug.Log("current phase: " + this.name + " " + currentPhase);
        // TODO: make a key that bypasses everything and ends the turn no matter what
        // TODO: make this key kind of hard to press, maybe two keys so its not an accident
        if (Input.GetKeyDown(KeyCode.Return) && this.tag == "Player" && currentPhase == CharacterPhase.Movement)
        {
            // End the current turn by talking to the TurnManager
            turnManager.SetEndMovementKey(true);
        }
        if (Input.GetKeyDown(KeyCode.A) && this.tag == "Player" && currentPhase == CharacterPhase.Fight && this.numAttackMoves > 0)
        {
            // Attempt to attack
            // If attackRange <= distance between player and target, actively attacking = true
            // else alert that there is no target
            // should a target be designated or can a player hit anything? I'd like a damageable environment
            // so maybe it can attack anything? And everything will have HP? Kind of like 7dtd, and so
            // certain things will have such high HP/Def it's not worth trying to attack them
            // so if attackRange <= distance between player and anything, anything gets damaged
            // god fuck this is gonna be a complex piece of code when I'm done.
            // probably won't do this but maybe have an energy system to allow moving/fighting in same phase? idk i dont like it. 
            // TODO: Test difference between fluid attack and transition to fight phase adding a "done moving" button and then fight? start with the more fluid approach

            // From chatGPT
            /*
            This combat system seems interesting, and has potential to offer a deep strategic gameplay. 
            A few things to start with:
            1. **Implement the base attack system**: Start with the attack button press, the decrease of "attackMoves", and the automatic ending of a turn when there are no more moves left. This is the core of your combat system and you need to get this working first.
            2. **Basic hit calculation**: For now, just check if the target is within the "attackRange". If it is, apply some damage. You don't need to worry about timing, poses, or environment objects yet. Just get the basic hit calculation working.
            3. **Adding timing**: Implement the mouse swipe we talked about earlier, and add a timing element to your attacks. The damage calculation now has to take this timing into account.
            4. **Poses and defensive stats**: Now that you have a functioning attack system, you can start thinking about defense. Implement the different poses and their effect on "attackMoves". Also, implement the chance to dodge, block, or counter based on defensive stats.
            5. **Environment objects**: Lastly, make your attack hit all damageable objects within the attack range. This might require some changes to your hit calculation, as you now need to check all objects within a certain range, instead of just one target.
            As you're implementing these features, always keep in mind that you're working on a prototype. Don't strive for perfect balance or depth yet. Instead, try to get a playable version of your game as quickly as possible. Once you have that, you can start iterating on your mechanics and adding depth.
            One thing you haven't mentioned is how you're going to handle animations and visual feedback for the players. These are important elements of any combat system, as they make the combat feel more satisfying and help the players understand what's happening. For now, you can use simple placeholder animations and effects, but you should plan to improve these as you develop your game.
            */
            numAttackMoves--;
            Debug.Log("num attack moves = " + numAttackMoves);
            ProcessInputAttack();
        }

        if (Input.GetKeyDown(KeyCode.A) && this.tag == "Player" && currentPhase == CharacterPhase.Fight && this.numAttackMoves <= 0 || ((Input.GetKeyDown(KeyCode.KeypadEnter)) && (Input.GetKeyDown(KeyCode.LeftControl))))
        {
            // end turn
            turnManager.SetEndTurnKey(true);
            Debug.Log("It is now " + turnManager.GetCharacter() + "'s turn");
        }

        if (currentPhase == CharacterPhase.Movement && this.tag == "Player")
        {
            // Debug.Log("movementPhase");
            ProcessInputMovement();
            Debug.Log("StartingAngle = " + startingAngle);
        }

        if (currentPhase == CharacterPhase.Movement && this.tag == "AI")
        {
            Debug.Log("movementPhase for AI");
            // TODO: AI movement Logic
            // AI needs to detect the closest enemy and move towards it until it hits a boundary or the enemy. 
            // If it's near the boundary, end the turn. Else it will attack, move to AI attack phase.
            Character nearestCharacter = null;
            float minDistance = Mathf.Infinity; // Start with infinity, so that any distance is smaller
            Vector2 currentPosition = this.transform.position;

            foreach (Character character in turnManager.playerCharacters)
            {
                // Calculate the distance from the AI character to the current player character
                float distance = Vector2.Distance(currentPosition, character.transform.position);

                // If this distance is smaller than the current minimum, update the minimum
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCharacter = character;
                }
            }
            Vector2 direction = (nearestCharacter.transform.position - this.transform.position).normalized;
            this._inputs = Vector2.zero;
            this._inputs.x = direction.x;
            this._inputs.y = direction.y;
            this._inputs = Vector2.ClampMagnitude(this._inputs, 1f);

        }
        else if (currentPhase == CharacterPhase.Fight && this.tag == "AI")
        {
            Debug.Log("combatPhase for AI");
            // TODO: AI Combat logic here
        }
    }

    private void ProcessInputMovement()
    {
        this._inputs = Vector2.zero;
        this._inputs.x = Input.GetAxisRaw("Horizontal");
        this._inputs.y = Input.GetAxisRaw("Vertical");
        this._inputs = Vector2.ClampMagnitude(this._inputs, 1f);
        Debug.Log("input" + this._inputs);

        // can also do this to play the right animation
        if (this._inputs.x == 0 && this._inputs.y > 0)
        {
            startingAngle = 90f * Mathf.Deg2Rad; // N
        }
        else if (this._inputs.x > 0 && this._inputs.y > 0)
        {
            startingAngle = 135f * Mathf.Deg2Rad; // NE
        }
        else if (this._inputs.x > 0 && this._inputs.y == 0)
        {
            startingAngle = 180f * Mathf.Deg2Rad; // E
        }
        else if (this._inputs.x > 0 && this._inputs.y < 0)
        {
            startingAngle = 225f * Mathf.Deg2Rad; // SE
        }
        else if (this._inputs.x == 0 && this._inputs.y < 0)
        {
            startingAngle = 270f * Mathf.Deg2Rad; // S
        }
        else if (this._inputs.x < 0 && this._inputs.y < 0)
        {
            startingAngle = 315f * Mathf.Deg2Rad; // SW
        }
        else if (this._inputs.x < 0 && this._inputs.y == 0)
        {
            startingAngle = 0f * Mathf.Deg2Rad; // E
        }
        else if (this._inputs.x < 0 && this._inputs.y > 0)
        {
            startingAngle = 45 * Mathf.Deg2Rad; // NE
        }
    }

    void OnDrawGizmos()
    {
        if (Input.GetKeyDown(KeyCode.A) && this.tag == "Player" && currentPhase == CharacterPhase.Fight && this.numAttackMoves > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

    private void ProcessInputAttack()
    {
        // if (this._inputs != Vector3.zero) // Make sure there is some input
        //{   
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            // Process hit entities
        }

        // PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        // Debug.Log("ProcessInputAttackBeingCalled");
        // // coneAngle will be decided by the weapon.
        // float angleStep = coneAngle / numberOfPoints; // coneAngle is the length of the arc in radians. Higher numberOfPoints for smoother arc
        // Vector2[] conePoints = new Vector2[numberOfPoints + 2];
        // conePoints[0] = new Vector2(this.transform.position.x, this.transform.position.y);
        // for (int i = 0; i <= numberOfPoints; i++)
        // {
        //     float angle = startingAngle + i * angleStep;
        //     Debug.Log("Angle = " + angle);
        //     // attack range is essentially the radius, based on the weapon range
        //     conePoints[i + 1] = new Vector2(this.transform.position.x + Mathf.Cos(angle) * attackRange, this.transform.position.y + Mathf.Sin(angle) * attackRange);
        // }

        // collider.SetPath(0, conePoints);
        // collider.offset = new Vector2(6.61f, 2.9f);
        // Vector3 characterPos = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        // for (int i = 0; i < conePoints.Length - 1; i++)
        // {
        //     Debug.DrawLine(characterPos + new Vector3(conePoints[i].x, conePoints[i].y, 0), characterPos + new Vector3(conePoints[i + 1].x, conePoints[i + 1].y, 0), Color.red, 10.0f);

        // }
        // Debug.DrawLine(characterPos + new Vector3(conePoints[conePoints.Length - 1].x, conePoints[conePoints.Length - 1].y, 0), characterPos + new Vector3(conePoints[0].x, conePoints[0].y, 0), Color.red, 2.0f);

        // }
    }

    private void FixedUpdate()
    {
        if (currentPhase == CharacterPhase.Movement && this.tag == "Player")
        {
            //body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            //rigidBody.AddForce(this._inputs * Time.deltaTime * moveSpeed);
            rigidBody.velocity = new Vector2(this._inputs.x * moveSpeed, this._inputs.y * moveSpeed) * Time.deltaTime;
        }
        else if (currentPhase == CharacterPhase.Fight && this.tag == "Player")
        {
            rigidBody.velocity = new Vector2(0, 0);
            // TODO: Combat logic here
        }

        // AI stuff
        if (currentPhase == CharacterPhase.Movement && this.tag == "AI")
        {
            //body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            //rigidBody.AddForce(this._inputs * Time.deltaTime * moveSpeed);
            rigidBody.velocity = new Vector2(this._inputs.x * moveSpeed, this._inputs.y * moveSpeed) * Time.deltaTime;
        }
        else if (currentPhase == CharacterPhase.Fight && this.tag == "AI")
        {
            rigidBody.velocity = new Vector2(0, 0);
            // TODO: Combat logic here
        }
    }
}
