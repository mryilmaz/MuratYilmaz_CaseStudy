using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PaintWall : MonoBehaviour
{
    public Sprite brushSprite; 
    public float brushSize = 0.5f; 
    public Material[] paintColors; 
    public TextMeshPro percentageText; 

    private Renderer wallRenderer;
    private Texture2D wallTexture;  
    private Color[] brushPixels;    
    private HashSet<Vector2Int> paintedPixels;  
    private int totalPixels;  

    private void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallTexture = new Texture2D(1024, 1024);  
        wallRenderer.material.mainTexture = wallTexture;

        
        if (brushSprite.texture.isReadable)
        {
            brushPixels = brushSprite.texture.GetPixels();
        }
        else
        {
            Debug.LogError("Texture is not readable");
        }

        paintedPixels = new HashSet<Vector2Int>();  
        totalPixels = wallTexture.width * wallTexture.height;  
    }

    void Update()
    {
        HandleBrushMovement();
        HandleBrushPainting();
        UpdatePercentageText();  
    }

    void HandleBrushMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //print("test");
            }
        }
    }

    void HandleBrushPainting()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("PaintableWall"))
                {
                    Paint(hit.textureCoord);
                }
            }
        }
    }

    void Paint(Vector2 textureCoord)
    {
        Color color = paintColors[0].color;  
        int pixelX = (int)(textureCoord.x * wallTexture.width);
        int pixelY = (int)(textureCoord.y * wallTexture.height);
        
        int brushWidth = Mathf.CeilToInt(brushSprite.bounds.size.x * brushSize * wallTexture.width);  
        int brushHeight = Mathf.CeilToInt(brushSprite.bounds.size.y * brushSize * wallTexture.height); 
        
        for (int i = 0; i < brushWidth; i++)
        {
            for (int j = 0; j < brushHeight; j++)
            {
                int textureIndexX = (int)(i * brushSprite.texture.width / brushWidth);
                int textureIndexY = (int)(j * brushSprite.texture.height / brushHeight);

                if (textureIndexX >= 0 && textureIndexX < brushSprite.texture.width &&
                    textureIndexY >= 0 && textureIndexY < brushSprite.texture.height)
                {
                    Color brushPixel = brushPixels[textureIndexY * brushSprite.texture.width + textureIndexX];
                    if (brushPixel.a > 0)  // if there is alpha value
                    {
                        int offsetX = pixelX + i - brushWidth / 2;
                        int offsetY = pixelY + j - brushHeight / 2;

                        if (offsetX >= 0 && offsetX < wallTexture.width &&
                            offsetY >= 0 && offsetY < wallTexture.height)
                        {
                            //checking if painted before
                            Vector2Int pixelPos = new Vector2Int(offsetX, offsetY);
                            if (!paintedPixels.Contains(pixelPos))
                            {
                                wallTexture.SetPixel(offsetX, offsetY, color);
                                paintedPixels.Add(pixelPos);  
                            }
                        }
                    }
                }
            }
        }

        wallTexture.Apply();  
    }

    
    void UpdatePercentageText()
    {
        float percentage = (float)paintedPixels.Count / totalPixels * 100f;
        percentageText.text = "Painted: " + Mathf.RoundToInt(percentage) + "%";
    }
    
    //ui'dan cagiracagim yarin
    public void SetBrushColor(int colorIndex)
    {
        paintColors[0] = paintColors[colorIndex];
    }
    //ui'dan cagiracagim yarin
    public void SetBrushSize(float size)
    {
        brushSize = size;
    }
}
