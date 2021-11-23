using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySprite : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSprite;

    private GameObject aquarium_go;

    [SerializeField]
    private GameObject prefabBubbles;

    [SerializeField]
    private BoxCollider bubbleCollider;

    void Awake()
    {
        // Instantiate the character gameobject based on the prefab
        aquarium_go = Instantiate(prefabSprite, Vector3.zero, Quaternion.identity);

        if (prefabSprite == null) { Debug.LogError("There is no prefabSprite"); }
    }

    public void CreateFish(Texture2D[] fishTextures, Vector3[] fishPositions, Vector3[] fishSize)
    {
        // Create standard rect and pivot.
        // The rect size is based on the first texture, but all should be the same.
        Rect standardRect = new Rect(0, 0, fishTextures[0].width, fishTextures[0].height);
        Vector2 standardPivot = new Vector2(0.5f, 0.5f);

        // For each fish
        for (int i = 0; i < fishPositions.Length; i++)
        {
            // Create gameobject
            GameObject fish_go = new GameObject();
            fish_go.name = "fish " + fishTextures[i].name;
            // Create sprite renderer
            SpriteRenderer fish_sr = fish_go.AddComponent<SpriteRenderer>();
            fish_sr.sortingOrder = 6;

            // Add fish sprite to sprite renderer
            fish_sr.sprite = Sprite.Create(fishTextures[i], standardRect, standardPivot);

            // Set fish position and scale
            fish_go.transform.position = fishPositions[i];
            fish_go.transform.localScale = fishSize[i];

            // Add fish movement script to the fish
            _ = fish_go.AddComponent<FishMovement>();

            // Add bubbles particle system gameobject to the same location
            GameObject bubbles = Instantiate(prefabBubbles, fish_go.transform);
            ParticleSystem bubbles_ps = bubbles.GetComponent<ParticleSystem>();
            bubbles_ps.trigger.AddCollider(bubbleCollider);


        }
    }

    public void CreateStarfish(Texture2D[] starfishTextures, Vector3[] starfishPositions, Vector3[] starfishSize)
    {
        // Create standard rect and pivot.
        // The rect size is based on the first texture, but all should be the same.
        Rect standardRect = new Rect(0, 0, starfishTextures[0].width, starfishTextures[0].height);
        Vector2 standardPivot = new Vector2(0.5f, 0.5f);

        // For each fish
        for (int i = 0; i < starfishPositions.Length; i++)
        {
            // Create gameobject
            GameObject starfish_go = new GameObject();
            starfish_go.name = "starfish " + starfishTextures[i].name;
            // Create sprite renderer
            SpriteRenderer starfish_sr = starfish_go.AddComponent<SpriteRenderer>();
            starfish_sr.sortingOrder = 8;

            // Add starfish sprite to sprite renderer
            starfish_sr.sprite = Sprite.Create(starfishTextures[i], standardRect, standardPivot);

            // Set fish position and scale
            starfish_go.transform.position = starfishPositions[i];
            starfish_go.transform.localScale = starfishSize[i];
            starfish_go.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
/*            // Add fish movement script to the fish
            _ = starfish_go.AddComponent<FishMovement>();*/
/*
            // Add bubbles particle system gameobject to the same location
            GameObject bubbles = Instantiate(prefabBubbles, starfish_go.transform);
            ParticleSystem bubbles_ps = bubbles.GetComponent<ParticleSystem>();
            bubbles_ps.trigger.AddCollider(bubbleCollider);*/


        }
    }

    public void UpdateSprites(Texture2D[] textures, Texture2D[] plants, Texture2D[] building)
    {
        // Create a new instantiation of the wizard sprite prefab
        SpriteRenderer[] spriteRenderers = aquarium_go.GetComponentsInChildren<SpriteRenderer>();

        // Create standard rect and pivot.
        // The rect size is based on the first texture, but all should be the same.
        Rect standardRect = new Rect(0, 0, textures[0].width, textures[0].height);
        Vector2 standardPivot = new Vector2(0.5f, 0.5f);

        // Set each sprite renderer to the appropriate sprite
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr.name.Contains("Wall")) 
            { 
                sr.sprite = Sprite.Create(textures[0], standardRect, standardPivot);
                continue;
            }
            if (sr.name.Contains("Glass"))
            {
                sr.sprite = Sprite.Create(textures[1], standardRect, standardPivot);
                continue;
            }
            if (sr.name.Contains("Water"))
            {
                sr.sprite = Sprite.Create(textures[2], standardRect, standardPivot);
                continue;
            }
            if (sr.name.Contains("Dirt"))
            {
                sr.sprite = Sprite.Create(textures[3], standardRect, standardPivot);

                // slightly transform the sprite left or right
                Vector3 pos = sr.transform.position;
                pos.x += Random.Range(-0.1f, 0.1f);
                sr.transform.position = pos;

                continue;
            }
            if (sr.name.Contains("Plants"))
            {
                sr.sprite = Sprite.Create(plants[0], standardRect, standardPivot);

                // slightly transform the sprite left or right
                Vector3 pos = sr.transform.position;
                pos.x += Random.Range(-2.5f, 2.5f);
                sr.transform.position = pos;

                continue;
            }
            if (sr.name.Contains("Rocks"))
            {
                sr.sprite = Sprite.Create(textures[4], standardRect, standardPivot);

                // slightly transform the sprite left or right
                Vector3 pos = sr.transform.position;
                pos.x += Random.Range(-0.4f, 0.2f);
                pos.y += Random.Range(-0.1f, 0.1f);
                sr.transform.position = pos;

                continue;
            }
            if (sr.name.Contains("Building"))
            {
                sr.sprite = Sprite.Create(building[0], standardRect, standardPivot);

                // slightly transform the sprite left or right
                Vector3 pos = sr.transform.position;
                pos.x += Random.Range(-2f, 0);
                sr.transform.position = pos;

                continue;
            }
            if (sr.name.Contains("Substrate"))
            {
                sr.sprite = Sprite.Create(textures[5], standardRect, standardPivot);
                continue;
            }
            if (sr.name.Contains("Base"))
            {
                sr.sprite = Sprite.Create(textures[6], standardRect, standardPivot);
                continue;
            }
            if (sr.name.Contains("Lid"))
            {
                sr.sprite = Sprite.Create(textures[7], standardRect, standardPivot);
                continue;
            }

        }
    }
}
