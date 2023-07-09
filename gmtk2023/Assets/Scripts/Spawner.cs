using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public CharacterController characterPrefab;
    public int maxSpawnedCount = 5;
    private List<CharacterController> characters = new();

    private IEnumerator Start()
    {
        for (var i = 0; i < maxSpawnedCount; i++)
        {
            InstantiateCharacter();
            yield return new WaitForSeconds(1);
        }
    }

    private void OnCharacterDie()
    {
        InstantiateCharacter();
    }

    private void InstantiateCharacter()
    {
        var character = Instantiate(characterPrefab, transform);
        character.GetComponent<SpriteRenderer>().DOFade(0, 0);
        character.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        character.OnDie += OnCharacterDie;
    }
}
