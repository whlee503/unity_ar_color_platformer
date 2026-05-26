using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class ColorDetector : MonoBehaviour  //ÀÌ¿øÈñ LeeWonhee's scrit     //color detecting demo code
{
    public ARCameraManager cameraManager;
    public Color targetColor = Color.red;
    [Range(0f, 1f)] public float colorThreshold = 0.2f;

    void Update()
    {
        colorDetect();
    }


    void colorDetect()
    {
        if (cameraManager == null)
        {
            Debug.LogWarning("ARCameraManager not assigned.");
            return;
        }

        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        using (image)
        {
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGBA32,
                transformation = XRCpuImage.Transformation.None
            };

            using (var rawTextureData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp))
            {
                image.Convert(conversionParams, rawTextureData);

                int width = image.width;
                int height = image.height;

                
                int regionWidth = width / 10;
                int regionHeight = height / 10;
                int startX = (width - regionWidth) / 2;
                int startY = (height - regionHeight) / 2;

                for (int y = startY; y < startY + regionHeight; y++)
                {
                    for (int x = startX; x < startX + regionWidth; x++)
                    {
                        int index = (y * width + x) * 4;
                        float r = rawTextureData[index] / 255f;
                        float g = rawTextureData[index + 1] / 255f;
                        float b = rawTextureData[index + 2] / 255f;

                        Color currentColor = new Color(r, g, b);

                        if (IsColorMatch(currentColor, targetColor))
                        {
                            Debug.Log("Center color matched at (" + x + "," + y + "): " + currentColor);
                            return; 
                        }
                    }
                }
            }
        }
    }

    bool IsColorMatch(Color c1, Color c2)
    {
        return Vector3.Distance(new Vector3(c1.r, c1.g, c1.b), new Vector3(c2.r, c2.g, c2.b)) < colorThreshold;
    }
}


