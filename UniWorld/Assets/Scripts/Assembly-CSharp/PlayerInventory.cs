using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
	public Sprite[] iconeSprite;

	public static PlayerInventory instance;

	public Image[] imageArray;

	public RectTransform selectedOutline;

	public byte[] itemSlot = new byte[9];

	public int selectedSlot;

	public GameObject itemContainer;

	public bool isOpen;

	public Transform itemButtonsRoot;

	public GameObject itemButtonPrefab;

	private void Awake()
	{
		if ((bool)instance)
		{
			Object.Destroy(this);
			return;
		}
		instance = this;
		for (byte b = 0; b < iconeSprite.Length; b++)
		{
			Object.Instantiate(itemButtonPrefab, itemButtonsRoot).GetComponent<InventoryItemButton>().Initialize(b);
		}
	}

	public void ChangeSelectedItem(float y)
	{
		selectedSlot = Mathf.Clamp(selectedSlot - Mathf.RoundToInt(y), 0, 8);
		selectedOutline.position = imageArray[selectedSlot].rectTransform.position;
	}

	public void UpdateSlot(byte newValue)
	{
		itemSlot[selectedSlot] = newValue;
		imageArray[selectedSlot].sprite = iconeSprite[newValue];
	}

	public byte GetSelectedItem()
	{
		return itemSlot[selectedSlot];
	}

	private void Update()
	{
		if (Time.timeScale != 0f && ActionManager.GetActionDown("Inventory"))
		{
			itemContainer.SetActive(!itemContainer.activeInHierarchy);
			isOpen = itemContainer.activeInHierarchy;
			Cursor.lockState = ((!isOpen) ? CursorLockMode.Locked : CursorLockMode.None);
		}
		if (Time.timeScale != 0f && Input.GetKeyDown(KeyCode.Escape) && isOpen)
		{
			itemContainer.SetActive(value: false);
			isOpen = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
