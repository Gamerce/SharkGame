using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HatData : ScriptableObject
{
	public enum HatName{
		None = -1,
		geo_bowler,
		geo_captain_hat,
		geo_chef_hat,
		geo_fedora,
		geo_graduate_hat,
		geo_jester_hat,
		geo_party_hat,
		geo_pirate_hat,
		geo_propeller_hat,
		geo_santa_hat,
		geo_sunhat,
		geo_tophat,

		Count,
	}
	[System.Serializable]
	public class HatInfo{
		public HatName hatName;
		public int unlockPointsNeeded;
		[System.NonSerialized]
		int internalVal = -1;
		public int atValue
		{
			get {
				if(internalVal <= 0)
					internalVal = PlayerPrefs.GetInt(hatName.ToString()+"_Amount", 0);
				return internalVal;
			}
			set {
				internalVal = value;
				PlayerPrefs.SetInt(hatName.ToString()+"_Amount", internalVal);
			}
		}
		public Vector2 imageMinMax;

		public bool IsUnlocked(){
			return atValue >= unlockPointsNeeded;
		}

		public float UnlockPercent(){
			return ((float)atValue / ((float)unlockPointsNeeded));
		}
	}

	public List<HatInfo> allHats = new List<HatInfo>();
}
