

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责提供listItem 回收listItem
/// </summary>
public class ListPool
{
      
      public  List<ListItem> listPool ;//缓存list实体

      public  GameObject pool; //存放无用的list实体

      public  Dictionary<ItemType, GameObject> loadObjectsPool;//缓存不同类型的list资源

      public static ListPool instance;
      public static ListPool Instance()
      {
            if (instance == null)
            {
                  instance = new ListPool();
                  instance.pool=  new GameObject("Pool");
                  instance.listPool = new List<ListItem>();
                  instance.loadObjectsPool = new Dictionary<ItemType, GameObject>();
            }
            return instance;
      }

      /// <summary>
      /// 获取listItem
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      public  ListItem GetItemByType(ItemType type)
      {
            ListItem temp = null;
            foreach (var item in listPool)
            {
                  if (item.listData.itemType == type && item.isInPool)
                  {
                        temp = item;
                  }
            }

            if (temp == null)
            {
                  temp = CreateItem(type);
            }
            temp.isInPool = false;
            ShowHideObj(temp.ItemObj, true);
            return temp;
      }
      /// <summary>
      /// 回收listItem
      /// </summary>
      /// <param name="item"></param>
      public  void RecycleItem(ListItem item)
      {
            item.isInPool = true;
            ShowHideObj(item.ItemObj, false);
            item.ItemObj.transform.SetParent(pool.transform,false);
      }


      private  ListItem CreateItem(ItemType type)
      {
            GameObject obj;
            if (loadObjectsPool.ContainsKey(type))
            {
                  obj = loadObjectsPool[type];
            }
            else
            {

                  obj = Resources.Load<GameObject>(ItemConst.GetPrefabPath(type));
                  loadObjectsPool.Add(type,obj);
            }
            ListItem temp = new ListItem(obj);
            listPool.Add(temp);
            ShowHideObj(temp.ItemObj, false);
            return temp;
      }

     

     
      public  void ShowHideObj(GameObject obj,bool state)
      {
            obj.SetActive(state);
      }

      
}