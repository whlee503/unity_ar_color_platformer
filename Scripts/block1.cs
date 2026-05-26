using UnityEngine;

public class block1 : MonoBehaviour //ÀÌ¿øÈñ LeeWonhee's script
{                                   //that managing block's states(color, collider)
    Renderer renderer;
    Material mat;
    BoxCollider collider;
    public bool isColor = false;

    [SerializeField]
    public Color blockColor = new Color(1f, 0f, 0f);


    void Start()
    {
        renderer = GetComponent<Renderer>();
        mat = renderer.material;
        collider = GetComponent<BoxCollider>();
        mat.SetFloat("_Mode", 3); 
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        Color tempC = blockColor;
        tempC.a = 0.3f;
        mat.SetColor("_BaseColor", tempC);  
        collider.enabled = false;           
    }

    void Update()
    {
        
    }

    public void SetBlock(Color newColor)
    {
        mat.SetColor("_BaseColor", newColor);
        collider.enabled = true;
        isColor = true;
    }


}
