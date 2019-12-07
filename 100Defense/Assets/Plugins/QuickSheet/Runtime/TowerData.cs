using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
///
[System.Serializable]
public class TowerData
{
  [SerializeField]
  TowerKey towerkey;
  public TowerKey TOWERKEY { get {return towerkey; } set { towerkey = value;} }
  
  [SerializeField]
  string modelname;
  public string Modelname { get {return modelname; } set { modelname = value;} }
  
  [SerializeField]
  TowerType towertype;
  public TowerType TOWERTYPE { get {return towertype; } set { towertype = value;} }
  
  [SerializeField]
  int price;
  public int Price { get {return price; } set { price = value;} }
  
  [SerializeField]
  int damage;
  public int Damage { get {return damage; } set { damage = value;} }
  
  [SerializeField]
  float range;
  public float Range { get {return range; } set { range = value;} }
  
  [SerializeField]
  float attackspeed;
  public float Attackspeed { get {return attackspeed; } set { attackspeed = value;} }
  
}