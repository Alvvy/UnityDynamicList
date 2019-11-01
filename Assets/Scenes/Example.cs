using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
	private ListManager listManager;

	public Button initBtn;

	public ScrollRect ScrollRect;
	
	public Dropdown itemTypeChose;

	public Button addBtn;

	public Button clearBtn;

	public InputField inputItemIndex;

	private int currInputIndex;

	void Start () {
		listManager = new ListManager();
		listManager.InitData(ScrollRect);
		initBtn.onClick.AddListener(InitList);
		addBtn.onClick.AddListener(AddListItem);
		clearBtn.onClick.AddListener(ClearList);
		itemTypeChose.ClearOptions();
		itemTypeChose.options.Add(new Dropdown.OptionData(ItemType.Main.ToString()));
		itemTypeChose.options.Add(new Dropdown.OptionData(ItemType.Child1.ToString()));
		itemTypeChose.options.Add(new Dropdown.OptionData(ItemType.Child2.ToString()));

	}

	void InitList()
	{
		listManager.InitList();
	}

	void AddListItem()
	{
		ItemType itemType = ItemType.Main;
		if (itemTypeChose.value == 0)
		{
			itemType = ItemType.Main;
		}
		else if(itemTypeChose.value == 1)
		{
			itemType = ItemType.Child1;
		}else if(itemTypeChose.value == 2)
		{
			itemType = ItemType.Child2;
		}

		int loopTime = 1;
		if (inputItemIndex.text != "")
		{
			loopTime = Int32.Parse(inputItemIndex.text);
		}

		for (int i = 0; i < loopTime; i++)
		{
			listManager.AddListItem(new ListData(itemType));

		}
	}


	void ClearList()
	{
		listManager.ClearList();
		listManager.ClearConent();
		listManager.ClearData();
	}
}



public static class ItemConst
{
    
	private static string mainPrefabPath = "main";
	private static string child1PrefabPath = "child1";
	private static float mainPrefabLength = 100;
	private static float child1PrefabLength = 50;
	private static string child2PrefabPath = "child2";
	private static float child2PrefabLength = 30;
	public static string GetPrefabPath(ItemType type)
	{
		switch (type)
		{
			case ItemType.Main:
				return mainPrefabPath;
			case  ItemType.Child1:
				return child1PrefabPath;
			case  ItemType.Child2:
				return child2PrefabPath;
			default:
				return "";
		}
	}

	public static float GetListWidthLength(ItemType type)
	{
		switch (type)
		{
			case ItemType.Main:
				return mainPrefabLength;
			case  ItemType.Child1:
				return child1PrefabLength;
			case  ItemType.Child2:
				return child2PrefabLength;
			default:
				return 0;
		}
	}
}