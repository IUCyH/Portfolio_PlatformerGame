using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout
{
    List<GameObject> items = new List<GameObject>();
    
    int distInVertical;
    int distInHorizontal;
    int maxCountHorizontal;
    
    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public void InitGrid(int distOfVertical, int distOfHorizontal, int maxCountInHorizontal)
    {
        distInVertical = distOfVertical;
        distInHorizontal = distOfHorizontal;
        maxCountHorizontal = maxCountInHorizontal;

        UpdateGrid();
    }

    public void UpdateGrid()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            if (i > 0)
            {
                var itemPos = items[i - 1].transform.position;
                var posX = itemPos.x + distInHorizontal;
                var posY = itemPos.y;
                
                if (i > maxCountHorizontal)
                {
                    posX = distInHorizontal;
                    posY = itemPos.y - distInVertical;
                }

                item.transform.position = new Vector3(posX, posY, 0f);
            }
            else
            {
                item.transform.position = new Vector3(distInHorizontal, distInVertical, 0f);
            }
        }
    }
}
