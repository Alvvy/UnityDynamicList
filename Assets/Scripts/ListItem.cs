using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem
{

    public ListItem(GameObject originObj)
    {
        ItemObj = GameObject.Instantiate(originObj);
    }
    private GameObject itemObj;
    public GameObject ItemObj
    {
        get { return itemObj; }
        set { itemObj = value; }
    }

    public ListData listData;
    public bool isInPool;
    
    public void BlindUI()
    {
        if (listData == null)
        {
            Debug.LogError("listData 为 null");
            return;
        }
        listData.btn = itemObj.GetComponent<Button>();
    }
}

public enum ItemType
{
    Main = 0,
    Child1 = 1,
    Child2 = 2,

}

public class ListData
{
    public readonly ItemType itemType;
    public readonly float length;
    public int index;
    public Button btn;
    public ListData(ItemType _type)
    {
        itemType = _type;
        length = ItemConst.GetListWidthLength(_type);
    }
    
}



