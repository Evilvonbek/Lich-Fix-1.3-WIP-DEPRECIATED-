using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LichFix
{
	// Token: 0x020000A3 RID: 163
	internal static class DescriptionTools
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x0005CB50 File Offset: 0x0005AD50
		public static string TagEncyclopediaEntries(string description)
		{
			string result = description;
			result = result.StripHTML();
			foreach (DescriptionTools.EncyclopediaEntry entry in DescriptionTools.EncyclopediaEntries)
			{
				foreach (string pattern in entry.Patterns)
				{
					result = result.ApplyTags(pattern, entry);
				}
			}
			return result;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0005CBD8 File Offset: 0x0005ADD8
		private static string ApplyTags(this string str, string from, DescriptionTools.EncyclopediaEntry entry)
		{
			string pattern = from.EnforceSolo().ExcludeTagged();
			IEnumerable<string> matches = (from m in Regex.Matches(str, pattern, RegexOptions.IgnoreCase).OfType<Match>()
										   select m.Value).Distinct<string>();
			foreach (string match in matches)
			{
				str = Regex.Replace(str, Regex.Escape(match).EnforceSolo().ExcludeTagged(), entry.Tag(match), RegexOptions.IgnoreCase);
			}
			return str;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0005CC88 File Offset: 0x0005AE88
		private static string StripHTML(this string str)
		{
			return Regex.Replace(str, "<.*?>", string.Empty);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0005CCAC File Offset: 0x0005AEAC
		private static string ExcludeTagged(this string str)
		{
			return "(?<!{g\\|Encyclopedia:\\w+}[^}]*)" + str + "(?![^{]*{\\/g})";
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0005CCD0 File Offset: 0x0005AED0
		private static string EnforceSolo(this string str)
		{
			return "(?<![\\w>]+)" + str + "(?![^\\s\\.,\"'<)]+)";
		}

		// Token: 0x04000C50 RID: 3152
		private static readonly DescriptionTools.EncyclopediaEntry[] EncyclopediaEntries = new DescriptionTools.EncyclopediaEntry[]
		{
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Strength",
				Patterns =
				{
					"Strength"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Dexterity",
				Patterns =
				{
					"Dexterity"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Constitution",
				Patterns =
				{
					"Constitution"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Intelligence",
				Patterns =
				{
					"Intelligence"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Wisdom",
				Patterns =
				{
					"Wisdom"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Charisma",
				Patterns =
				{
					"Charisma"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Ability_Scores",
				Patterns =
				{
					"Ability Scores?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Athletics",
				Patterns =
				{
					"Athletics"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Persuasion",
				Patterns =
				{
					"Persuasion"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Knowledge_World",
				Patterns =
				{
					"Knowledge \\(?World\\)?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Knowledge_Arcana",
				Patterns =
				{
					"Knowledge \\(?Arcana\\)?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Lore_Nature",
				Patterns =
				{
					"Lore \\(?Nature\\)?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Lore_Religion",
				Patterns =
				{
					"Lore \\(?Religion\\)?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Mobility",
				Patterns =
				{
					"Mobility"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Perception",
				Patterns =
				{
					"Perception"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Stealth",
				Patterns =
				{
					"Stealth"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Trickery",
				Patterns =
				{
					"Trickery"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Use_Magic_Device",
				Patterns =
				{
					"Use Magic Device",
					"UMD"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Race",
				Patterns =
				{
					"Race"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Alignment",
				Patterns =
				{
					"Alignment"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Caster_Level",
				Patterns =
				{
					"Caster Level",
					"CL"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "DC",
				Patterns =
				{
					"DC"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Saving_Throw",
				Patterns =
				{
					"Saving Throw"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Spell_Resistance",
				Patterns =
				{
					"Spell Resistance"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Spell_Fail_Chance",
				Patterns =
				{
					"Arcane Spell Failure"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Concentration_Checks",
				Patterns =
				{
					"Concentration Checks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Concealment",
				Patterns =
				{
					"Concealment"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Bonus",
				Patterns =
				{
					"Bonus(es)?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Speed",
				Patterns =
				{
					"Speed"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Flat_Footed_AC",
				Patterns =
				{
					"Flat Footed AC",
					"Flat Footed Armor Class"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Flat_Footed",
				Patterns =
				{
					"Flat Footed"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Armor_Class",
				Patterns =
				{
					"Armor Class",
					"AC"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Armor_Check_Penalty",
				Patterns =
				{
					"Armor Check Penalty"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Damage_Reduction",
				Patterns =
				{
					"DR"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Free_Action",
				Patterns =
				{
					"Free Action"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Swift_Action",
				Patterns =
				{
					"Swift Action"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Standard_Actions",
				Patterns =
				{
					"Standard Action"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Full_Round_Action",
				Patterns =
				{
					"Full Round Action"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Skills",
				Patterns =
				{
					"Skills? Checks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Combat_Maneuvers",
				Patterns =
				{
					"Combat Maneuvers?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "CMB",
				Patterns =
				{
					"Combat Maneuver Bonus",
					"CMB"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "CMD",
				Patterns =
				{
					"Combat Maneuver Defense",
					"CMD"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "BAB",
				Patterns =
				{
					"Base Attack Bonus",
					"BAB"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Incorporeal_Touch_Attack",
				Patterns =
				{
					"Incorporeal Touch Attacks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "TouchAttack",
				Patterns =
				{
					"Touch Attacks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "NaturalAttack",
				Patterns =
				{
					"Natural Attacks?",
					"Natural Weapons?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Attack_Of_Opportunity",
				Patterns =
				{
					"Attacks? Of Opportunity",
					"AoO"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Penalty",
				Patterns =
				{
					"Penalty"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Check",
				Patterns =
				{
					"Checks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Spells",
				Patterns =
				{
					"Spells?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Attack",
				Patterns =
				{
					"Attacks?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Feat",
				Patterns =
				{
					"Feats?"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Charge",
				Patterns =
				{
					"Charge"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Critical",
				Patterns =
				{
					"Critical Hit"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Fast_Healing",
				Patterns =
				{
					"Fast Healing"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Temporary_HP",
				Patterns =
				{
					"Temporary HP"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Flanking",
				Patterns =
				{
					"Flanking",
					"Flanked"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Magic_School",
				Patterns =
				{
					"School of Magic"
				}
			},
			new DescriptionTools.EncyclopediaEntry
			{
				Entry = "Damage_Type",
				Patterns =
				{
					"Bludgeoning",
					"Piercing",
					"Slashing"
				}
			}
		};

		// Token: 0x02000123 RID: 291
		private class EncyclopediaEntry
		{
			// Token: 0x06000A3C RID: 2620 RVA: 0x0006AC74 File Offset: 0x00068E74
			public string Tag(string keyword)
			{
				return string.Concat(new string[]
				{
					"{g|Encyclopedia:",
					this.Entry,
					"}",
					keyword,
					"{/g}"
				});
			}

			// Token: 0x04001284 RID: 4740
			public string Entry = "";

			// Token: 0x04001285 RID: 4741
			public List<string> Patterns = new List<string>();
		}
	}
}
