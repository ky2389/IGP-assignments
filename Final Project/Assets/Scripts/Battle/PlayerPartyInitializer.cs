using UnityEngine;

public class PlayerPartyInitializer : MonoBehaviour
{
    [SerializeField] PokemonBase[] starterPokemon;
    [SerializeField] int[] starterLevels;

    void Start()
    {
        var party = GetComponent<PokemonParty>();
        for (int i = 0; i < starterPokemon.Length; i++)
        {
            party.AddPokemon(new Pokemon(starterPokemon[i], starterLevels[i]));
        }
    }
} 