using System.Collections;
using BackEnd.Utilities;
using UnityEngine;

namespace VisualCues
{
    public class TerrainTextureSwapper : MonoBehaviour
    {
        private Terrain _terrain;

        [SerializeField] private TerrainLayer _targetLayer; 
        [SerializeField] private Texture2D _defaultTexture;
        [SerializeField] private Texture2D _alternateTexture;
        [SerializeField] private Texture2D _laneSelectedTexture;

        private int _layerIndex = -1;
        private ManagedCoroutine _flashRoutine;
        private ManagedCoroutine _flashOnceRoutine;

        private void Awake()
        {
            _terrain = FindAnyObjectByType<Terrain>();
            ResolveLayerIndex();

            if (_layerIndex != -1)
            {
                ApplyTexture(_defaultTexture);
            }
            else
            {
                Debug.LogWarning($"Terrain layer '{_targetLayer?.name}' not found on terrain.");
            }
        }

        private void ResolveLayerIndex()
        {
            if (_terrain == null || _targetLayer == null) return;

            TerrainLayer[] layers = _terrain.terrainData.terrainLayers;
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] == _targetLayer)
                {
                    _layerIndex = i;
                    break;
                }
            }
        }

        public void ApplyTexture(Texture2D texture)
        {
            if (_layerIndex == -1 || texture == null) return;

            _terrain.terrainData.terrainLayers[_layerIndex].diffuseTexture = texture;

            // Force Unity to refresh the terrain rendering
            TerrainLayer[] layers = _terrain.terrainData.terrainLayers;
            _terrain.terrainData.terrainLayers = layers;
        }

        public void FlashOnce(float duration = 0.2f)
        {
            _flashOnceRoutine = CoroutineManager.Instance.StartManagedCoroutine(FlashOnceRoutine(duration));
        }

        public void StartFlashing(float interval)
        {
            if (_flashRoutine == null)
            {
                _flashRoutine = CoroutineManager.Instance.StartManagedCoroutine(FlashLoop(interval));
            }
        }

        public void StopFlashing()
        {
            if (_flashRoutine != null)
            {
                _flashRoutine.Stop();
                _flashRoutine = null;
                ApplyTexture(_defaultTexture);
            }
        }

        private IEnumerator FlashLoop(float interval)
        {
            while (true)
            {
                ApplyTexture(_alternateTexture);
                yield return new WaitForSeconds(interval);
                ApplyTexture(_defaultTexture);
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator FlashOnceRoutine(float duration)
        {
            ApplyTexture(_laneSelectedTexture);
            yield return new WaitForSeconds(duration);
            ApplyTexture(_defaultTexture);
        }
    }
}