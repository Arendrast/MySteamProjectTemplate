using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Data
{
    
    /// <summary>
    /// Пространственный хеш
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpatialHash<T> where T : class
    {
        private readonly float _cellSize;
        private readonly Dictionary<Vector3Int, List<T>> _cells;
        private readonly Dictionary<T, Vector3Int> _objectToCell;
        private readonly Func<T, Vector3> _positionGetter;

        public SpatialHash(float cellSize, Func<T, Vector3> positionGetter)
        {
            _cellSize = cellSize;
            _cells = new Dictionary<Vector3Int, List<T>>();
            _objectToCell = new Dictionary<T, Vector3Int>();
            _positionGetter = positionGetter;
        }

        private Vector3Int GetCellCoords(Vector3 position)
        {
            var x = Mathf.FloorToInt(position.x / _cellSize);
            var y = Mathf.FloorToInt(position.y / _cellSize);
            var z = Mathf.FloorToInt(position.z / _cellSize);
            return new Vector3Int(x, y, z);
        }

        public void Insert(T obj)
        {
            if (obj == null)
            {
                return;
            }
            
            var cell = GetCellCoords(_positionGetter(obj));
            if (!_cells.TryGetValue(cell, out var list))
            {
                list = new List<T>();
                _cells[cell] = list;
            }

            list.Add(obj);
            _objectToCell[obj] = cell;
        }

        public void Remove(T obj)
        {
            if (obj == null)
            {
                return;
            }
            
            if (_objectToCell.TryGetValue(obj, out var cell))
            {
                if (_cells.TryGetValue(cell, out var list))
                {
                    list.Remove(obj);
                    if (list.Count == 0)
                    {
                        _cells.Remove(cell);
                    }
                }

                _objectToCell.Remove(obj);
            }
        }

        public void Update(T obj)
        {
            if (obj == null)
            {
                return;
            }
            
            var newCell = GetCellCoords(_positionGetter(obj));
            if (_objectToCell.TryGetValue(obj, out var oldCell))
            {
                if (oldCell == newCell) return;
                Remove(obj);
            }

            Insert(obj);
        }

        public void QueryNotAlloc(Vector3 position, float radius, List<T> collection)
        {
            var minX = Mathf.FloorToInt((position.x - radius) / _cellSize);
            var maxX = Mathf.FloorToInt((position.x + radius) / _cellSize);
            var minY = Mathf.FloorToInt((position.y - radius) / _cellSize);
            var maxY = Mathf.FloorToInt((position.y + radius) / _cellSize);
            var minZ = Mathf.FloorToInt((position.z - radius) / _cellSize);
            var maxZ = Mathf.FloorToInt((position.z + radius) / _cellSize);

            var radiusSqr = radius * radius;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    for (var z = minZ; z <= maxZ; z++)
                    {
                        var cell = new Vector3Int(x, y, z);
                        if (_cells.TryGetValue(cell, out var list))
                        {
                            foreach (var obj in list)
                            {
                                var diff = _positionGetter(obj) - position;
                                if (diff.sqrMagnitude <= radiusSqr)
                                    collection.Add(obj);
                            }
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            _cells.Clear();
            _objectToCell.Clear();
        }
    }
}