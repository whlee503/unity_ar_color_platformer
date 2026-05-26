using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class button1 : MonoBehaviour    //ÀÌ¿øÈñ LeeWonhee's script
{                                       //that manages Color Detecting with "Color!!"  button's behaviour
    int flag;
    public ARCameraManager cameraManager;
    public Color targetColor = Color.black;
    public ChangePanelColor panelImage;

    public AudioClip audioClip1; 
    public AudioClip audioClip2;
    public AudioSource audioSource;

    [Range(0f, 1f)] public float colorThreshold = 0.2f;


    public void Play()
    {
        if (panelImage.isColor)
        {
            emitColor();
        }
        else colorDetect();        
    }

    public void emitColor()     //emitting color to block
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Block");
        if(candidates != null) {
            GameObject block = candidates[0];
            Debug.Log(block);
            block1 blockobj = block.GetComponent<block1>();
            candidates[0].tag = "Untagged";
            blockobj.SetBlock(targetColor);
            panelImage.SetDetectedColor(new Color(1f, 1f, 1f, 0.0f));
            PlayAudio1();
            panelImage.isColor = false;
        }
    }

    void Start()
    {
        //SaudioSource = gameObject.AddComponent<AudioSource>(); 
        audioSource.clip = audioClip1;
        audioSource.playOnAwake = false; 
    }

    public void PlayAudio1()
    {
        audioSource.clip = audioClip1;
        audioSource.Play();             // play audio clip
    }

    public void PlayAudio2()
    {
            audioSource.clip = audioClip1;
            audioSource.Play();         // play audio clip
    }


    void colorDetect()                  //detecting color code
    {
        flag = 1;
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Block");
        if (candidates != null)
        {
            GameObject block = candidates[0];
            Debug.Log(block);
            block1 blockobj = block.GetComponent<block1>();
            targetColor = blockobj.blockColor;
        }
        Debug.Log(targetColor);
        

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

                // use only 5% of the screenspace for color detecting
                int regionWidth = width / 20;
                int regionHeight = height / 20;
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
                        if (!panelImage.isColor) {
                            if (IsColorMatch(currentColor, targetColor))
                            {
                                Debug.Log("Center color matched at (" + x + "," + y + "): " + currentColor);
                                Color tempColor = targetColor;
                                tempColor.a = 0.8f;
                                panelImage.SetDetectedColor(tempColor);             //give detected color to UI 
                                panelImage.isColor = true;
                                PlayAudio1();
                            }
                            PlayAudio2();
                            flag = 0;
                        }
                       
                        return;
                    }
                }
            }
        }
    }

    bool IsColorMatch(Color c1, Color c2)           //color comparing code
    {
        return Vector3.Distance(new Vector3(c1.r, c1.g, c1.b), new Vector3(c2.r, c2.g, c2.b)) <= colorThreshold;
    }

}
