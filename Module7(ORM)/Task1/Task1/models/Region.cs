using LinqToDB.Mapping;
using System.Collections.Generic;

namespace Task1.models
{
	[Table(Schema = "dbo", Name = "Region")]
	public partial class Region
	{
		[PrimaryKey, NotNull]
		public int RegionID { get; set; }

		[Column, NotNull]
		public string RegionDescription { get; set; }

		[Association(ThisKey = "RegionID", OtherKey = "RegionID", CanBeNull = true, IsBackReference = true)]
		public IEnumerable<Territory> Territories { get; set; }
	}
}
