using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // This script handles the turn order. Will calculate initiate based on several factors.
    // TODO: Calculate initiative properly
    public List<Character> characters = new List<Character>();
    private int currentTurn;
    private bool endKey;

    void Start()
    {
        PopulateCharacterList();
        for (int i = 0; i < characters.Count; i++)
        {
            Debug.Log("Character = " + characters[i]);
        }
        currentTurn = 0;
    }

    void Update()
    {
        // initialize current character to movement so that they can move
        characters[currentTurn].currentPhase = Character.CharacterPhase.Movement;

        // If they push a certain key/Button, movementPhase will end
        if (endKey == true) {
            characters[currentTurn].currentPhase = Character.CharacterPhase.Fight;
        }
    }

    void PopulateCharacterList()
    {
        characters.Clear(); // Clear the list before populating
        Character[] foundCharacters = FindObjectsOfType<Character>();
        foreach (Character character in foundCharacters)
        {
            characters.Add(character);
        }

        if (currentTurn < characters.Count) {
            
            currentTurn++;
        } else {
            currentTurn = 0;
        }
    }


}
