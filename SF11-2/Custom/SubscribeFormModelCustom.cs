using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Models;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Newsletters;
using Telerik.Sitefinity.Newsletters.Model;
using Telerik.Sitefinity.Services;

namespace SF11_2.Custom
{
	public class SubscribeFormModelCustom : ISubscribeFormModel
	{
		#region Properties
		/// <inheritdoc />
		public string Provider { get; set; }

		/// <inheritdoc />
		public Guid SelectedMailingListId { get; set; }

		/// <inheritdoc />
		public SuccessfullySubmittedForm SuccessfullySubmittedForm { get; set; }

		/// <inheritdoc />
		public string CssClass { get; set; }

		/// <inheritdoc />
		public Guid PageId { get; set; }
		#endregion

		#region Public and virtual methods

		/// <inheritdoc />
		public SubscribeFormViewModel CreateViewModel()
		{
			return new SubscribeFormViewModelCustom
			{
				CssClass = this.CssClass
			};
		}

		/// <inheritdoc />
		public virtual bool AddSubscriber(SubscribeFormViewModel viewModelOld, out string error)
		{
			var agreed = SystemManager.CurrentHttpContext.Request.Params["TermsAgreement"].Split(',').Count() > 1;
			var viewModel = new SubscribeFormViewModelCustom()
			{
				FirstName = viewModelOld.FirstName,
				LastName = viewModelOld.LastName,
				Email = viewModelOld.Email,
				CssClass = viewModelOld.CssClass,
				RedirectPageUrl = viewModelOld.RedirectPageUrl,

				// custom - getting the property values from the request
				TermsAgreement = agreed

			};

			error = string.Empty;
			if (viewModel.TermsAgreement)
			{
				if (NewsletterValidator.IsValidEmail(viewModel.Email))
				{
					var newslettersManager = NewslettersManager.GetManager(this.Provider);

					// check if subscriber exists
					var email = viewModel.Email.ToLower();

					IQueryable<Subscriber> matchingSubscribers = newslettersManager.GetSubscribers().Where(s => s.Email == email);
					bool subscriberAlreadyInList = false;
					foreach (Subscriber s in matchingSubscribers)
					{
						if (s.Lists.Any(ml => ml.Id == this.SelectedMailingListId))
						{
							subscriberAlreadyInList = true;
							break;
						}
					}

					if (subscriberAlreadyInList)
					{
						Subscriber subscriber = matchingSubscribers.FirstOrDefault();
						if(subscriber != null)
						{
							//update their name
							subscriber.FirstName = viewModel.FirstName;
							subscriber.LastName = viewModel.LastName;

							//update their terms acceptance
							subscriber.SetValue("TermsAccepted", viewModel.TermsAgreement);
							newslettersManager.SaveChanges();
						}
						// If the email has already been subscribed, consider it as success.
						return true;
					}
					else
					{
						Subscriber subscriber = matchingSubscribers.FirstOrDefault();
						if (subscriber == null)
						{
							subscriber = newslettersManager.CreateSubscriber(true);
							subscriber.Email = viewModel.Email;
							subscriber.FirstName = viewModel.FirstName ?? string.Empty;
							subscriber.LastName = viewModel.LastName ?? string.Empty;

							// custom - setting the property values 
							subscriber.SetValue("TermsAccepted", viewModel.TermsAgreement);
						}
						else
						{
							subscriber.FirstName = viewModel.FirstName ?? string.Empty;
							subscriber.LastName = viewModel.LastName ?? string.Empty;
							//update acceptance of terms
							subscriber.SetValue("TermsAccepted", viewModel.TermsAgreement);
						}

						// check if the mailing list exists
						if (newslettersManager.Subscribe(subscriber, this.SelectedMailingListId))
						{
							if (this.SuccessfullySubmittedForm == SuccessfullySubmittedForm.OpenSpecificPage)
							{
								viewModel.RedirectPageUrl = HyperLinkHelpers.GetFullPageUrl(this.PageId);
							}

							newslettersManager.SaveChanges();

							return true;
						}
					}
				}
				error = Res.Get<SubscribeFormResources>().EmailAddressErrorMessageResourceName;
			}
			error = "You must agree to the terms";
			return false;
		}
		#endregion
	}
}
