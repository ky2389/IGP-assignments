using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<int> possiblePokemonIds; // List of possible Pokemon IDs for this area
    [SerializeField] List<float> encounterRates; // Chance of encountering each Pokemon

    public async Task<Pokemon> GetRandomWildPokemon()
    {
        // Get a random Pokemon ID based on encounter rates
        int randomIndex = Random.Range(0, possiblePokemonIds.Count);
        int pokemonId = possiblePokemonIds[randomIndex];

        // Get the Pokemon base from the database
        PokemonBase pokemonBase = await PokemonDatabase.Instance.GetPokemon(pokemonId);
        
        if (pokemonBase != null)
        {
            // Create a new Pokemon instance with random level (e.g., 5-10)
            int level = Random.Range(5, 11);
            Pokemon pokemon = new Pokemon(pokemonBase, level);
            pokemon.Init();
            return pokemon;
        }

        Debug.LogError($"Failed to load Pokemon with ID {pokemonId}");
        return null;
    }
}
