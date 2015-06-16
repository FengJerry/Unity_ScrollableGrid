using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewController : MonoBehaviour, GridInterface
{
    public UIScrollView draggablePanel;
    public UIGrid grid;
    public Transform TestSprite;
    public Transform TestSprite2;
    public Transform TestSprite3;

    private float cellWidth = 200;
    private List<Transform> dataList = new List<Transform>();
    private UISlider slider;

    public int GetPoolCellCount()
    {
        return grid.GetChildList().Count;
    }

    public int GetDataListCount()
    {
        return 100;
    }

    public GameObject GetGameObject(GameObject objOld, int oldIndex, int newIndex)
    {
        GameObject newGameObject;
        DestroyImmediate(objOld);
        if (newIndex % 3 == 0)
            newGameObject = (Instantiate(TestSprite) as Transform).gameObject;
        else if (newIndex % 3 == 1)
            newGameObject = (Instantiate(TestSprite2) as Transform).gameObject;
        else
            newGameObject = (Instantiate(TestSprite3) as Transform).gameObject;
        return newGameObject;
    }

    public List<Transform> InitGameObjects()
    {
        for (int i = 0; i < 80; i++)
        {

            Transform obj;
            switch (i % 3)
            {
                case 0:
                    {
                        obj = Instantiate(TestSprite) as Transform;
                        dataList.Add(obj);
                        break;
                    }
                case 1:
                    {
                        obj = Instantiate(TestSprite2) as Transform;
                        dataList.Add(obj);
                        break;
                    }
                case 2:
                    {
                        obj = Instantiate(TestSprite3) as Transform;
                        dataList.Add(obj);
                        break;
                    }
                default:
                    break;
            }
        }
        return dataList;
    }

    public bool OnItemClicked(Transform gameObjectClicded)
    {
        return false;
    }

    public UIScrollView SetScrollView()
    {
        return draggablePanel;
    }

    public UIGrid SetGrid()
    {
        return grid;
    }

    public float SetCellWidth()
    {
        return cellWidth;
    }

    // Use this for initialization
    void Start()
    {
        slider = GameObject.Find("Progress Bar").GetComponent<UISlider>();

        MyScrollView listview = new MyScrollView();
        listview.SetInterface(this);
        listview.Show();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
