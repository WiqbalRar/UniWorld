using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
	public byte id;

	public Image image;

	public void Initialize(byte index)
	{
		id = index;
		image.sprite = PlayerInventory.instance.iconeSprite[index];
	}

	public void OnClick()
	{
		PlayerInventory.instance.UpdateSlot(id);
	}
}
