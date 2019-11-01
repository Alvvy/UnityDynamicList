using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ListManager
{
	private ScrollRect scrollRect;
	
	private List<ListData> listDatas;//list的数据
	private List<ListItem> listItems;//list的实体
	private int currFirstItemIndex = 0;//当前显示的第一个元素的索引
	private float currListOffset = 0;//当前列表偏移
	
	
	
	private RectTransform rectTransform;
	private RectTransform scrollContent;
	private bool isHorizontal = false;//是否是水平的

	public void InitData(ScrollRect rect)
	{
		scrollRect = rect;
		isHorizontal = rect.horizontal;
		
		rectTransform = scrollRect.transform as RectTransform;
		scrollContent = scrollRect.content.transform as RectTransform;
		SetRectTransform(scrollContent);
		listDatas = new  List<ListData>();
		listItems = new  List<ListItem>();
		scrollRect.onValueChanged.AddListener(OnRectValueChanged);
	}
	public void InitList()
	{
		CalculateContentSize();
		ClearConent();
		RenderList();
	}
	
	private int GetListLastIndex()
	{
		int currLastItemIndex = currFirstItemIndex;
		float widthCount = 0;
		float allWidth;
		if (isHorizontal)
		{
			allWidth = rectTransform.sizeDelta.x;
		}
		else
		{
			allWidth = rectTransform.sizeDelta.y;
		}
		
		do
		{
			if (currLastItemIndex < listDatas.Count)
			{
				widthCount += listDatas[currLastItemIndex].length;
				currLastItemIndex += 1;
			}
			else
			{
				break;
			}
			
		} while (widthCount < allWidth + listDatas[currFirstItemIndex].length);
		return currLastItemIndex;
	}

	
	public void AddListItem(ListData listData)
	{
		listDatas.Add(listData);
		listData.index = listDatas.Count-1;
		InitList();
	}

	
	public void RemoveListItem(int index)
	{
		if (index <= listDatas.Count)
		{
			listDatas.RemoveAt(index);
			for (int i = index; i < listDatas.Count; i++)
			{
				listDatas[i].index -= 1;
			}
			InitList();
		}
	}

	
	/// <summary>
	/// 清除列表
	/// </summary>
	public void ClearList()
	{
		
		foreach (var item in listItems)
		{
			ListPool.Instance().RecycleItem(item);
		}
		listItems = new List<ListItem>();
		

	}
	/// <summary>
	/// 清除数据
	/// </summary>
	public void ClearData()
	{
		listDatas = new List<ListData>();
	}
	/// <summary>
	/// 清除content
	/// </summary>
	public void ClearConent()
	{
		currFirstItemIndex = 0;
		currListOffset = 0;
		if (isHorizontal)
		{
			scrollContent.localPosition = new Vector3(0,scrollContent.localPosition.y,scrollContent.localPosition.z);
		}
		else
		{
			scrollContent.localPosition = new Vector3(scrollContent.localPosition.x,0,scrollContent.localPosition.z);
		}
	}
	/// <summary>
	/// 渲染list
	/// </summary>
	public void RenderList()
	{
		ClearList();
		float currPos = currListOffset;
		for (int i = currFirstItemIndex; i < GetListLastIndex(); i++)
		{
			ListItem item = ListPool.Instance().GetItemByType(listDatas[i].itemType);
			item.listData = listDatas[i];
			item.ItemObj.transform.SetParent(scrollRect.content,false);
			if (isHorizontal)
			{
				item.ItemObj.transform.localPosition = new Vector3(currPos,0,0);

			}
			else
			{
				item.ItemObj.transform.localPosition = new Vector3(0,-currPos,0);
			}
			listItems.Add(item);
			item.BlindUI();
			SetRectTransform(item.ItemObj.transform as RectTransform);
			item.listData.btn.onClick.RemoveAllListeners();
			item.listData.btn.onClick.AddListener(() =>
			{
				Debug.Log("点击元素的索引为"+item.listData.index);
			});
			currPos = currPos + item.listData.length;
			item.ItemObj.name = i.ToString();
		}
	}
	/// <summary>
	/// 计算content尺寸
	/// </summary>
	private void CalculateContentSize()
	{
		if (isHorizontal)
		{
			scrollContent.sizeDelta = new Vector2(0,scrollRect.content.sizeDelta.y);

		}
		else
		{
			scrollContent.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x,0);
		}
		foreach (var data in listDatas)
		{
			if (isHorizontal)
			{
				scrollContent.sizeDelta = scrollRect.content.sizeDelta + new Vector2(data.length, 0);

			}
			else
			{
				scrollContent.sizeDelta = scrollRect.content.sizeDelta + new Vector2(0, data.length);
			}
		}

		if (isHorizontal)
		{
			currListOffset = scrollRect.content.transform.position.x;

		}
		else
		{
			currListOffset = scrollRect.content.transform.position.y;
		}
	}
	

	
	public void OnRectValueChanged(Vector2 sizeDelta)
	{
		CheckIsOutBounds();
	}

	public void CheckIsOutBounds()
	{
		float bound;
		if (isHorizontal)
		{
			bound =  -scrollRect.content.transform.localPosition.x;
		}
		else
		{
			bound =  scrollRect.content.transform.localPosition.y;
		}
		float currPos = 0;
		int tempIndex = 0;
		foreach (var data in listDatas)
		{
			currPos += data.length;
			if (currPos > bound)
			{
				tempIndex = data.index;
				break;
			}
		}
		if (tempIndex != currFirstItemIndex)
		{
			currFirstItemIndex = tempIndex;
			currListOffset = -listDatas[tempIndex].length + currPos;
			RenderList();
		}
	}


	private void SetRectTransform(RectTransform obj)
	{
		if (isHorizontal)
		{
			obj.pivot = new Vector2(0,0);
			obj.anchorMin = new Vector2(0,0);
			obj.anchorMax = new Vector2(0,0);

		}
		else
		{
			obj.pivot = new Vector2(1,1);
			obj.anchorMin = new Vector2(0,1);
			obj.anchorMax = new Vector2(0,1);
		}
	}
	
	
	
}

