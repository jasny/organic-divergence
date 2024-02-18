using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class SpriteSelector : MonoBehaviour
{
    public SpriteResolver[] spriteResolvers;
    public Light2D spriteLight;

    private void Start()
    {
        Randomize();
    }

    public void Randomize()
    {
        foreach (var resolver in spriteResolvers)
        {
            RandomizeSprite(resolver);
        }

        SetSpriteLight();
    }

    public void ChangeLibrary(SpriteLibraryAsset library)
    {
        ChangeLibrary(0, library);
    }
    
    public void ChangeLibrary(int index, SpriteLibraryAsset library)
    {
        var resolver = spriteResolvers[index];
        resolver.spriteLibrary.spriteLibraryAsset = library;
        
        SetSpriteLight();
    }
    
    private void RandomizeSprite(SpriteResolver resolver)
    {
        var spriteLibrary = resolver.spriteLibrary.spriteLibraryAsset;
        if (!spriteLibrary)
        {
            Debug.LogError("No sprite library");
            return;
        }
        
        var category = resolver.GetCategory();
        var spriteList = spriteLibrary.GetCategoryLabelNames(category).ToList();
        var count = spriteList.Count;

        if (count == 0) return;
        
        var randomIndex = Random.Range(0, count);
        var randomSprite = spriteList[randomIndex];

        resolver.SetCategoryAndLabel(category, randomSprite);
    }

    private void SetSpriteLight()
    {
        if (!spriteLight) return;

        var resolver = spriteResolvers[0];

        var spriteLibrary = resolver.spriteLibrary.spriteLibraryAsset;
        if (!spriteLibrary)
        {
            Debug.LogError("No sprite library");
            return;
        }
        
        spriteLight.lightCookieSprite = spriteLibrary.GetSprite("Light", resolver.GetLabel());
    }
}
