using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_LSystem : MonoBehaviour
{
    [SerializeField]
    private Material lineMaterial;

    [SerializeField]
    private Texture2D plantCircle;

    private int plantCount;

    private Vector3 currentDrawPosition;
    private Stack<Vector3> returnDrawPositionStack = new Stack<Vector3>();
    
    private Vector3 direction;
    private Stack<Vector3> returnDirectionStack = new Stack<Vector3>();

    private float rotationAngle;

    private float lineLength;
    private float lineWidth;

    private const float colorGradientAmount = 0.0001f;
    private Color32 lineColor;

    private string[] axioms = new string[7] { "X", "-X", "+X", "W", "Y", "Z", "VC"};

    private int plantLayer;

    private GameObject plantParent;

    // Start is called before the first frame update
    void Start()
    {
        plantCount = UnityEngine.Random.Range(3, 6);

        for (int i = 0; i < plantCount; i++)
        {
            CreatePlant();
        }
    }

    private void CreatePlant()
    {
        GameObject plant = new GameObject("Plant");
        plant.transform.SetParent(gameObject.transform);
        plantParent = plant;

        plantLayer = DeterminePlantLayer();
        int iterations = UnityEngine.Random.Range(6, 7);

        // Plant starting color

        SetStartingColor();

        SetLineType();

        SetRotation();
        string axiom = axioms[UnityEngine.Random.Range(0, axioms.Length)];
        Vector3 plantStartLocation = new Vector3(UnityEngine.Random.Range(-2f, 2f),
                                                 UnityEngine.Random.Range(-0.9f, -1.1f),
                                                 0);
        InitializeVectors(plantStartLocation);

        // Create plant string by applying rules
        string plantString = ApplyRules(iterations, axiom);

        //Debug.Log(plantString);

        // Display the plant string
        Render(plantString);
    }

    private void SetStartingColor()
    {
        (float, float, float) color1 =
            (UnityEngine.Random.value,
            UnityEngine.Random.Range(0.6f, 0.7f),
            UnityEngine.Random.Range(0.15f, 0.25f));

        lineColor = Color.HSVToRGB(color1.Item1, color1.Item2, color1.Item3);
    }

    private void InitializeVectors(Vector3 plantStartLocation)
    {
        currentDrawPosition = plantStartLocation;
        direction = Vector3.up;
        returnDirectionStack.Clear();
        returnDrawPositionStack.Clear();
    }

    private static string ApplyRules(int iterations, string axiom)
    {
        string currentString = axiom;

        // For each itertion
        for (int i = 0; i < iterations; i++)
        {
            // Create a string for this iteration
            string iterationString = null;

            // For each character in the current string
            foreach (char c in currentString)
            {
                // Check if there is a rule for c and apply it
                string ruleOutput = CheckRule(c);

                iterationString += ruleOutput;
            }

            // After each character is evaluated
            // Set the currentString to the iterationString
            currentString = iterationString;
        }

        return currentString;
    }

    private static string CheckRule(char c)
    {
        if (c == 'W')
        {
            return "[+W]-F[-W]FW";
        }
        else if (c == 'X')
        {
            return "+[[X]-X]-F[-FX]+X";
        }
        else if(c == 'Y')
        {
            return "[+Y]F[-Y]FY";
        }
        else if (c == 'Z')
        {
            return "[+Z]F[-Z] + Z";
        }
        else if (c == 'F')
        {
            return "FF";
        }
        else if (c == 'V')
        {
            return "+[[XC]-XC]-F[-FXC]+X";
        }
        else if (c == 'C')
        {
            return "C";
        }
        else
        {
            // No rule for this character. 
            return c.ToString();
        }

    }

    private void Render(string plantString)
    {


        foreach (char c in plantString)
        {
            ApplyDrawingRule(c);
        }
    }

    private void ApplyDrawingRule(char c)
    {
        if (c == 'F')
        {
            Vector3 startPos = currentDrawPosition;
            Vector3 endPos = currentDrawPosition + direction;

            CreateLine(startPos, endPos);

            // Return position and direction
            currentDrawPosition = endPos;

            ResizeDirection();
        }
        else if (c == 'C')
        {
            // Draw a circle
            CreateCircle(currentDrawPosition);

        }
        else if (c == '+')
        {
            // Change the direction by a set rotation (e.g., 45 degrees)
            direction = RotateDirection(rotationAngle);
            ResizeDirection();
        }
        else if (c == '-')
        {
            // Change the direction by a set rotation (e.g., 45 degrees)
            direction = RotateDirection(-rotationAngle);
            ResizeDirection();
        }
        else if (c == '[')
        {
            // Add the position to the stack
            returnDrawPositionStack.Push(currentDrawPosition);
            // Add the direction to the stack
            returnDirectionStack.Push(direction);

            ResizeDirection();
        }
        else if (c == ']')
        {
            // Return position and direction
            currentDrawPosition = returnDrawPositionStack.Pop();
            direction = returnDirectionStack.Pop();

            ResizeDirection();
        }
        else
        {
            // No rule for this character. 
        }
    }

    private void SetLineType()
    {
        if (UnityEngine.Random.value > 0.7)
        {
            // Normal line size 70%
            lineLength = UnityEngine.Random.Range(0.01f, 0.03f);
            lineWidth = UnityEngine.Random.Range(0.02f, 0.04f);
        }
        else
        {
            // Fat line size
            lineLength = UnityEngine.Random.Range(0.01f, 0.03f);
            lineWidth = UnityEngine.Random.Range(0.05f, 0.1f);
        }

    }

    private float SetRotation()
    {
        rotationAngle = UnityEngine.Random.Range(10f, 30f);
        return rotationAngle;
    }

    private Vector3 RotateDirection(float angle)
    {
        Vector3 tempAngle = Vector3.zero;

        tempAngle.x = direction.x * Mathf.Cos(angle * Mathf.Deg2Rad) -
                      direction.y * Mathf.Sin(angle * Mathf.Deg2Rad);

        tempAngle.y = direction.x * Mathf.Sin(angle * Mathf.Deg2Rad) +
                      direction.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        return tempAngle;
    }

    private void ResizeDirection()
    {
        direction.Normalize();
        direction *= lineLength;
    }

    private void CreateLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject newLine_go = new GameObject("Line");
        newLine_go.transform.SetParent(plantParent.transform);
        LineRenderer newLine_lr = newLine_go.AddComponent<LineRenderer>();

        // Set the color
        newLine_lr.material = lineMaterial;

        newLine_lr.startColor = lineColor;
        UpdateColor(lineColor);
        newLine_lr.endColor = lineColor;

        newLine_lr.startWidth = lineWidth;
        newLine_lr.endWidth = lineWidth;

        newLine_lr.SetPosition(0, startPos);
        newLine_lr.SetPosition(1, endPos);

        newLine_lr.sortingOrder = plantLayer;
    }

    private void CreateCircle(Vector3 position)
    {
        GameObject newcircle_go = new GameObject("Circle");
        newcircle_go.transform.SetParent(plantParent.transform);
        newcircle_go.transform.position = position;
        newcircle_go.transform.localScale = new Vector3(0.4f, 0.4f, 1);

        SpriteRenderer newcircle_sr = newcircle_go.AddComponent<SpriteRenderer>();
        newcircle_sr.sprite = Sprite.Create(
            plantCircle,
            new Rect(0.0f, 0.0f, plantCircle.width, plantCircle.height),
            new Vector2(0.5f, 0.5f));

        newcircle_sr.sortingOrder = plantLayer + 1;
    }

    private void UpdateColor(Color32 oldColor)
    {
        // Get current color
        float h; float s; float v;
        Color.RGBToHSV(oldColor, out h, out s, out v);

        // Edit color
        v += colorGradientAmount;

        // Set new color
        Color32 newColor = Color.HSVToRGB(h, s, v);
        lineColor = newColor;

    }

    private int DeterminePlantLayer()
    {
        if (UnityEngine.Random.value > .5f)
        {
            return 5;
        }
        return 7;
    }
}
