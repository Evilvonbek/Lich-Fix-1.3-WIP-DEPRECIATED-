using System;
using UnityModManagerNet;

namespace LichFix
{
	// Token: 0x0200000B RID: 11
	public class Settings : UnityModManager.ModSettings
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00004D05 File Offset: 0x00002F05
		public override void Save(UnityModManager.ModEntry modEntry)
		{
			UnityModManager.ModSettings.Save<Settings>(this, modEntry);
		}

		// Token: 0x04000007 RID: 7
		public int corruptedBloodRange = 5;

		// Token: 0x04000008 RID: 8
		public bool allowCorruptedBloodToSelective = true;

		// Token: 0x04000009 RID: 9
		public bool fixBlessingOfUnlifeDoubleSavingWithWorldCrawl = false;

		// Token: 0x0400000A RID: 10
		public bool applySaveOnCorruptedBloodExplosion = true;

        // Token: 0x0400000B RID: 11
        //public bool allowLichAuraAffectLivingWithBlessingOfUnlife = true;

        // Token: 0x0400000C RID: 12
        public bool removeDeathAndGazeDescriptorOnEyeOfTheBodak = true;

		// Token: 0x0400000D RID: 13
		public bool allowEclipseChillDCBaseOnCharacterLevel = true;

		// Token: 0x0400000E RID: 14
		public bool allowTaintedSneakAttackDCBaseOnCharacterLevel = true;

		// Token: 0x0400000F RID: 15
		public bool setMaximumDamageOnNegativeEruption = true;

		// Token: 0x04000010 RID: 16
		public static UnityModManager.ModEntry ModEntry;

		// Token: 0x04000011 RID: 17
		public bool allowLichAuraStr = true;

		// Token: 0x04000012 RID: 18
		public bool allowLichAuraDex = true;

		// Token: 0x04000013 RID: 19
		public bool allowLichAuraCha = true;

		// Token: 0x04000014 RID: 20
		public bool allowLichAuraInt = false;

		// Token: 0x04000015 RID: 21
		public bool allowLichAuraWis = false;
	}
}