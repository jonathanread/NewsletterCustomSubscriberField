using SF11_2.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Frontend;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Models;

namespace SF11_2
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
		}

		private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
		{
			FrontendModule.Current.DependencyResolver.Rebind<ISubscribeFormModel>().To<SubscribeFormModelCustom>();
		}
	}
}