using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using ChatGPTWrapper;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PokemonCustomizationUI : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] TMP_InputField pokemonNameInput;
    [SerializeField] TMP_Dropdown type1Dropdown;
    [SerializeField] TMP_Dropdown type2Dropdown;
    [SerializeField] TMP_InputField animalNameInput;

    [Header("Preview")]
    [SerializeField] Image pokemonSprite;
    [SerializeField] TextMeshProUGUI previewText;

    [Header("Navigation")]
    [SerializeField] Button confirmButton;
    [SerializeField] Button backButton;
    [SerializeField] Button generateButton;
    [SerializeField] GameObject loadingIndicator;

    private PokemonType selectedType1;
    private PokemonType selectedType2;
    private string defaultSpritePathBack = "Pokemon/Sprites/sprite_-1547647098";
    private string defaultSpritePathFront = "Pokemon/Sprites/sprite_989815560";
    private const string CUSTOM_POKEMON_PATH = "Assets/Resources/CustomPokemon";
    [SerializeField] private ChatGPTConversation chatGPT;
    private PokemonData currentPokemonData;

    private void Start()
    {
        InitializeTypeDropdowns();
        SetupButtons();
        UpdatePreview();
        loadingIndicator.SetActive(false);

        // Initialize ChatGPT
        if (chatGPT != null)
        {
            chatGPT.Init();
            chatGPT.chatGPTResponse.AddListener(OnChatGPTResponse);
        }
        else
        {
            Debug.LogError("ChatGPTConversation component is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Clean up event listener
        if (chatGPT != null)
        {
            chatGPT.chatGPTResponse.RemoveListener(OnChatGPTResponse);
        }
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
        type1Dropdown.AddOptions(new List<string> { "None" }.Concat(typeOptions).ToList());
        type2Dropdown.AddOptions(new List<string> { "None" }.Concat(typeOptions).ToList());

        // Add listeners
        type1Dropdown.onValueChanged.AddListener((value) => {
            selectedType1 = value == 0 ? PokemonType.None : types[value - 1];
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
        generateButton.onClick.AddListener(OnGenerateClicked);
    }

    private void OnGenerateClicked()
    {
        if (string.IsNullOrWhiteSpace(animalNameInput.text))
        {
            // Show error message
            return;
        }

        if (chatGPT == null)
        {
            Debug.LogError("ChatGPTConversation component is not assigned!");
            return;
        }

        loadingIndicator.SetActive(true);
        generateButton.interactable = false;

        string prompt = $"Create a Pokemon based on this animal: {animalNameInput.text}. " +
                      "Format your response as a JSON object with these fields: " +
                      "name (string), type1 (string), type2 (string), description (string), " +
                      "moves (array of objects with name, type, power, accuracy, description)";

        chatGPT.SendToChatGPT(prompt);
    }

    public void OnChatGPTResponse(string response)
    {
        try
        {
            currentPokemonData = JsonUtility.FromJson<PokemonData>(response);

            // Update UI with generated data
            pokemonNameInput.text = currentPokemonData.name;
            selectedType1 = (PokemonType)System.Enum.Parse(typeof(PokemonType), currentPokemonData.type1);
            selectedType2 = (PokemonType)System.Enum.Parse(typeof(PokemonType), currentPokemonData.type2);

            // Update dropdowns to match selected types
            type1Dropdown.value = type1Dropdown.options.FindIndex(option => option.text == currentPokemonData.type1);
            type2Dropdown.value = type2Dropdown.options.FindIndex(option => option.text == currentPokemonData.type2);

            // Create and save new moves
            List<LearnableMove> learnableMoves = CreateNewMoves(currentPokemonData.moves);

            UpdatePreview();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating Pokemon: {e.Message}");
            // Show error message to user
        }
        finally
        {
            loadingIndicator.SetActive(false);
            generateButton.interactable = true;
            // Unsubscribe from the response event
            chatGPT.chatGPTResponse.RemoveListener(OnChatGPTResponse);
        }
    }

    private List<LearnableMove> CreateNewMoves(MoveData[] movesData)
    {
        List<LearnableMove> learnableMoves = new List<LearnableMove>();

        #if UNITY_EDITOR
        // Create directory if it doesn't exist
        string movesPath = "Assets/Resources/CustomMoves";
        if (!Directory.Exists(movesPath))
        {
            Directory.CreateDirectory(movesPath);
        }

        foreach (var moveData in movesData)
        {
            // Create new move asset
            MoveBase newMove = ScriptableObject.CreateInstance<MoveBase>();
            newMove.Name = moveData.name;
            newMove.Type = (PokemonType)System.Enum.Parse(typeof(PokemonType), moveData.type);
            newMove.Power = moveData.power;
            newMove.Accuracy = moveData.accuracy;
            newMove.Description = moveData.description;
            newMove.AlwaysHits = moveData.alwaysHits;
            newMove.PP = moveData.pp;
            newMove.Priority = moveData.priority;
            newMove.Category = (MoveCategory)System.Enum.Parse(typeof(MoveCategory), moveData.category);
            newMove.Target = (MoveTarget)System.Enum.Parse(typeof(MoveTarget), moveData.target);

            // Initialize empty effects
            newMove.Effects = new MoveEffects();
            newMove.Secondaries = new List<SecondaryEffects>();

            // Save move asset
            string safeName = string.Join("_", moveData.name.Split(Path.GetInvalidFileNameChars()));
            string assetPath = $"{movesPath}/{safeName}.asset";
            AssetDatabase.CreateAsset(newMove, assetPath);

            // Create learnable move
            LearnableMove learnableMove = new LearnableMove
            {
                Base = newMove,
                Level = 1
            };
            learnableMoves.Add(learnableMove);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        #endif

        return learnableMoves;
    }

    private void UpdatePreview()
    {
        // Update sprite
        Sprite sprite = Resources.Load<Sprite>(defaultSpritePathFront);
        if (sprite != null)
            pokemonSprite.sprite = sprite;

        // Update preview text
        string type2Text = selectedType2 == PokemonType.None ? "" : $" / {selectedType2}";
        string pokemonName = string.IsNullOrWhiteSpace(pokemonNameInput.text) ? "Who am I ?" : pokemonNameInput.text;
        previewText.text = $"Preview:\n{pokemonName}\n{selectedType1}{type2Text}";
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
        var initializer = FindFirstObjectByType<PlayerPartyInitializer>();
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
        pokemon.FrontSprite = Resources.Load<Sprite>(defaultSpritePathFront);
        pokemon.BackSprite = Resources.Load<Sprite>(defaultSpritePathBack);

        // Set default stats
        pokemon.MaxHp = 120;
        pokemon.Attack = 120;
        pokemon.Defense = 120;
        pokemon.SpAttack = 120;
        pokemon.SpDefense = 120;
        pokemon.Speed = 120;

        // Get moves from GPT response
        List<LearnableMove> learnableMoves = CreateNewMoves(currentPokemonData.moves);
        pokemon.LearnableMoves = learnableMoves;

        // Save the custom Pokemon as an asset
        SaveCustomPokemon(pokemon);

        return pokemon;
    }

    private void SaveCustomPokemon(PokemonBase pokemon)
    {
        #if UNITY_EDITOR
        // Create directory if it doesn't exist
        if (!Directory.Exists(CUSTOM_POKEMON_PATH))
        {
            Directory.CreateDirectory(CUSTOM_POKEMON_PATH);
        }

        // Create a unique filename based on the Pokemon's name
        string safeName = string.Join("_", pokemon.Name.Split(Path.GetInvalidFileNameChars()));
        string assetPath = $"{CUSTOM_POKEMON_PATH}/{safeName}.asset";

        // Save the asset
        AssetDatabase.CreateAsset(pokemon, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Saved custom Pokemon to: {assetPath}");
        #endif
    }
    void Update()
    {
        UpdatePreview();
    }

    // // this is for testing moves
    // private List<LearnableMove> GetRandomMovesForTypes(PokemonType type1, PokemonType type2)
    // {
    //     List<LearnableMove> moves = new List<LearnableMove>();
        
    //     // Load all moves from Resources
    //     MoveBase[] allMoves = Resources.LoadAll<MoveBase>("Moves");
        
    //     // Filter moves by type
    //     var typeMoves = allMoves.Where(m => m.Type == type1 || m.Type == type2).ToList();
        
    //     // Randomly select 4 moves
    //     int moveCount = Mathf.Min(4, typeMoves.Count);
    //     for (int i = 0; i < moveCount; i++)
    //     {
    //         int randomIndex = Random.Range(0, typeMoves.Count);
    //         LearnableMove move = new LearnableMove
    //         {
    //             Base = typeMoves[randomIndex],
    //             Level = 1
    //         };
    //         moves.Add(move);
    //         typeMoves.RemoveAt(randomIndex);
    //     }

    //     return moves;
    // }
}

[System.Serializable]
public class PokemonData
{
    public string name;
    public string type1;
    public string type2;
    public string description;
    public MoveData[] moves;
}

[System.Serializable]
public class MoveData
{
    public string name;
    public string type;
    public int power;
    public int accuracy;
    public string description;
    public bool alwaysHits;
    public int pp;
    public int priority;
    public string category; // "Physical", "Special", or "Status"
    public string target; // "Foe" or "Self"
} 