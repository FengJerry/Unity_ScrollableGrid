using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyScrollView : MonoBehaviour
{
    public UIScrollView draggablePanel;
    private UIGrid grid;
    public float cellWidth;
    private int gridMaxPerLine;
    private List<Transform> dataList = new List<Transform>();
    private List<Transform> dataPool = new List<Transform>();
    private Hashtable dataTracker = new Hashtable();
    private int poolSize;
    private int poolSizeBuffer=8;
    private int dataListSize;
    private bool isUpdatingList = false; //check the grid is updating or not
    GridInterface gridInterface;

    //GridInterfaceinterface methods
    public void SetInterface(GridInterface _interface)
    {
        
        gridInterface = _interface;
    }

    public void Show()
    {
        poolSize = gridInterface.GetPoolCellCount();
        
        dataListSize = gridInterface.GetDataListCount();
        draggablePanel = gridInterface.SetScrollView();
        grid = gridInterface.SetGrid();
        //    cellWidth = gridInterface.SetCellWidth();
        cellWidth = grid.cellWidth;
     //   poolSize = (int)(Mathf.Floor(draggablePanel.panel.width / cellWidth)) + poolSizeBuffer;

       gridMaxPerLine = grid.maxPerLine;
        RefreshPool();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Transform GetObjFromPool(int i)
    {
        if (i >= 0 && i < poolSize)
        {
            dataPool[i].gameObject.SetActive(true);
            return dataPool[i];
        }
        else
            return null;
    }

    void RefreshPool()
    {
        for (int i = 0; i < dataPool.Count; i++)
        {
            Object.Destroy(dataPool[i].gameObject);
        }
        dataPool.Clear();
        dataTracker.Clear();
        List<Transform> gridChildList = grid.GetChildList();
      //  int realPoolSize = ((poolSize * gridMaxPerLine) < dataListSize ? (poolSize * gridMaxPerLine):dataListSize);
        for (int i = 0; i < 24; i++)
     //   for (int i = 0; i < realPoolSize ; i++)
        {
            if (grid.GetChild(i).transform.childCount != 0)
            {
                for (int j = 0; j < grid.GetChild(i).transform.childCount; j++)
                {
                   DestroyImmediate(grid.GetChild(i).GetChild(j));
                }
            }

            GameObject newGameObject = gridInterface.GetGameObject(null, i, i);
            if (newGameObject == null)
                break;
            newGameObject.transform.parent = grid.GetChild(i).transform;
            newGameObject.transform.localScale = Vector3.one;
            newGameObject.transform.localPosition = Vector3.zero;
            grid.GetChild(i).GetComponent<GameObjectBehaviour>().itemNumber = i;
            grid.GetChild(i).GetComponent<GameObjectBehaviour>().itemDataIndex = i;
            grid.GetChild(i).GetComponent<GameObjectBehaviour>().listPopulator = this;
            grid.GetChild(i).GetComponent<GameObjectBehaviour>().panel = draggablePanel.panel;
            dataTracker.Add(grid.GetChild(i).GetComponent<GameObjectBehaviour>().itemDataIndex, grid.GetChild(i).GetComponent<GameObjectBehaviour>().itemNumber);
        }
    }

    void PrepareListItemWithIndex(Transform item, int newIndex, int oldIndex)
    {
        if (newIndex < oldIndex)
            grid.GetChild(oldIndex % 24).localPosition -= new Vector3((poolSize) * cellWidth / gridMaxPerLine, 0, 0);
        else
            grid.GetChild(oldIndex % 24).localPosition += new Vector3((poolSize) * cellWidth / gridMaxPerLine, 0, 0);

        GameObject newGameObject = gridInterface.GetGameObject(item.gameObject, oldIndex, newIndex);
        newGameObject.transform.parent = grid.GetChild(oldIndex % 24).transform;
        newGameObject.transform.localScale = Vector3.one;
        newGameObject.transform.localPosition = Vector3.zero;
        grid.GetChild(oldIndex % 24).GetComponent<GameObjectBehaviour>().itemDataIndex = newIndex;
        dataTracker.Add(newIndex, (int)(dataTracker[oldIndex]));
        dataTracker.Remove(oldIndex);

    }

    public IEnumerator ItemIsInvisible(int itemNumber)
    {
        if (isUpdatingList) yield return null;
        isUpdatingList = true;
        if (dataListSize > poolSize)// we need to do something "smart"... 
        {
            Transform gameObj = grid.GetChild(itemNumber).GetChild(0);
            int itemDataIndex = gameObj.parent.GetComponent<GameObjectBehaviour>().itemDataIndex;
            int indexToCheck = 0;
            GameObjectBehaviour cellBehaviour = null;
            if (dataTracker.ContainsKey(itemDataIndex + gridMaxPerLine))
            {
                if (itemNumber < poolSize)
                {
                    cellBehaviour = grid.GetChild((itemNumber + gridMaxPerLine) % 24).GetComponent<GameObjectBehaviour>();
                    if ((cellBehaviour != null && cellBehaviour.verifyVisibility()))
                    {
                        indexToCheck = itemDataIndex - 8;
                        if (dataTracker.ContainsKey(indexToCheck))
                        {
                            for (int i = indexToCheck; i >= 0; i = i - gridMaxPerLine)
                            {
                                if (dataTracker.ContainsKey(i))
                                {
                                    cellBehaviour = grid.GetChild((int)dataTracker[i]).GetComponent<GameObjectBehaviour>();
                                    if ((cellBehaviour != null && !cellBehaviour.verifyVisibility()))
                                    {
                                        gameObj = grid.GetChild((int)(dataTracker[i])).GetChild(0);
                                        if ((i) + poolSize < dataListSize && i > -1)
                                        {
                                            PrepareListItemWithIndex(gameObj, i + poolSize, i);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

            }
            if (dataTracker.ContainsKey(itemDataIndex - gridMaxPerLine))
            {
                if (itemNumber > -1)
                {
                    cellBehaviour = grid.GetChild(((itemNumber - gridMaxPerLine) + 24) % 24).GetComponent<GameObjectBehaviour>();

                    if ((cellBehaviour != null && cellBehaviour.verifyVisibility()))
                    {
                        indexToCheck = itemDataIndex + 8;
                        if (dataTracker.ContainsKey(indexToCheck))
                        {
                            // if we have an extra item
                            for (int i = indexToCheck; i < dataListSize; i = i + gridMaxPerLine)
                            {
                                if (dataTracker.ContainsKey(i))
                                {
                                    cellBehaviour = grid.GetChild((int)dataTracker[i]).GetComponent<GameObjectBehaviour>();
                                    if ((cellBehaviour != null && !cellBehaviour.verifyVisibility()))
                                    {
                                        gameObj = grid.GetChild((int)(dataTracker[i])).GetChild(0);
                                        if ((i) - poolSize > -1 && (i) < dataListSize)
                                        {
                                            PrepareListItemWithIndex(gameObj, i - poolSize, i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        isUpdatingList = false;
    }


}
