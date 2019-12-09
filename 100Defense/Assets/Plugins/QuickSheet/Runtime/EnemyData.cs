using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
///
[System.Serializable]
public class EnemyData
{
  [SerializeField]
  string key;
  public string Key { get {return key; } set { key = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { name = value;} }
  
  [SerializeField]
  int health;
  public int Health { get {return health; } set { health = value;} }
  
  [SerializeField]
  float movespeed;
  public float Movespeed { get {return movespeed; } set { movespeed = value;} }
  
}