using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Models;

namespace SF11_2.Custom
{
	public class SubscribeFormViewModelCustom : SubscribeFormViewModel
	{
		[Required]
		[CheckBoxRequired(ErrorMessage = "You must agree to the terms")]
		public bool TermsAgreement { get; set; }
	}

	public class CheckBoxRequired : ValidationAttribute, IClientValidatable
	{
		public override bool IsValid(object value)
		{
			if (value is bool)
			{
				return (bool)value;
			}

			return false;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule
			{
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
				ValidationType = "checkboxrequired"
			};
		}
	}
}
