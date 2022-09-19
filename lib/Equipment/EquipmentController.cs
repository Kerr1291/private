using UnityEngine;
using System.Collections;
 
public class EquipmentController : MonoBehaviour
{
    public Transform mainHandAttachPoint;
    public Transform offHandAttachPoint;

    public void AttachItemPoint( Transform item_attach_point, Transform character_attach_point )
    {
        item_attach_point.SetParent( character_attach_point );
    }
}
