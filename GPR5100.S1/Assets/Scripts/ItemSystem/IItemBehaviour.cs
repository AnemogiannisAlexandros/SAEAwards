using Photon.Realtime;
using UnityEngine;

public interface IItemBehaviour 
{
    void InitializeItem(Player player, float offset,Vector3 originalDirection,float lag);
    float GetOffset();
}
