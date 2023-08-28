using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCreatorScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _tileGround;

    
    private void InitObjectsScene(out GameObject objectToCreate, out TypeOfCategoryObjects objectCategory)
    {
        int percentage = ProbabilityPercentage(100);

        List<TypeOfCategoryObjects> categoryObjectType = new List<TypeOfCategoryObjects>();

        foreach (var item in SceneGeneralController.Instance.ObjectValues.CategoryObjectsProportions)//Agregamos los valores que coinciden a una lista para el desempate
        {
            if (percentage <= item.Value)
            {
                categoryObjectType.Add(item.Key);
                Debug.Log("Objeto a pre-crear " + categoryObjectType[categoryObjectType.Count - 1]);
            }
            else if(percentage > item.Value) 
            {
                categoryObjectType.Add(TypeOfCategoryObjects.GROUND);
            }
        }
        //Desempatamos si hay mas de dos objetos con el mismo rango de porcentage de probabilidad
        int objectToSelect = UnityEngine.Random.Range(0, categoryObjectType.Count);

        /*if(TypeOfCategoryObjects.TELEPORTER == categoryObjectType[objectToSelect] && SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[TypeOfCategoryObjects.TELEPORTER] > 5) 
        {
            objectCategory = TypeOfCategoryObjects.GROUND;
            
        }
        else 
        {
            objectCategory = categoryObjectType[objectToSelect];
        }*/

        objectCategory = categoryObjectType[objectToSelect];

        SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[objectCategory]++;

        objectToCreate = _tileGround;
        
        //Debug.Log("Objeto a crear " + categoryObjectType[objectToSelect].ToString() + " con porcentage de " + percentage);
    }

    private int ProbabilityPercentage(int maxValue)
    {
        int result = UnityEngine.Random.Range(1, maxValue + 1);

        return result;
    }

    public void CreateGround()
    {
        List<GameObject> groundTile = new List<GameObject>();

        float posX = 0.5f;
        float posZ = 0.5f;

        for (int i = 0; i < SceneGeneralController.Instance.SceneSizeX; i++)
        {
            for (int j = 0; j < SceneGeneralController.Instance.SceneSizeY; j++)
            {
                InitObjectsScene(out GameObject objectToCreate, out TypeOfCategoryObjects objectCategory);

                

                groundTile.Add(Instantiate(objectToCreate, this.gameObject.transform));

                groundTile[groundTile.Count - 1].transform.position = new Vector3(posX, 0f, posZ);

                groundTile[groundTile.Count - 1].GetComponent<ObjectspawnedOnScene>().ActiveObjectToSpawn(objectCategory);
                groundTile[groundTile.Count - 1].GetComponent<ObjectspawnedOnScene>().SetIdObject(i + j);

                posX += 1f;
            }
            posX = 0.5f;
            posZ += 1f;
        }

        SceneGeneralController.Instance.GroundTiles = groundTile;

        DelegateUtils.OnChangeProgressBarVBalue.Invoke(true);
    }

    private void OnEnable()
    {
        DelegateUtils.OnCreateGroundScene += CreateGround;
    }

    private void OnDisable()
    {
        DelegateUtils.OnCreateGroundScene -= CreateGround;
    }
}
