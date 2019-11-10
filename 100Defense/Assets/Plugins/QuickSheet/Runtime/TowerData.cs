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
  string key;
  public string Key { get {return key; } set { key = value;} }
  
  [SerializeField]
  string modelname;
  public string Modelname { get {return modelname; } set { modelname = value;} }
  
  [SerializeField]
  string type;
  public string Type { get {return type; } set { type = value;} }
  
  [SerializeField]
  int price;
  public int Price { get {return price; } set { price = value;} }
  
  [SerializeField]
  int damage;
  public int Damage { get {return damage; } set { damage = value;} }
  
  [SerializeField]
  int range;
  public int Range { get {return range; } set { range = value;} }
  
  [SerializeField]
  int attackspeed;
  public int Attackspeed { get {return attackspeed; } set { attackspeed = value;} }
  
}