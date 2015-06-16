using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface GridInterface
{


    int GetPoolCellCount();

    int  GetDataListCount();

    GameObject GetGameObject(GameObject objOld, int oldIndex, int newIndex);

    List<Transform> InitGameObjects();

    bool OnItemClicked(Transform gameObjectClicded);

    UIScrollView SetScrollView();

    UIGrid SetGrid();

    float SetCellWidth();

}
