using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PaintWall : MonoBehaviour
{
    public Sprite brushSprite;
    public float brushSize = 0.5f;
    public TextMeshPro percentageText;
    public Slider brushSizeSlider;
    public Material[] paintMaterials;
    private Color currentColor;
    private Renderer wallRenderer;
    private Texture2D wallTexture;
    private Color[] brushPixels;
    private HashSet<Vector2> paintedPixels = new HashSet<Vector2>();

    private int totalPixels;
    private int paintedCount;

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
            Debug.LogError("Cannot read brush sprite!");
        }

        totalPixels = wallTexture.width * wallTexture.height;
        paintedCount = 0;

        currentColor = paintMaterials[0].color;

        if (brushSizeSlider != null)
        {
            brushSizeSlider.onValueChanged.AddListener(SetBrushSize);
            brushSizeSlider.value = 0.5f;
        }
    }

    void Update()
    {
        HandleBrushPainting();
        UpdatePercentage();
    }

    

    void HandleBrushPainting()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            ProcessBrushPainting(touchPos);
        }
        else if (Input.GetMouseButton(0)) // added for windows build
        {
            Vector2 mousePos = Input.mousePosition;
            ProcessBrushPainting(mousePos);
        }
    }

    void ProcessBrushPainting(Vector2 inputPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("PaintableWall"))
            {
                Paint(hit.textureCoord);
            }
        }
    }

    void Paint(Vector2 textureCoord)
    {
        Color color = currentColor;

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
                    if (brushPixel.a > 0)
                    {
                        int offsetX = pixelX + i - brushWidth / 2;
                        int offsetY = pixelY + j - brushHeight / 2;

                        if (offsetX >= 0 && offsetX < wallTexture.width &&
                            offsetY >= 0 && offsetY < wallTexture.height)
                        {
                            Vector2 pixelCoord = new Vector2(offsetX, offsetY);

                            if (!paintedPixels.Contains(pixelCoord))
                            {
                                wallTexture.SetPixel(offsetX, offsetY, color);
                                paintedPixels.Add(pixelCoord);
                                paintedCount++;
                            }
                            else
                            {
                                wallTexture.SetPixel(offsetX, offsetY, color);
                                paintedPixels.Add(pixelCoord);
                            }
                        }
                    }
                }
            }
        }

        wallTexture.Apply();
    }

    void UpdatePercentage()
    {
        float percentage = (float)paintedCount / totalPixels * 100;

        if (percentageText != null)
        {
            percentageText.text = "Painted: " + Mathf.RoundToInt(percentage) + "%";
        }
    }

    public void SetBrushColor(int colorIndex)
    {
        currentColor = paintMaterials[colorIndex].color;
    }

    public void SetBrushSize(float size)
    {
        brushSize = Mathf.Lerp(0.01f, 0.05f, size);
    }
}
