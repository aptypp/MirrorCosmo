using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MomoCoop.MapGeneration
{
    public sealed class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private int _mapSize;
        [SerializeField]
        private int _cellSize;

        [field: SerializeField]
        public MapModule[] modules { get; private set; }

        [SerializeField]
        private MapModule _spawnRoomPrefab;

        [SerializeField]
        private MapElement _mapElementPrefab;

        private MapElement[,] _elements;

        public void Initialize()
        {
            _elements = new MapElement[_mapSize, _mapSize];
        }

        public void Generate()
        {
            InitializeGrid();

            Collapse(new Vector2Int(Random.Range(1, _mapSize - 1), Random.Range(1, _mapSize - 1)), true);
            CollapseDeterminedElements();
                
            while (true)
            {
                if (!TryGetLowestEntropy(out Vector2Int resultCoords)) break;

                Collapse(resultCoords);
                CollapseDeterminedElements();
            }

            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    if (_elements[x, y].currentModule is null) continue;

                    MapModule mapModule = Instantiate(_elements[x, y].currentModule, _elements[x, y].transform.position, Quaternion.identity);
                    NetworkServer.Spawn(mapModule.gameObject);
                }
            }
        }

        private void CollapseDeterminedElements()
        {
            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    if (_elements[x, y].isCollapsed) continue;

                    if (_elements[x, y].allowedModules.Count != 1) continue;

                    Collapse(new Vector2Int(x, y));
                }
            }
        }

        private bool TryGetLowestEntropy(out Vector2Int resultCoords)
        {
            resultCoords = Vector2Int.zero;

            int minCount = Int32.MaxValue;
            bool isFound = false;

            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    if (_elements[x, y].isCollapsed) continue;
                    if (_elements[x, y].allowedModules.Count >= minCount) continue;

                    minCount = _elements[x, y].allowedModules.Count;
                    resultCoords = new Vector2Int(x, y);
                    isFound = true;
                }
            }

            return isFound;
        }

        private void Collapse(Vector2Int thisCoordinates, bool isSpawn = false)
        {
            MapElement mapElement = _elements[thisCoordinates.x, thisCoordinates.y];

            mapElement.isCollapsed = true;

            if (!isSpawn && mapElement.allowedModules.Count == 0) return;

            mapElement.currentModule = isSpawn ? _spawnRoomPrefab : mapElement.allowedModules[Random.Range(0, mapElement.allowedModules.Count)];

            Vector2Int up = thisCoordinates + Vector2Int.up;
            Vector2Int down = thisCoordinates + Vector2Int.down;
            Vector2Int left = thisCoordinates + Vector2Int.left;
            Vector2Int right = thisCoordinates + Vector2Int.right;

            if (IsCoordinatesCorrect(up)) RemoveModules(_elements[up.x, up.y], mapElement.currentModule.excludedModulesUp);
            if (IsCoordinatesCorrect(down)) RemoveModules(_elements[down.x, down.y], mapElement.currentModule.excludedModulesDown);
            if (IsCoordinatesCorrect(left)) RemoveModules(_elements[left.x, left.y], mapElement.currentModule.excludedModulesLeft);
            if (IsCoordinatesCorrect(right)) RemoveModules(_elements[right.x, right.y], mapElement.currentModule.excludedModulesRight);
        }

        private void RemoveModules(MapElement element, MapModule[] excludedModules)
        {
            if (element.isCollapsed) return;

            for (int moduleIndex = 0; moduleIndex < excludedModules.Length; moduleIndex++)
            {
                element.allowedModules.Remove(element.allowedModules.Find(module => module.instanceId == excludedModules[moduleIndex].instanceId));
            }
        }

        private bool IsCoordinatesCorrect(Vector2Int coords) => coords.x >= 0 && coords.x < _mapSize && coords.y >= 0 && coords.y < _mapSize;

        private void InitializeGrid()
        {
            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    _elements[x, y] = Instantiate(_mapElementPrefab, new Vector3(x, 0, y) * _cellSize, Quaternion.identity);

                    _elements[x, y].allowedModules = new List<MapModule>(modules);
                }
            }
        }
    }
}