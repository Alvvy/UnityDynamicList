using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SameItemListManager
{
    
    private ScrollRect scrollRect;
	
	private List<ListData> listDatas;//list的数据
	private List<ListItem> listItems;//list的实体
	
	private int currFirstItemIndex = 0;//当前显示的第一个元素的索引
	private float currListOffset = 0;//当前列表偏移
	
	public int sameItemCount = 1;//一行元素的个数
	public float sameItemDis = 0;//一行元素的间距
	
	
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
		LayoutRebuilder.ForceRebuildLayoutImmediate(scrollContent);
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
			widthCount += listDatas[0].length;
			currLastItemIndex += sameItemCount;
		} while (widthCount < allWidth + listDatas[currFirstItemIndex].length);
		
		int lastIndex = currLastItemIndex > listDatas.Count-1 ? listDatas.Count-1 : currLastItemIndex;
		Debug.Log(lastIndex);
		return lastIndex;
	}

	
	public void AddListItem(ListData listData)
	{
		listDatas.Add(listData);
		listData.index = listDatas.Count-1;
//		InitList();
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
		int currIndex = 0;
		for (int i = currFirstItemIndex; i <= GetListLastIndex(); i++)
		{
			ListItem item = ListPool.Instance().GetItemByType(listDatas[i].loadPath);
			item.listData = listDatas[i];
			item.ItemObj.transform.SetParent(scrollRect.content,false);
			SetRectTransform(item.ItemObj.transform as RectTransform);
			float otherLength = currIndex * listDatas[i].otherLength + currIndex * sameItemDis;
			if (isHorizontal)
			{
				item.ItemObj.transform.localPosition = new Vector3(currPos,otherLength,0);
			}
			else
			{
				item.ItemObj.transform.localPosition = new Vector3(otherLength,-currPos,0);
			}
//			Debug.Log(currPos);
			listItems.Add(item);
			item.BlindUI();
			item.ItemObj.name = i.ToString();
			if (currIndex < sameItemCount-1)
			{
				currIndex++;
			}
			else
			{
				currIndex = 0;
				currPos = currPos + item.listData.length;
			}
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

		int count = Mathf.CeilToInt(listDatas.Count/sameItemCount);
		if (isHorizontal)
		{
			scrollContent.sizeDelta =new Vector2( listDatas[0].length * count, 0);
		}
		else
		{
			scrollContent.sizeDelta =new Vector2( 0, listDatas[0].length * count);
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
		int lengthCount = 0;
		int rowCount = 0;
		bool isLook = false;
		for (int i = 0; i < listDatas.Count; i++)
		{
			if (lengthCount < sameItemCount-1)
			{
				lengthCount++;
			}
			else
			{
				lengthCount = 0;
				rowCount += 1;
				if (rowCount* listDatas[0].length > bound)
				{
					tempIndex = sameItemCount * (rowCount-1);
//					tempIndex = listDatas[i].index-sameItemCount * rowCount;
					isLook = true;
					if (tempIndex < 0)
					{
						tempIndex = 0;
					}
					break;
				}
			}
		}

		if (!isLook)
		{
			Debug.Log(rowCount);
		}
		if (tempIndex != currFirstItemIndex && isLook)
		{
			currFirstItemIndex = tempIndex;
//			Debug.Log(currFirstItemIndex+":"+GetListLastIndex());
			currListOffset = -listDatas[tempIndex].length + rowCount* listDatas[0].length;
			RenderList();
		}
	}



	private  void SetRectTransform(RectTransform obj)
	{
		if (isHorizontal)
		{
			obj.pivot = new Vector2(0,0);
			obj.anchorMin = new Vector2(0,0);
			obj.anchorMax = new Vector2(0,0);

		}
		else
		{
			obj.pivot = new Vector2(0,1);
			obj.anchorMin = new Vector2(0,1);
			obj.anchorMax = new Vector2(0,1);
		}
	}
	
	
}