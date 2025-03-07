using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleEffect
{
    public string effectName;
    public ParticleSystem particleSystem;

    public ParticleEffect(string name, ParticleSystem system)
    {
        effectName = name;
        particleSystem = system;
    }
}

public class ParticlesManager : MonoBehaviour //attaché au joueur
{
    public ParticleEffect[] particleEffects;
    private Dictionary<string, ParticleEffect> effectsDictionary;

    void Start()
    {
        effectsDictionary = new Dictionary<string, ParticleEffect>();
        foreach (var effect in particleEffects)
        {
            if (!effectsDictionary.ContainsKey(effect.effectName))
            {
                effectsDictionary.Add(effect.effectName, effect);
            }
            else
            {
                Debug.LogWarning("Effet: " + effect.effectName + " déjà présent");
            }
        }
    }

    public void PlayEffect (string effectName, Vector3 position)
    {
        if (effectsDictionary.ContainsKey(effectName))
        {
            var effect = effectsDictionary[effectName];
            if ( effect.particleSystem!= null)
            {
                ParticleSystem instantiatedEffect = Instantiate(effect.particleSystem, position, Quaternion.identity);
                instantiatedEffect.Play();

                Destroy(instantiatedEffect.gameObject, instantiatedEffect.main.duration + instantiatedEffect.main.startLifetime.constantMax);
            }
            else 
            {
                Debug.LogWarning("Aucun particle system assigné pour " + effectName);
            }
        }
        else
        {
            Debug.LogWarning("Aucun effet trouvé poiur " + effectName);
        }
    }
}
