using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // This script handles the turn order. Will calculate initiate based on several factors.
    // TODO: Calculate initiative properly
    public List<Character> characters = new List<Character>();
    private int currentTurn;
    private bool EndMovementKey;
    private bool EndTurnKey;
    private bool EndIdlePhaseKey;

    void Start()
    {
        EndMovementKey = false;
        EndTurnKey = false;
        currentTurn = 0;
        PopulateCharacterList();
        // initialize current character to movement so that they can move
        characters[currentTurn].currentPhase = Character.CharacterPhase.Movement;

        for (int i = 0; i < characters.Count; i++)
        {
            Debug.Log("Character = " + characters[i]);
        }

    }

    void Update()
    {
        // If they push a certain key/Button, movementPhase will end
        if (EndMovementKey == true)
        {
            characters[currentTurn].currentPhase = Character.CharacterPhase.Fight;
            EndIdlePhaseKey = false;
            EndMovementKey = false;
            EndTurnKey = false;
        }

        if (EndTurnKey == true)
        {
            characters[currentTurn].currentPhase = Character.CharacterPhase.Idle;


            if (currentTurn < characters.Count)
            {
                currentTurn++;
            }
            else
            {
                currentTurn = 0;
            }

            characters[currentTurn].currentPhase = Character.CharacterPhase.Movement;

            EndIdlePhaseKey = false;
            EndMovementKey = false;
            EndTurnKey = false;
        }
    }

    public void SetEndMovementKey(bool setKey)
    {
        EndMovementKey = setKey;
    }

    public void SetEndTurnKey(bool setKey)
    {

    }

    public Character.CharacterPhase getCurrentPhase()
    {
        return characters[currentTurn].currentPhase;
    }

    public Character GetCharacter()
    {
        return characters[currentTurn];
    }
    void PopulateCharacterList()
    {
        characters.Clear(); // Clear the list before populating
        Character[] foundCharacters = FindObjectsOfType<Character>();
        foreach (Character character in foundCharacters)
        {
            characters.Add(character);
        }

    }


}
