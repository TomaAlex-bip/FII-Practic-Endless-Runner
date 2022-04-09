using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverAnimationScript : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private float hoverSize;
    
    private Color initialColor;
    private float initialSize;
    
    private Button button;
    private Text text;

    private void Start()
    {
        button = transform.GetComponent<Button>();
        text = transform.GetChild(0).GetComponent<Text>();

        initialColor = text.color;
        initialSize = button.transform.localScale.x;
    }
    
    public void OnMouseHoverEnter()
    {
        text.color = hoverColor;
        button.transform.localScale = Vector3.one * hoverSize;
    }
    
    public void OnMouseHoverExit()
    {
        text.color = initialColor;
        button.transform.localScale = Vector3.one * initialSize;
    }
    
}
