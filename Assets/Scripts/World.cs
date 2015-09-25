using UnityEngine;
using System.Collections;

public enum WorldSize {None, Small, Medium, Large};
public enum WorldType {None, Verdant, Icy, Ocean, Barren, Volcanic, Radioactive, Gaseous};
public enum AxisTilt {None, Slight, Moderate, Severe};      // Affects intensity of difficulty scaling during seasons
public enum Season {None, Spring, Summer, Fall, Winter};


public class World {

  public string name;

  public WorldSize size;
  public WorldType type;
  public AxisTilt axisTilt;
  public Season currentSeason;

  Zone[] allZones;

}
