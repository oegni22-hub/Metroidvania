using System;
using UnityEngine;

[Serializable]
public class ScenarioData
{
   [SerializeField] private bool doorOpen;
   [SerializeField] private bool boss1Defeated;


   public bool DoorOpen
   {
      get { return doorOpen; }
      set { doorOpen = value; }
   }
   
   public bool Boss1Defeated
   {
      get { return boss1Defeated; }
      set { boss1Defeated = value; }
   }
}
