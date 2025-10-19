using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace units.Behavior
{
    public class IndexForGettingHit : MonoBehaviour
    {
        public float _flashTime = 0.2f;

        private UnitHealthManager HealthManager;

        // Store default colors for each renderer
        private Dictionary<Renderer, Color[]> defaultRendererColors = new();
        private Dictionary<SpriteRenderer, Color> defaultSpriteColors = new();

        private void Awake()
        {
            HealthManager = GetComponent<UnitHealthManager>();
            if (HealthManager != null)
            {
                HealthManager.OnHealthChanged += GettingHurtFlash;
            }

            // Collect all MeshRenderers and SkinnedMeshRenderers
            var meshRenderers = GetComponentsInChildren<MeshRenderer>() ?? new MeshRenderer[0];
            var skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>() ?? new SkinnedMeshRenderer[0];

            foreach (var renderer in meshRenderers.Concat<Renderer>(skinnedMeshRenderers))
            {
                var colors = renderer.materials.Select(m => m.color).ToArray();
                defaultRendererColors[renderer] = colors;
            }

            // Collect SpriteRenderers
            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>() ?? new SpriteRenderer[0];
            foreach (var spriteRenderer in spriteRenderers)
            {
                defaultSpriteColors[spriteRenderer] = spriteRenderer.color;
            }
        }

        private void GettingHurtFlash()
        {
            StartCoroutine(Flash(_flashTime));
        }

        private IEnumerator Flash(float flashTime)
        {
            // Flash mesh/skinned renderers to red
            if (defaultRendererColors.Count > 0)
            {
                foreach (var renderer in defaultRendererColors.Keys)
                {
                    if (renderer.materials != null && renderer.materials.Length > 0)
                    {
                        foreach (var material in renderer.materials)
                        {
                            material.color = Color.red;
                        }
                    }
                }
            }

            // Flash sprite renderers to red
            if (defaultSpriteColors.Count > 0)
            {
                foreach (var spriteRenderer in defaultSpriteColors.Keys)
                {
                    spriteRenderer.color = Color.red;
                }
            }

            yield return new WaitForSeconds(flashTime);

            // Restore mesh/skinned renderer colors
            if (defaultRendererColors.Count > 0)
            {
                foreach (var kvp in defaultRendererColors)
                {
                    var renderer = kvp.Key;
                    var colors = kvp.Value;
                    if (renderer.materials != null && renderer.materials.Length == colors.Length)
                    {
                        for (int i = 0; i < renderer.materials.Length; i++)
                        {
                            renderer.materials[i].color = colors[i];
                        }
                    }
                }
            }

            // Restore sprite renderer colors
            if (defaultSpriteColors.Count <= 0) yield break;
            {
                foreach (var kvp in defaultSpriteColors)
                {
                    kvp.Key.color = kvp.Value;
                }
            }
        }
    }
}