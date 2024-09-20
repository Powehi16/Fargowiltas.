using System;

namespace Fargowiltas;

internal class MutantSummonInfo
{
	internal float progression;

	internal string modSource;

	internal int itemId;

	internal Func<bool> downed;

	internal int price;

	internal MutantSummonInfo(float progression, int itemId, Func<bool> downed, int price)
	{
		this.progression = progression;
		this.itemId = itemId;
		this.downed = downed;
		this.price = price;
	}
}
