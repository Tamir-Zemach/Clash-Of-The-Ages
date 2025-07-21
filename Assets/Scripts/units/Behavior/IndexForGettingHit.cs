using System.Collections;
using units.Behavior;
using UnityEngine;

[RequireComponent(typeof(UnitHealthManager))]
public class IndexForGettingHit : MonoBehaviour
{
    public float _flashTime = 0.2f;
    private UnitHealthManager HealthManager;
    private MeshRenderer[] meshRenderers;
    private SpriteRenderer[] spriteRenderers;
    private Color[][] defaultMeshColors; // Store colors for multiple materials
    private Color[] defaultSpriteColors; // Store sprite renderer colors

    private void Awake()
    {
        HealthManager = GetComponent<UnitHealthManager>();
        if (HealthManager != null)
        {
            HealthManager.OnHealthChanged += GettingHurtFlash; 
        }

        meshRenderers = GetComponentsInChildren<MeshRenderer>() ?? new MeshRenderer[0];
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>() ?? new SpriteRenderer[0];

        // Store default colors for mesh renderers
        defaultMeshColors = new Color[meshRenderers.Length][];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            defaultMeshColors[i] = new Color[meshRenderers[i].materials.Length];
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                defaultMeshColors[i][j] = meshRenderers[i].materials[j].color;
            }
        }

        // Store default colors for sprite renderers
        defaultSpriteColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            defaultSpriteColors[i] = spriteRenderers[i].color;
        }
    }

    private void GettingHurtFlash()
    {
        StartCoroutine(Flash(_flashTime));
    }

    private IEnumerator Flash(float flashTime)
    {
        // Change colors to red
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                meshRenderers[i].materials[j].color = Color.red;
            }
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.red;
        }

        yield return new WaitForSeconds(flashTime);

        // Restore original colors
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                meshRenderers[i].materials[j].color = defaultMeshColors[i][j];
            }
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = defaultSpriteColors[i];
        }
    }
}