using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PokemonCustomizationUI : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] TMP_InputField pokemonNameInput;
    [SerializeField] TMP_Dropdown type1Dropdown;
    [SerializeField] TMP_Dropdown type2Dropdown;

    [Header("Preview")]
    [SerializeField] Image pokemonSprite;
    [SerializeField] TextMeshProUGUI previewText;

    [Header("Navigation")]
    [SerializeField] Button confirmButton;
    [SerializeField] Button backButton;

    private PokemonType selectedType1;
    private PokemonType selectedType2;
    private string defaultSpritePath = "Pokemon/Sprites/sprite_-1547647098";

    private void Start()
    {
        InitializeTypeDropdowns();
        SetupButtons();
        UpdatePreview();
    }

    private void InitializeTypeDropdowns()
    {
        // Get all Pokemon types except None
        var types = System.Enum.GetValues(typeof(PokemonType))
            .Cast<PokemonType>()
            .Where(t => t != PokemonType.None)
            .ToList();

        // Clear and populate dropdowns
        type1Dropdown.ClearOptions();
        type2Dropdown.ClearOptions();

        var typeOptions = types.Select(t => t.ToString()).ToList();
        type1Dropdown.AddOptions(typeOptions);
        type2Dropdown.AddOptions(new List<string> { "None" }.Concat(typeOptions).ToList());

        // Add listeners
        type1Dropdown.onValueChanged.AddListener((value) => {
            selectedType1 = types[value];
            UpdatePreview();
        });

        type2Dropdown.onValueChanged.AddListener((value) => {
            selectedType2 = value == 0 ? PokemonType.None : types[value - 1];
            UpdatePreview();
        });
    }

    private void SetupButtons()
    {
        confirmButton.onClick.AddListener(OnConfirmClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void UpdatePreview()
    {
        // Update sprite
        Sprite sprite = Resources.Load<Sprite>(defaultSpritePath);
        if (sprite != null)
            pokemonSprite.sprite = sprite;

        // Update preview text
        string type2Text = selectedType2 == PokemonType.None ? "" : $" / {selectedType2}";
        previewText.text = $"Preview:\n{pokemonNameInput.text}\n{selectedType1}{type2Text}";
    }

    private void OnConfirmClicked()
    {
        if (string.IsNullOrWhiteSpace(pokemonNameInput.text))
        {
            // Show error message
            return;
        }

        // Create custom Pokemon
        PokemonBase customPokemon = CreateCustomPokemon();
        
        // Save to PlayerPartyInitializer
        var initializer = FindObjectOfType<PlayerPartyInitializer>();
        if (initializer != null)
        {
            initializer.SetInitialPokemon(customPokemon);
        }

        // Load next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
    }

    private void OnBackClicked()
    {
        // Load previous scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private PokemonBase CreateCustomPokemon()
    {
        PokemonBase pokemon = ScriptableObject.CreateInstance<PokemonBase>();
        
        // Set basic info
        pokemon.Name = pokemonNameInput.text;
        pokemon.Type1 = selectedType1;
        pokemon.Type2 = selectedType2;

        // Load default sprite
        pokemon.FrontSprite = Resources.Load<Sprite>(defaultSpritePath);
        pokemon.BackSprite = Resources.Load<Sprite>(defaultSpritePath);

        // Set default stats
        pokemon.MaxHp = 50;
        pokemon.Attack = 50;
        pokemon.Defense = 50;
        pokemon.SpAttack = 50;
        pokemon.SpDefense = 50;
        pokemon.Speed = 50;

        // Get random moves based on types
        List<LearnableMove> learnableMoves = GetRandomMovesForTypes(selectedType1, selectedType2);
        pokemon.LearnableMoves = learnableMoves;

        return pokemon;
    }

    private List<LearnableMove> GetRandomMovesForTypes(PokemonType type1, PokemonType type2)
    {
        List<LearnableMove> moves = new List<LearnableMove>();
        
        // Load all moves from Resources
        MoveBase[] allMoves = Resources.LoadAll<MoveBase>("Moves");
        
        // Filter moves by type
        var typeMoves = allMoves.Where(m => m.Type == type1 || m.Type == type2).ToList();
        
        // Randomly select 4 moves
        int moveCount = Mathf.Min(4, typeMoves.Count);
        for (int i = 0; i < moveCount; i++)
        {
            int randomIndex = Random.Range(0, typeMoves.Count);
            LearnableMove move = new LearnableMove
            {
                Base = typeMoves[randomIndex],
                Level = 1
            };
            moves.Add(move);
            typeMoves.RemoveAt(randomIndex);
        }

        return moves;
    }
} 