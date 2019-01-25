using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Newsletters.Model;

namespace SF11_2.Custom
{
	public static class SubscriberHelper
	{
		public static void CreateCustomSubscriberField()
		{
			var metadataManager = MetadataManager.GetManager();
			var dynamicType = metadataManager.GetMetaType(typeof(Subscriber));
			if (dynamicType == null)
				dynamicType = metadataManager.CreateMetaType(typeof(Subscriber));

			var title = "TermsAccepted";
			if (!dynamicType.Fields.Any(f => f.FieldName == title))
			{
				var metaField = CreateMetaField(metadataManager, title, "BIT", "BIT", typeof(bool).FullName);
				dynamicType.Fields.Add(metaField);
			}


			using (new ElevatedModeRegion(metadataManager))
			{
				metadataManager.SaveChanges();
			}
		}

		public static MetaField CreateMetaField(MetadataManager metadataManager, string title, string dbSQLType, string dbType, string clrType) {
			var metaField = metadataManager.CreateMetafield(title);
			metaField.Title = title;
			metaField.DBSqlType = dbSQLType;
			metaField.DBType = dbType;
			metaField.ClrType = clrType;

			return metaField;
		}
	}
}