﻿using RomUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1Lib
{
	public enum StartingItemSet
	{
		[Description("None")]
		None,

		[Description("SafetyBit (1 Tent, 1 Pure, 1 Soft)")]
		SafetyBit,

		[Description("Beggars Choice (5 Tents, 20 Heals, 2 Pures, 1 Softs)")]
		BeggarsChoice,

		[Description("Just Tents (10 Tents)")]
		JustTents,

		[Description("Explorer Starting Pack (10 Tents, 50 Heals, 4 Pures, 2 Softs)")]
		ExplorerStartingPack,

		[Description("Real Estate (15 Tents, 3 Cabins, 1 House)")]
		RealEstate,

		[Description("Warriors Standard (15 Tents, 3 Cabins, 1 House, 50 Heals, 6 Pures, 3 Softs)")]
		WarriorsStandard,

		[Description("Royal Packmule (50 Tents, 15 Cabins, 5 Houses, 99 Heals, 10 Pures, 5 Softs)")]
		RoyalPackmule,

		[Description("Duckling Boon (99 Tents, 99 Cabins, 99 Houses, 99 Heals, 99 Pures, 99 Softs)")]
		DucklingBoon,

		[Description("R. Beggars Choice (0-5 Tents, 0-20 Heals, 0-2 Pures, 0-1 Softs)")]
		RandomizedBeggarsChoice,

		[Description("R. Explorer Starting Pack (1-10 Tents, 0-50 Heals, 0-4 Pures, 0-2 Softs)")]
		RandomizedExplorerStartingPack,

		[Description("R. Warriors Standard (3-15 Tents, 0-3 Cabins, 0-1 House, 20-50 Heals, 2-6 Pures, 1-3 Softs)")]
		RandomizedWarriorsStandard,

		[Description("R. Royal Packmule (10-50 Tents, 5-15 Cabins, 1-5 Houses, 30-99 Heals, 5-10 Pures, 2-5 Softs)")]
		RandomizedRoyalPackmule,

		[Description("R. High Rolling (35-99 Tents, 10-25 Cabins, 5-10 Houses, 30-99 Heals, 10-20 Pures, 5-10 Softs)")]
		RandomizedHighRolling,

		[Description("Randomized (0-99 Tents, 0-99 Cabins, 0-99 Houses, 0-99 Heals, 0-20 Pures, 0-10 Softs)")]
		Randomized,
	}

	public class StartingInventory
	{
		MT19337 rng;
		Flags flags;
		FF1Rom rom;

		StartingItems ItemData;

		public StartingInventory(MT19337 _rng, Flags _flags, FF1Rom _rom)
		{
			rng = _rng;
			flags = _flags;
			rom = _rom;

			ItemData = new StartingItems(rom);
		}

		public void SetStartingInventory()
		{
			ItemData.LoadTable();

			foreach (var e in StartingItemSetDic[flags.StartingItemSet])
			{
				if (e.Cnt.HasValue)
				{
					ItemData[e.Item] = (byte)e.Cnt;
				}
				else
				{
					var min = e.Min;
					var max = e.Max;
					var rmin = e.RMin ?? min;
					var rmax = e.RMax ?? max;

					ItemData[e.Item] = (byte)Math.Min(Math.Max(rng.Between(rmin, rmax), min), max);
				}
			}

			ItemData.StoreTable();
		}


		private class StartingItem
		{
			public Item Item { get; set; }

			public int? Cnt { get; set; }

			public int Min { get; set; }

			public int Max { get; set; }

			public int? RMin { get; set; }

			public int? RMax { get; set; }
		}

		private static Dictionary<StartingItemSet, StartingItem[]> StartingItemSetDic = new System.Collections.Generic.Dictionary<StartingItemSet, StartingItem[]>
		{
			{ StartingItemSet.None, Array.Empty<StartingItem>() },
			{ StartingItemSet.SafetyBit, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 1 },
					new StartingItem { Item = Item.Pure, Cnt = 1 },
					new StartingItem { Item = Item.Soft, Cnt = 1 },
				}
			},
			{ StartingItemSet.BeggarsChoice, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 5 },
					new StartingItem { Item = Item.Heal, Cnt = 20 },
					new StartingItem { Item = Item.Pure, Cnt = 2 },
					new StartingItem { Item = Item.Soft, Cnt = 1 },
				}
			},
			{ StartingItemSet.JustTents, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 10 },
				}
			},
			{ StartingItemSet.ExplorerStartingPack, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 10 },
					new StartingItem { Item = Item.Heal, Cnt = 50 },
					new StartingItem { Item = Item.Pure, Cnt = 4 },
					new StartingItem { Item = Item.Soft, Cnt = 2 },
				}
			},
			{ StartingItemSet.RealEstate, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 15 },
					new StartingItem { Item = Item.Cabin, Cnt = 5 },
					new StartingItem { Item = Item.House, Cnt = 1 },
				}
			},
			{ StartingItemSet.WarriorsStandard, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 15 },
					new StartingItem { Item = Item.Cabin, Cnt = 5 },
					new StartingItem { Item = Item.House, Cnt = 1 },
					new StartingItem { Item = Item.Heal, Cnt = 50 },
					new StartingItem { Item = Item.Pure, Cnt = 6 },
					new StartingItem { Item = Item.Soft, Cnt = 3 },
				}
			},
			{ StartingItemSet.RoyalPackmule, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 50 },
					new StartingItem { Item = Item.Cabin, Cnt = 15 },
					new StartingItem { Item = Item.House, Cnt = 5 },
					new StartingItem { Item = Item.Heal, Cnt = 99 },
					new StartingItem { Item = Item.Pure, Cnt = 10 },
					new StartingItem { Item = Item.Soft, Cnt = 5 },
				}
			},
			{ StartingItemSet.DucklingBoon, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Cnt = 99 },
					new StartingItem { Item = Item.Cabin, Cnt = 99 },
					new StartingItem { Item = Item.House, Cnt = 99 },
					new StartingItem { Item = Item.Heal, Cnt = 99 },
					new StartingItem { Item = Item.Pure, Cnt = 99 },
					new StartingItem { Item = Item.Soft, Cnt = 99 },
				}
			},
			{ StartingItemSet.RandomizedBeggarsChoice, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 0, Max = 5, RMin = -1, RMax = 6 },
					new StartingItem { Item = Item.Heal, Min = 0, Max = 20 },
					new StartingItem { Item = Item.Pure, Min = 0, Max = 2 },
					new StartingItem { Item = Item.Soft, Min = 0, Max = 1 },
				}
			},
			{ StartingItemSet.RandomizedExplorerStartingPack, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 1, Max = 10, RMin = -2, RMax = 12 },
					new StartingItem { Item = Item.Heal, Min = 0, Max = 50 },
					new StartingItem { Item = Item.Pure, Min = 0, Max = 4 },
					new StartingItem { Item = Item.Soft, Min = 0, Max = 2 },
				}
			},
			{ StartingItemSet.RandomizedWarriorsStandard, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 3, Max = 15 },
					new StartingItem { Item = Item.Cabin, Min = 0, Max = 3 },
					new StartingItem { Item = Item.House, Min = 0, Max = 1 },
					new StartingItem { Item = Item.Heal, Min = 20, Max = 50 },
					new StartingItem { Item = Item.Pure, Min = 2, Max = 6 },
					new StartingItem { Item = Item.Soft, Min = 1, Max = 3 },
				}
			},
			{ StartingItemSet.RandomizedRoyalPackmule, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 10, Max = 50 },
					new StartingItem { Item = Item.Cabin, Min = 5, Max = 15 },
					new StartingItem { Item = Item.House, Min = 1, Max = 5 },
					new StartingItem { Item = Item.Heal, Min = 30, Max = 99 },
					new StartingItem { Item = Item.Pure, Min = 5, Max = 10 },
					new StartingItem { Item = Item.Soft, Min = 2, Max = 5 },
				}
			},
			{ StartingItemSet.RandomizedHighRolling, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 30, Max = 99 },
					new StartingItem { Item = Item.Cabin, Min = 10, Max = 25 },
					new StartingItem { Item = Item.House, Min = 5, Max = 10 },
					new StartingItem { Item = Item.Heal, Min = 30, Max = 99 },
					new StartingItem { Item = Item.Pure, Min = 10, Max = 20 },
					new StartingItem { Item = Item.Soft, Min = 5, Max = 10 },
				}
			},
			{ StartingItemSet.Randomized, new StartingItem[]
				{
					new StartingItem { Item = Item.Tent, Min = 0, Max = 99, RMin = -30, RMax = 114 },
					new StartingItem { Item = Item.Cabin, Min = 0, Max = 99, RMin = -30, RMax = 114 },
					new StartingItem { Item = Item.House, Min = 0, Max = 99, RMin = -30, RMax = 114 },
					new StartingItem { Item = Item.Heal, Min = 0, Max = 99, RMin = -15, RMax = 114 },
					new StartingItem { Item = Item.Pure, Min = 0, Max = 20, RMin = -3, RMax = 23 },
					new StartingItem { Item = Item.Soft, Min = 0, Max = 10, RMin = -2, RMax = 12 },
				}
			},
		};
	}
}