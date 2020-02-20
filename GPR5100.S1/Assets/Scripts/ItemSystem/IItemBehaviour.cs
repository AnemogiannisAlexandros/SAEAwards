using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Interface of All Items
/// </summary>
public interface IItemBehaviour 
{
    void InitializeItem(Player player, float offset,Vector3 originalDirection,float lag,PhotonMessageInfo info);
    float GetOffset();
}
