using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameObjectBehaviour : MonoBehaviour
{

    public UIPanel panel;
    public MyScrollView listPopulator;
    public UIScrollView draggablePanel;
    public UIGrid grid;
    public int itemNumber;
    public int itemDataIndex;


    private bool isVisible = true;
    private BoxCollider thisCollider;

    // Use this for initialization
    void Start()
    {
        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();

        thisCollider = GetComponent<BoxCollider>();
        transform.localScale = new Vector3(1, 1, 1); // some weird scaling issues with NGUI
    }
    void Update()
    {

        if (Mathf.Abs(draggablePanel.currentMomentum.x) > 0)
        {

            CheckVisibilty();
        }
    }
    public bool verifyVisibility()
    {


        return checkVisible;
    }
    #region item UI functions
    void OnClick()
    {

    }

    void OnDrag(Vector2 delta)
    {

    }

    void OnPress(bool isDown)
    {

    }
    #endregion

    private bool checkVisible;
    private bool currentVisibilty;
    void CheckVisibilty()
    {

        currentVisibilty = checkVisible;
        if (currentVisibilty != isVisible)
        {
            isVisible = currentVisibilty;
            thisCollider.enabled = isVisible;

            if (!isVisible && grid != null)
            {
                print("startthread:" + itemNumber);
                StartCoroutine(listPopulator.ItemIsInvisible(itemNumber));
            }

        }
    }


    void OnBecameInvisible()
    {
        checkVisible = false;
    }

    void OnBecameVisible()
    {
        checkVisible = true;
    }

}
