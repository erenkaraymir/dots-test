using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnEntity : MonoBehaviour,IDeclareReferencedPrefabs, IConvertGameObjectToEntity

{
    private List<GameObject> _gameObjects;
    [SerializeField] private GameObject _spawnableObject;
    private GameObject _temp;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
         _temp = Instantiate(_spawnableObject, Vector3.zero, Quaternion.identity);
          _gameObjects.Add(_temp);
        }
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        for (var i = 0; i < _gameObjects.Count; i++)
        {
            var prefabEntity = conversionSystem.GetPrimaryEntity(_gameObjects[i]);
            //We got prefab entity, store it in global map ID <-> Prefab Entity or where you want
        }

        dstManager.DestroyEntity(entity);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(_gameObjects);
    }

    public void ProcessPrefabs(List<GameObject> prefabs)
    {
        _gameObjects = prefabs;
    }
}
