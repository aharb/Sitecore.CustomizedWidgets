using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using SABB.Entities.Widgets;
using SABB.Utilities.Utils;

using Newtonsoft.Json;
using SABB.Utilities.Widgets;

namespace SABB.Website.WebServices
{
    /// <summary>
    /// Summary description for widgets
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class widgets : System.Web.Services.WebService
    {

        [WebMethod]
        public Boolean Sort(string widgets)
        {
            // validate the JSON: 
            // 1- check if the type is correct.
            // 2- check if the user send the correct placeholder ids
            if (!WidgetsValidator.Validate(widgets))
            {
                return false;
            }

            List<SABB.Entities.Widgets.Widget> userWidgets;

            try
            {
                // deserilaize the JSON sting to list of Widgets entity 
                userWidgets = JsonConvert.DeserializeObject<List<Widget>>(widgets);
            }
            catch (Exception ex)
            {

                // log the exception
                Sitecore.Diagnostics.Log.Error("Error in user widget sorting, The object is:" + widgets, ex, this);
                return false;
            }

            

            // serilize back to JSON string
            string widgetJson = JsonConvert.SerializeObject(userWidgets);

            // save the value in the user profile
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(Sitecore.Context.User.Name, true);

                user.Profile.SetCustomProperty(Constants.UserCustomProperty.UserWidgets, widgetJson);

                user.Profile.Save();

            }


            return true;
        }

        
        [WebMethod]
        public Boolean Remove(int widgetType)
        {
            // Check if the type is correct.
            if (!WidgetsValidator.ValidateWidgetType(widgetType))
            {
                return false;
            }

            // get sitecore context user 
            Sitecore.Security.Accounts.User currentUser = Sitecore.Context.User;

            // load widgets setting from the context user
            string userWidgetsJson = currentUser.Profile.GetCustomProperty(Constants.UserCustomProperty.UserWidgets);

            
            List<SABB.Entities.Widgets.Widget> userWidgets;
             
            try
            {
                // deserilaize the JSON sting to list of Widgets entity 
                userWidgets = JsonConvert.DeserializeObject<List<Widget>>(userWidgetsJson);
            }
            catch (Exception ex)
            {
                // log the exception
                Sitecore.Diagnostics.Log.Error("Error in user delete widgets, The object is:" + userWidgetsJson, ex, this);
                return false;
            }


            userWidgets.Remove(userWidgets.SingleOrDefault(w => w.WidgetType == (WidgetType)widgetType));

            // serilize back to JSON string
            string widgetJson = JsonConvert.SerializeObject(userWidgets);

            // save the value in the user profile 
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {

                Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(Sitecore.Context.User.Name, true);

                user.Profile.SetCustomProperty(Constants.UserCustomProperty.UserWidgets, widgetJson);

                user.Profile.Save();

            }

            return true;
        }

        [WebMethod]
        public Boolean Add(int widgetType)
        {
            
            // Check if the type is correct.
            if (!WidgetsValidator.ValidateWidgetType(widgetType))
            {
                return false;
            }

            // get sitecore context user 
            Sitecore.Security.Accounts.User currentUser = Sitecore.Context.User;

            // load widgets setting from the context user
            string userWidgetsJson = currentUser.Profile.GetCustomProperty(Constants.UserCustomProperty.UserWidgets);


            List<SABB.Entities.Widgets.Widget> userWidgets;

            try
            {
                // deserilaize the JSON sting to list of Widgets entity 
                userWidgets = JsonConvert.DeserializeObject<List<Widget>>(userWidgetsJson);
            }
            catch (Exception ex)
            {

                // log the exception
                Sitecore.Diagnostics.Log.Error("Error in user adding widgets, The object is:" + userWidgetsJson, ex, this);
                return false;
            }

            // validate if the widget already added, if not then add it
            if (!userWidgets.Any(w => w.WidgetType == (WidgetType)widgetType))
            {
                userWidgets.Add(new Widget { WidgetType = (WidgetType) widgetType, PlaceholderOrder = 1, Order = 1 });

                // convert to JSON string
                string widgetJson = JsonConvert.SerializeObject(userWidgets);

                // save the value in the user profile
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {

                    Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(Sitecore.Context.User.Name, true);

                    user.Profile.SetCustomProperty(Constants.UserCustomProperty.UserWidgets, widgetJson);

                    user.Profile.Save();

                }


                return true;
            }
            else
            {
                return false;
            }

 
        }
    }
}
