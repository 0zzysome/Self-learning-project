using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Prop Item", menuName = "Inventory/Item/Prop")]
public class PropItem : Item
{

    public override void UseInInventory()
    {
        base.UseInInventory();
        EquipItem();
        
    }
    public override void AlternativeUse()
    {
        base.AlternativeUse();
        ThrowItem(this);
    }
    public override void Use()
    {
        base.Use();
        
    }
    
}
