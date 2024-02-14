using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class SpriteSelector : MonoBehaviour
{
    public SpriteResolver[] spriteResolvers;

    private void Start()
    {
        foreach (var resolver in spriteResolvers)
        {
            RandomizeSprite(resolver);
        }
    }
    
    private void RandomizeSprite(SpriteResolver resolver)
    {
        var spriteLibrary = resolver.spriteLibrary.spriteLibraryAsset;
        if (!spriteLibrary)
        {
            Debug.Log("here");
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
}
