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

    void Start()
    {
        PopulateCharacterList();
        // initialize current character to movement so that they can move
        characters[currentTurn].currentPhase = Character.CharacterPhase.Movement;
        
        for (int i = 0; i < characters.Count; i++)
        {
            Debug.Log("Character = " + characters[i]);
        }
        currentTurn = 0;
    }

    void Update()
    {
        // If they push a certain key/Button, movementPhase will end
        if (EndMovementKey == true)
        {
            characters[currentTurn].currentPhase = Character.CharacterPhase.Fight;
        }
    }

    public void SetEndMovementKey(bool setKey) {
        EndMovementKey = setKey;
    }
    void PopulateCharacterList()
    {
        characters.Clear(); // Clear the list before populating
        Character[] foundCharacters = FindObjectsOfType<Character>();
        foreach (Character character in foundCharacters)
        {
            characters.Add(character);
        }

        if (currentTurn < characters.Count)
        {

            currentTurn++;
        }
        else
        {
            currentTurn = 0;
        }
    }


}
