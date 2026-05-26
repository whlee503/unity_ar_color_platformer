using UnityEngine;
using UnityEngine.UI;

public class ChangePanelColor : MonoBehaviour   //ÀÌ¿øÈñ LeeWonhee's script
{
    public Button colorButton;     
    public Image panelImage;       
    public Color detectedColor;    
    public bool isColor = false;

    void Start()
    {
        colorButton.onClick.AddListener(ChangeColor);
    }

    void ChangeColor()
    {
        panelImage.color = detectedColor;
    }

    public void SetDetectedColor(Color newColor)    //change UI color recieved
    {
        detectedColor = newColor;
    }
}
