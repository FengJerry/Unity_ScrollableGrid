using UnityEngine;
using System.Collections;

public class UpdateValue : MonoBehaviour
{
    public UIScrollView SV;
    public UIGrid Grid;
    public UISlider slider;

    int cellCount;
    int rowCount;
    float cellHeight;
    float cellWidth;
    float maxSVSize;

    // Use this for initialization
    void Start()
    {
        cellCount = SV.GetComponent<ScrollViewController>().GetDataListCount();
        rowCount = Grid.maxPerLine;
        cellHeight = Grid.cellHeight;
        cellWidth = Grid.cellWidth;
        maxSVSize = SV.transform.localPosition.x - Mathf.Ceil((cellCount / rowCount)) * cellWidth + SV.panel.width;
        //SV.bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (SV.transform.localPosition.x < maxSVSize)
            slider.value = 1;
        else if (SV.transform.localPosition.x > 0)
            slider.value = 0;
        else
            slider.value = Mathf.Abs(SV.transform.localPosition.x / maxSVSize);



    }
}
