using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private CreatureGenerator fishGenerator;

    private DisplaySprite displayScript;

    // Dictionaries to map string names to sprites
    public Dictionary<string, Texture2D> WallSprites { get; private set; }
    public Dictionary<string, Texture2D> GlassSprites { get; private set; }
    public Dictionary<string, Texture2D> WaterSprites { get; private set; }
    public Dictionary<string, Texture2D> DirtSprites { get; private set; }
    public Dictionary<string, Texture2D> PlantsSprites { get; private set; }
    public Dictionary<string, Texture2D> RocksSprites { get; private set; }
    public Dictionary<string, Texture2D> SubstrateSprites { get; private set; }
    public Dictionary<string, Texture2D> BaseSprites { get; private set; }
    public Dictionary<string, Texture2D> LidSprites { get; private set; }
    public Dictionary<string, Texture2D> FishSprites { get; private set; }
    public Dictionary<string, Texture2D> StarfishSprites { get; private set; }
    public Dictionary<string, Texture2D> BuildingSprites { get; private set; }


    //-----------------------------------------
    // Old Colors
    Color32 oldbgWallColor1 = new Color32(143, 86, 59, 255);        // medium
    Color32 oldbgWallColor2 = new Color32(102, 57, 49, 255);        // dark
    Color32 oldbgWallAccent1 = new Color32(34, 32, 52, 255);        // very dark
    Color32 oldbgWallAccent2 = new Color32(217, 160, 102, 255);     // light

    Color32[] oldColors;

    // New Colors
    Color32 bgWallColor1;
    Color32 bgWallColor2;
    Color32 bgWallAccent1;
    Color32 bgWallAccent2;

    Color32[] newColors;

    //-----------------------------------------



    // Start is called before the first frame update
    void Start()
    {
        oldColors = new Color32[4] { oldbgWallColor1,  oldbgWallColor2,
                                   oldbgWallAccent1, oldbgWallAccent2 };

        displayScript = FindObjectOfType<DisplaySprite>();

        LoadTextures();
        CreateAquarium();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<Loader>().LoadNext();
        }
    }

    private void CreateAquarium()
    {
           // Choose a color scheme
           (bgWallColor1, bgWallColor2,
            bgWallAccent1, bgWallAccent2)
                = CreateColorScheme();

        newColors = new Color32[4] { bgWallColor1,  bgWallColor2,
                                   bgWallAccent1, bgWallAccent2 };

        // Create the background
        // Create the aquarium
        // Get the textures
        Texture2D[] textures;
        Texture2D[] plants;
        (textures, plants) = RandomTextures();

        // Create the fish
        Texture2D[] fishTextures;
        Vector3[] fishPositions;
        Vector3[] fishSize;
        (fishTextures, fishPositions, fishSize) = fishGenerator.GenerateFish(FishSprites);

        // Create the starfish
        Texture2D[] starfishTextures;
        Vector3[] starfishPositions;
        Vector3[] starfishSize;
        (starfishTextures, starfishPositions, starfishSize) = fishGenerator.GenerateStarfish(StarfishSprites);

        // Choose a building
        Texture2D[] building = new Texture2D[1] {
            BuildingSprites.Values.ElementAt(Random.Range(0, BuildingSprites.Count)) };

        // Graphics
        displayScript.UpdateSprites(textures, plants, building);
        displayScript.CreateFish(fishTextures, fishPositions, fishSize);
        displayScript.CreateStarfish(starfishTextures, starfishPositions, starfishSize);

    }


    private (Color32 bgWallColor1, Color32 bgWallColor2,
             Color32 bgWallAccent1, Color32 bgWallAccent2)
        CreateColorScheme()
    {
        float maxH = 360;
        float maxS = 100;
        float maxV = 100;

        (float, float, float) bgWallHSV1 = (Random.value,
                                            Random.Range(0.1f, 0.4f),
                                            Random.Range(0.25f, 0.3f));

        (float, float, float) bgWallHSV2;
        if (Random.value > 0.5f)
        {
            //Debug.Log("Split complementary");
            // Split complementary
            bgWallHSV2 = ((bgWallHSV1.Item1 + (135 / maxH)) % 1,
                                    bgWallHSV1.Item2 + (5 / maxS),
                                    bgWallHSV1.Item3 - (5 / maxV));
        }
        else
        {
            //Debug.Log("Analogous");
            // Analogous
            bgWallHSV2 = ((bgWallHSV1.Item1 + (30 / maxH)) % 1,
                        bgWallHSV1.Item2 + (5 / maxS),
                        bgWallHSV1.Item3 - (5 / maxV));
        }

        (float, float, float) bgWallHSVAccent1 = ((bgWallHSV1.Item1 + (225 / maxH)) % 1,
                                                  bgWallHSV1.Item2 + (10 / maxS),
                                                  bgWallHSV1.Item3 - (20 / maxV));

        (float, float, float) bgWallHSVAccent2 = ((bgWallHSV1.Item1 + (0 / maxH)) % 1,
                                                  bgWallHSV1.Item2 + (5 / maxS),
                                                  bgWallHSV1.Item3 + (20 / maxV));

        (float, float, float) bgOtherHSV = (bgWallHSV1.Item1 % 1,
                                            bgWallHSV1.Item2 + (5 / maxS),
                                            bgWallHSV1.Item3);


        Color32 bgWallColor1 = Color.HSVToRGB(bgWallHSV1.Item1, bgWallHSV1.Item2, bgWallHSV1.Item3);
        Color32 bgWallColor2 = Color.HSVToRGB(bgWallHSV2.Item1, bgWallHSV2.Item2, bgWallHSV2.Item3);
        Color32 bgWallAccent1 = Color.HSVToRGB(bgWallHSVAccent1.Item1, bgWallHSVAccent1.Item2, bgWallHSVAccent1.Item3);
        Color32 bgWallAccent2 = Color.HSVToRGB(bgWallHSVAccent2.Item1, bgWallHSVAccent2.Item2, bgWallHSVAccent2.Item3);
        Color32 bgOtherColor = Color.HSVToRGB(bgOtherHSV.Item1, bgOtherHSV.Item2, bgOtherHSV.Item3);

        return (bgWallColor1, bgWallColor2,
             bgWallAccent1, bgWallAccent2);
    }

    public (Texture2D[], Texture2D[]) RandomTextures()
    {
        // Get sprites for each part:
        //      Wall
        //      Glass
        //      Water
        //      Dirt
        //      Plants
        //      Rocks
        //      Substrate
        //      Base
        //      Lid

        // wall
        int wallIndex = Random.Range(0, WallSprites.Count);
        Texture2D wall = WallSprites.Values.ElementAt(wallIndex);

        // glass
        int glassIndex = Random.Range(0, GlassSprites.Count);
        Texture2D glass = GlassSprites.Values.ElementAt(glassIndex);

        // water
        int waterIndex = Random.Range(0, WaterSprites.Count);
        Texture2D water = WaterSprites.Values.ElementAt(waterIndex);

        // dirt
        int dirtIndex = Random.Range(0, DirtSprites.Count);
        Texture2D dirt = DirtSprites.Values.ElementAt(dirtIndex);

        // plants
        int plantsIndex = Random.Range(0, PlantsSprites.Count);
        Texture2D[] plants = new Texture2D[1] { PlantsSprites.Values.ElementAt(plantsIndex) };


        // rocks
        int rocksIndex = Random.Range(0, RocksSprites.Count);
        Texture2D rocks = RocksSprites.Values.ElementAt(rocksIndex);

        // substrate
        int substrateIndex = Random.Range(0, SubstrateSprites.Count);
        Texture2D substrate = SubstrateSprites.Values.ElementAt(substrateIndex);

        // base
        int baseIndex = Random.Range(0, BaseSprites.Count);
        Texture2D aquariumBase = BaseSprites.Values.ElementAt(baseIndex);

        // lid
        int lidIndex = Random.Range(0, LidSprites.Count);
        Texture2D lid = LidSprites.Values.ElementAt(lidIndex);

        // Combine each of the Textures into a texture array
        Texture2D[] textures = new Texture2D[8] { wall, glass, water, dirt, rocks, substrate, aquariumBase, lid };

        // Recolor each texture
        return Recolor(textures, plants);
    }

    private (Texture2D[], Texture2D[]) Recolor(Texture2D[] oldTextures, Texture2D[] oldplants)
    {
        Texture2D[] newTextures = new Texture2D[oldTextures.Length];

        for (int i = 0; i < newTextures.Length; i++)
        {
            // Create a new texture for each old texture
            newTextures[i] = new Texture2D(oldTextures[i].width, oldTextures[i].height);
            newTextures[i].filterMode = FilterMode.Point;

            // Change the colors in the new texture based on the palettes
            int y = 0;
            while (y < oldTextures[i].height)
            {
                int x = 0;
                while (x < oldTextures[i].width)
                {
                    Color32 currentColor = oldTextures[i].GetPixel(x, y);
                    // Check if the current color is in a palette,
                    // if so, then loop through the colors in the palette to find which ones to swap.
                    if (CompareAll(oldColors, currentColor))
                    {
                        // Swap palettes
                        for (int j = 0; j < oldColors.Length; j++)
                        {
                            if (CompareColors(currentColor, oldColors[j]))
                            {
                                // Get color from the selected color map
                                Color32 selectedColor = newColors[j];

                                // Set the pixel to be the selected color
                                newTextures[i].SetPixel(x, y, selectedColor);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Color not changed
                        newTextures[i].SetPixel(x, y, oldTextures[i].GetPixel(x, y));
                    }
                    ++x;
                }
                ++y;
            }
            newTextures[i].Apply();
        }

        Texture2D[] newPlants = new Texture2D[oldplants.Length];

        for (int i = 0; i < newPlants.Length; i++)
        {
            // Create a new texture for each old texture
            newPlants[i] = new Texture2D(oldplants[i].width, oldplants[i].height);
            newPlants[i].filterMode = FilterMode.Point;

            // Change the colors in the new texture based on the palettes
            int y = 0;
            while (y < oldplants[i].height)
            {
                int x = 0;
                while (x < oldplants[i].width)
                {
                    Color32 currentColor = oldplants[i].GetPixel(x, y);
                    // Check if the current color is in a palette,
                    // if so, then loop through the colors in the palette to find which ones to swap.
                    if (CompareAll(oldColors, currentColor))
                    {
                        // Swap palettes
                        for (int j = 0; j < oldColors.Length; j++)
                        {
                            if (CompareColors(currentColor, oldColors[j]))
                            {
                                // Get color from the selected color map
                                Color32 selectedColor = newColors[j];

                                // Set the pixel to be the selected color
                                newPlants[i].SetPixel(x, y, selectedColor);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Color not changed
                        newPlants[i].SetPixel(x, y, oldplants[i].GetPixel(x, y));
                    }
                    ++x;
                }
                ++y;
            }
            newPlants[i].Apply();
        }
        return (newTextures, newPlants);
    }

    // Load part textures/sprites from the resources folder
    private void LoadTextures()
    {
        // Construct each dictionary
        WallSprites = new Dictionary<string, Texture2D>();
        GlassSprites = new Dictionary<string, Texture2D>();
        WaterSprites = new Dictionary<string, Texture2D>();
        DirtSprites = new Dictionary<string, Texture2D>();
        PlantsSprites = new Dictionary<string, Texture2D>();
        RocksSprites = new Dictionary<string, Texture2D>();
        SubstrateSprites = new Dictionary<string, Texture2D>();
        BaseSprites = new Dictionary<string, Texture2D>();
        LidSprites = new Dictionary<string, Texture2D>();
        FishSprites = new Dictionary<string, Texture2D>();
        StarfishSprites = new Dictionary<string, Texture2D>();
        BuildingSprites = new Dictionary<string, Texture2D>();

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Wall/");
        foreach (Texture2D t in textures) { WallSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Glass/");
        foreach (Texture2D t in textures) { GlassSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Water/");
        foreach (Texture2D t in textures) { WaterSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Dirt/");
        foreach (Texture2D t in textures) { DirtSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Plants/");
        foreach (Texture2D t in textures) { PlantsSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Rocks/");
        foreach (Texture2D t in textures) { RocksSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Substrate/");
        foreach (Texture2D t in textures) { SubstrateSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Base/");
        foreach (Texture2D t in textures) { BaseSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Lid/");
        foreach (Texture2D t in textures) { LidSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Fish/");
        foreach (Texture2D t in textures) { FishSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Starfish/");
        foreach (Texture2D t in textures) { StarfishSprites[t.name] = t; }

        textures = Resources.LoadAll<Texture2D>("Buildings/");
        foreach (Texture2D t in textures) { BuildingSprites[t.name] = t; }
    }

    /// <summary>
    /// Returns true if the color is included in the array of colors
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <returns></returns>
    public static bool CompareAll(Color32[] colorArray, Color32 color2)
    {
        for (int i = 0; i < colorArray.Length; i++)
        {
            if (CompareColors(colorArray[i], color2))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CompareColors(Color32 color1, Color32 color2)
    {
        int rdiff = Mathf.Abs(color1.r - color2.r);
        int gdiff = Mathf.Abs(color1.g - color2.g);
        int bdiff = Mathf.Abs(color1.b - color2.b);
        int adiff = Mathf.Abs(color1.a - color2.a);

        return rdiff + gdiff + bdiff + adiff < 5;
    }
}
