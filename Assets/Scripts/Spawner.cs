using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private GameObject gameObjectPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField] int xSize = 10;
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;

    private GameObject _tempSpawnableObstacle;
    private Entity _tempSpawnableObstacleEntity;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;
    private BlobAssetStore blobAsset;

    [SerializeField] TMP_Text counterText;
    private int counter;

    [SerializeField] private Transform _spawnPoint;

    void Start()
    {
        blobAsset = new BlobAssetStore();
        // setup references to World and EntityManager
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        // generate Entity Prefab
        if (gameObjectPrefab != null)
        {
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, blobAsset);
            entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);
            // spawn x by y grid of Entities
            // InstantiateEntityGrid(xSize, ySize, spacing);
        }
    }

    private void Update()
    {

            //if (_tempSpawnableObstacle != null)
            //{
            //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            //pos.z = 0;
            //    _tempSpawnableObstacle.transform.position = pos;
            //    Debug.Log("Pos : " + _tempSpawnableObstacle.transform.position);
            //}

        if (Input.GetMouseButtonUp(0))
        {
            SpawnObstacle();
         //   InstantiateEntityGridTest(xSize, ySize, spacing);
            ObstacleToEntity();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            InstantiateEntityGridTest(xSize, ySize, spacing);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ObstacleToEntity();
        }
    }


    private void ObstacleToEntity()
    {
        if (_tempSpawnableObstacle != null)
        {
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, blobAsset);
            _tempSpawnableObstacleEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_tempSpawnableObstacle, settings);
            Destroy(_tempSpawnableObstacle);
            _tempSpawnableObstacle = null;
        }
    }


    public void SpawnObstacle()
    {
        if(_tempSpawnableObstacle == null)
        {
            _tempSpawnableObstacle = Instantiate(obstaclePrefab, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,  Camera.main.transform.position.z)), Quaternion.identity);
        }
    }

    // create a single Entity from an Entity prefab
    private void InstantiateEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("InstantiateEntity WARNING: No EntityManager found!");
            return;
        }

        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    public void InstantiateEntityGridTest(int dimX, int dimY, float spacing = 1f)
    {
        counter += xSize * ySize;
        counterText.text = counter.ToString();
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(_spawnPoint.position.x + (i * spacing), _spawnPoint.position.y + 15f + ( j * spacing), 0f));
            }
        }
    }

    // create a grid of Entities in an x by y formation
    private void InstantiateEntityGrid(int dimX, int dimY, float spacing = 1f)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
            }
        }
    }
    
    public void SpawnBalls()
    {
        InstantiateEntityGrid(5,5,.5f);
    }
    // create a single Entity using the Conversion Workflow
    private void ConvertToEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: No EntityManager found!");
            return;
        }

        if (gameObjectPrefab == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: Missing GameObject Prefab");
            return;
        }

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    // create a single Entity using "pure ECS"
    private void MakeEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: No EntityManager found!");
            return;
        }

        if (unitMesh == null || unitMaterial == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: Missing mesh or material");
            return;
        }

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        Entity myEntity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = position
        });

        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });
    }
}
