using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout
{
    List<GameObject> items = new List<GameObject>();
    
    (float x, float y) firstItemPos;
    (float x, float y) distBetweenItems;
    int maxCountHorizontal;
    
    public GridLayout((float, float) firstItemPosition, (float, float) distanceBetweenItems, int maxCountInHorizontal)
    {
        firstItemPos = firstItemPosition;
        distBetweenItems = distanceBetweenItems;
        maxCountHorizontal = maxCountInHorizontal;

        UpdateGrid();
    }
    
    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public void UpdateGrid()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var itemRectTrans = item.GetComponent<RectTransform>();
            
            if (i > 0)
            {
                var itemPos = items[i - 1].GetComponent<RectTransform>().localPosition;
                var posX = itemPos.x + distBetweenItems.x;
                var posY = itemPos.y;
                Debug.Log(itemPos);
                if (i % maxCountHorizontal == 0)
                {
                    posX = firstItemPos.x;
                    posY = itemPos.y - distBetweenItems.y;
                }

                itemRectTrans.localPosition = new Vector3(posX, posY, 0f);
            }
            else
            {
                itemRectTrans.localPosition = new Vector3(firstItemPos.x, firstItemPos.y, 0f);
            }
            
            item.SetActive(true);
        }
    }
}
