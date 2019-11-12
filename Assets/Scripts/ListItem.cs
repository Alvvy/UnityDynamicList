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
        listData.listObj = itemObj;
        listData.UIDic.Clear();
        Button btn = listData.AddUIToDic("test","Button","") as Button;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("点击元素的索引为"+listData.index);
        });
    }
   
}





public  class ListData
{
    public readonly float length;
    public readonly string loadPath;
    public int index;
    public GameObject listObj;
    public Dictionary<string, object> UIDic;
    public ListData(string _loadPath,float _length)
    {
        length = _length;
        loadPath = _loadPath;
        UIDic = new Dictionary<string, object>();
    }

    public object AddUIToDic(string _name,string T,string _path)
    {
        if (listObj == null)
        {
            return null;
        }
        if (!UIDic.ContainsKey(_name))
        {
            if (_path == "")
            {
                UIDic.Add(_name,listObj.GetComponent(T));
            }
            else
            {
                UIDic.Add(_name,listObj.transform.Find(_path).GetComponent(T));
            }
        }
        return UIDic[_name];
    }
}





