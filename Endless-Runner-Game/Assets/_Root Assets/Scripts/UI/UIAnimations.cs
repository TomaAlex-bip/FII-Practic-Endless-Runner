using System.Collections;
using UnityEngine;

public class UIAnimations
{
    public static IEnumerator PanelAnimationCoroutine(Transform panel, float animationSpeed, float outsidePosition)
    {
        for (var i = 0; i < panel.childCount; i++)
        {
            var child = panel.GetChild(i);
            if (i % 2 == 0)
            {
                child.localPosition = new Vector3(outsidePosition, child.localPosition.y, child.localPosition.z);
            }
            else
            {
                child.localPosition = new Vector3(-outsidePosition, child.localPosition.y, child.localPosition.z);
            }
        }
        
        for (var i = 0; i < panel.childCount; i++)
        {
            var child = panel.GetChild(i);
            while (Mathf.Abs(child.localPosition.x) > 0.1f)
            {
                child.localPosition = Vector3.Lerp( 
                    child.localPosition, 
                    new Vector3(0, child.localPosition.y, child.localPosition.z),
                    animationSpeed * Time.unscaledDeltaTime
                );
                // Debug.Log($"vine ceva: {child.name}: {Mathf.Abs(child.localPosition.x)}");
                yield return null;
            }
        }
    }
}
