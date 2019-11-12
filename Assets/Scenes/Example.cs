using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
	private SameItemListManager _diffItemSizeListManager;

	public Button initBtn;

	public ScrollRect ScrollRect;
	
	public Dropdown itemTypeChose;

	public Button addBtn;

	public Button clearBtn;

	public InputField inputItemIndex;

	private int currInputIndex;

	void Start () {
		_diffItemSizeListManager = new SameItemListManager();
		_diffItemSizeListManager.InitData(ScrollRect);
		_diffItemSizeListManager.sameItemCount = 2;
		_diffItemSizeListManager.sameItemDis = 4;
		initBtn.onClick.AddListener(InitList);
		addBtn.onClick.AddListener(AddListItem);
		clearBtn.onClick.AddListener(ClearList);
		itemTypeChose.ClearOptions();
		itemTypeChose.options.Add(new Dropdown.OptionData("main"));
	}

	void InitList()
	{
		_diffItemSizeListManager.InitList();
	}

	void AddListItem()
	{
		string path = "main";
		float length = 0;
		if (itemTypeChose.value == 0)
		{
			path = "main";
			length = 100;
		}
		else if(itemTypeChose.value == 1)
		{
			path = "child1";
			length = 50;
		}else if(itemTypeChose.value == 2)
		{
			path = "child2";
			length = 30;
		}

		int loopTime = 1;
		if (inputItemIndex.text != "")
		{
			loopTime = Int32.Parse(inputItemIndex.text);
		}

		for (int i = 0; i < loopTime; i++)
		{
			_diffItemSizeListManager.AddListItem(new ListData(path,length,length));
		}
		_diffItemSizeListManager.InitList();
	}
	void ClearList()
	{
		_diffItemSizeListManager.ClearList();
		_diffItemSizeListManager.ClearConent();
		_diffItemSizeListManager.ClearData();
	}
}
