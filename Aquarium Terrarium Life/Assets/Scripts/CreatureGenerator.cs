using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    //-----------------------------------------
    // Starting Fish Colors
    Color32 oldMainFishColor = new Color32(223, 113, 38, 255);
    Color32 oldAccentFishColor = new Color32(217, 160, 102, 255);
    Color32[] startingFishColors;

    // New Fish Colors
    Color32 mainFishColor;
    Color32 accentFishColor;
    Color32[] fishColors;

    //-----------------------------------------

    private void Start()
    {
        startingFishColors = new Color32[2] { oldMainFishColor, oldAccentFishColor };

    }

    public (Texture2D[] fishTextures, Vector3[] fishPositions, Vector3[] fishSize) GenerateFish(Dictionary<string, Texture2D> FishSprites)
    {
        int fishCount = Random.Range(4, 8);

        Texture2D[] fishTextures = new Texture2D[fishCount];
        Vector3[] fishPositions = new Vector3[fishCount];
        Vector3[] fishSize = new Vector3[fishCount];


        for (int i = 0; i < fishCount; i++)
        {
            // Determine fish position
            float xPos = Random.Range(-3.2f, 3.4f);
            float yPos = Random.Range(-0.7f, 2);
            fishPositions[i] = new Vector3(xPos, yPos, 0);

            // Determine fish size
            float scale = Random.Range(2, 5);
            fishSize[i] = new Vector3(scale, scale, 1);

            // Determine fish texture
            fishTextures[i] = FishSprites.Values.ElementAt(
                Random.Range(0, FishSprites.Count));

        }

        fishTextures = ColorFish(fishTextures);

        return (fishTextures, fishPositions, fishSize);

    }

    public (Texture2D[] fishTextures, Vector3[] fishPositions, Vector3[] fishSize) GenerateStarfish(Dictionary<string, Texture2D> StarfishSprites)
    {
        int starfishCount = Random.Range(1, 3);

        Texture2D[] starfishTextures = new Texture2D[starfishCount];
        Vector3[] starfishPositions = new Vector3[starfishCount];
        Vector3[] starfishSize = new Vector3[starfishCount];

        for (int i = 0; i < starfishCount; i++)
        {
            // Determine fish position
            float xPos = Random.Range(-3.2f, 3.4f);
            float yPos = Random.Range(-0.7f, 2);
            starfishPositions[i] = new Vector3(xPos, yPos, 0);

            // Determine fish size
            float scale = Random.Range(2, 5);
            starfishSize[i] = new Vector3(scale, scale, 1);

            // Determine fish texture
            starfishTextures[i] = StarfishSprites.Values.ElementAt(
                Random.Range(0, StarfishSprites.Count));

        }

        starfishTextures = ColorFish(starfishTextures);

        return (starfishTextures, starfishPositions, starfishSize);

    }



    private Texture2D[] ColorFish(Texture2D[] startingTextures)
    {
        if (startingFishColors == null)
        {
            startingFishColors = new Color32[2] { oldMainFishColor, oldAccentFishColor };
        }
        float maxH = 360;
        float maxS = 100;
        float maxV = 100;

        Texture2D[] newTextures = new Texture2D[startingTextures.Length];

        for (int i = 0; i < newTextures.Length; i++)
        {
            // Create a new texture for each old texture
            newTextures[i] = new Texture2D(startingTextures[i].width, startingTextures[i].height);
            newTextures[i].filterMode = FilterMode.Point;

            // Determine the new colors of the fish
            (float, float, float) color1 = (Random.value,
                                Random.Range(0.8f, 1),
                                Random.Range(0.7f, 0.9f));
            (float, float, float) color2 = ((color1.Item1 + (6 / maxH)) % 1,
                                            color1.Item2 - (30 / maxS),
                                            color1.Item3 - (2 / maxV));

            mainFishColor = Color.HSVToRGB(color1.Item1, color1.Item2, color1.Item3);
            accentFishColor = Color.HSVToRGB(color2.Item1, color2.Item2, color2.Item3);
            fishColors = new Color32[2] { mainFishColor, accentFishColor };

            // Change the colors in the new texture based on the palettes
            int y = 0;
            while (y < startingTextures[i].height)
            {
                int x = 0;
                while (x < startingTextures[i].width)
                {
                    Color32 currentColor = startingTextures[i].GetPixel(x, y);
                    // Check if the current color is in a palette,
                    // if so, then loop through the colors in the palette to find which ones to swap.
                    if (MainController.CompareAll(startingFishColors, currentColor))
                    {
                        // Swap palettes
                        for (int j = 0; j < startingFishColors.Length; j++)
                        {
                            if (MainController.CompareColors(currentColor, startingFishColors[j]))
                            {
                                // Get color from the selected color map
                                Color32 selectedColor = fishColors[j];

                                // Set the pixel to be the selected color
                                newTextures[i].SetPixel(x, y, selectedColor);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Color not changed
                        newTextures[i].SetPixel(x, y, startingTextures[i].GetPixel(x, y));
                    }
                    ++x;
                }
                ++y;
            }
            newTextures[i].Apply();
        }
        return newTextures;

    }




}
