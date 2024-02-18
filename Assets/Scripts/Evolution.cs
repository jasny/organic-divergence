using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Evolution : MonoBehaviour
{
    public Ring ring;
    public string[] filterSprites;
    
    public float chance = 1;
    public bool setFree;
    public Color color;

    public void OnEnable()
    {
        IEnumerable<Organism> organisms = ring.GetComponentsInRing<Organism>();
        if (filterSprites.Length != 0) organisms = organisms.Where(organism => filterSprites.Contains(organism.SpriteLabel));
        if (chance < 0.99f) organisms = organisms.Where(_ => Random.value <= chance);
        
        foreach (var organism in organisms)
        {
            Evolve(organism);
        }
        
        enabled = false; // Disable self after effect
    }

    private void Evolve(Organism organism)
    {
        var evolved = organism.Evolve();
        if (!evolved) return;
                
        if (setFree) organism.Free = true;
        if (color != Color.clear) organism.Color = color;
    }
}
