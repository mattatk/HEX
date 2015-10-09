using UnityEngine;
using System.Collections;

public enum WorldSize {None, Small, Medium, Large};
public enum WorldType {None, Verdant, Icy, Ocean, Barren, Volcanic, Radioactive, Gaseous};
public enum Season {None, Spring, Summer, Fall, Winter};
public enum AxisTilt { None, Slight, Moderate, Severe };      // Affects intensity of difficulty scaling during seasons


public class World {

  public string name;

  public WorldSize size;
  public WorldType type;
  public Season season;
  public AxisTilt tilt;

  World[] allWorlds;
  Zone[] allZones;

  public World(WorldSize s, WorldType t, Season se, AxisTilt at)
  {
        size = s;
        type = t;
        season = se;
        tilt = at;
  }
}
