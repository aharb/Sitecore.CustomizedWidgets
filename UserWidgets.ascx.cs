using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Newtonsoft.Json;

using SABB.Utilities.Widgets;
using SABB.Entities.Widgets;
using Sitecore.SecurityModel;
using SABB.Utilities.Utils ;



namespace SABB.Website.layouts.SABB.SubLayouts.Home
{
    public partial class UserWidgets : System.Web.UI.UserControl
    {
        #region Private Attributes
        private const int PLACEHOLDERS_COUNT = 8;
        private const string PLACEHOLDER_ID_CONVENTION = "phWidget{NUMBER}";
        private string UserJson = "";
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            using (new SecurityDisabler())
            {
                var promotionWidget = LoadControl(UserControlPaths.Promotion);
                var calenderWidget = LoadControl(UserControlPaths.Calender);
                var newsWidget = LoadControl(UserControlPaths.News);
                var internalVacancesWidget = LoadControl(UserControlPaths.InternalVacancies);
                var pollsWidget = LoadControl(UserControlPaths.Polls);
                var circularWidget = LoadControl(UserControlPaths.Circular);
                var linksWidget = LoadControl(UserControlPaths.Links);
                var projectsWidget = LoadControl(UserControlPaths.Projects);
                var myClassifiedsWidget = LoadControl(UserControlPaths.MyClassifieds);
                var staffDirectoryWidget = LoadControl(UserControlPaths.StaffDirectory);
                var topSearchesWidget = LoadControl(UserControlPaths.TopSearches);
                var departmentWidget = LoadControl(UserControlPaths.Departments);
                var documentWidget = LoadControl(UserControlPaths.DocumentLibrary);
                var faqWidget = LoadControl(UserControlPaths.FAQ);
                var groupTelWidget = LoadControl(UserControlPaths.GroupTel);
                var hSBCvacanciesWidget = LoadControl(UserControlPaths.HSBCvacancies);
                var InternalVacanciesWidget = LoadControl(UserControlPaths.InternalVacancies);

               

               // *** sample Json format ***
               // [{WidgetType: 1, Order: 1, PlaceholderOrder: 1},{WidgetType: 6, Order: 2, PlaceholderOrder: 1},{WidgetType: 2, Order: 1, PlaceholderOrder: 4},{WidgetType: 3, Order: 1, PlaceholderOrder: 3},{WidgetType: 4, Order: 2, PlaceholderOrder: 3}]";

                Sitecore.Security.Accounts.User currentUser = Sitecore.Context.User;
              

               UserJson= currentUser.Profile.GetCustomProperty(Constants.UserCustomProperty.UserWidgets);

               if (string.IsNullOrEmpty(UserJson))
                   UserJson = Sitecore.Configuration.Settings.GetSetting("UserWidgetsDefault");

               var userWidgets = JsonConvert.DeserializeObject<List<Widget>>(UserJson);

                // start grouping the widgets between the placeholders
                for (int i = 1; i <= PLACEHOLDERS_COUNT; i++)
                {
                    var widgetsInCurrentPlaceholder = userWidgets.Where(x => x.PlaceholderOrder == i).ToList();

                    if (widgetsInCurrentPlaceholder != null && widgetsInCurrentPlaceholder.Count() > 0)
                    {
                        var widgetsInCurrentPlaceholderByOrder = widgetsInCurrentPlaceholder.OrderBy(w => w.Order).ToList();

                        string placeholerId = PLACEHOLDER_ID_CONVENTION.Replace("{NUMBER}", i.ToString());
                        PlaceHolder tempPlaceholder = (PlaceHolder)this.FindControl(placeholerId);

                        foreach (var widget in widgetsInCurrentPlaceholderByOrder)
                        {
                            switch (widget.WidgetType)
                            {
                                case WidgetType.Calender:
                                    tempPlaceholder.Controls.Add(calenderWidget);
                                    break;
                                case WidgetType.Circular:
                                    tempPlaceholder.Controls.Add(circularWidget);
                                    break;
                                case WidgetType.Departments:
                                    tempPlaceholder.Controls.Add(departmentWidget);
                                    break;
                                case WidgetType.DocumentLibrary:
                                    tempPlaceholder.Controls.Add(documentWidget);
                                    break;
                                case WidgetType.FAQ:
                                    tempPlaceholder.Controls.Add(faqWidget);
                                    break;
                                case WidgetType.GroupTel:
                                    tempPlaceholder.Controls.Add(groupTelWidget);
                                    break;
                                case WidgetType.HSBCvacancies:
                                    tempPlaceholder.Controls.Add(hSBCvacanciesWidget);
                                    break;
                                case WidgetType.InternalVacancies:
                                    tempPlaceholder.Controls.Add(InternalVacanciesWidget);
                                    break;
                                case WidgetType.Links:
                                    tempPlaceholder.Controls.Add(linksWidget);
                                    break;
                                case WidgetType.MyClassifieds:
                                    tempPlaceholder.Controls.Add(myClassifiedsWidget);
                                    break;
                                case WidgetType.News:
                                    tempPlaceholder.Controls.Add(newsWidget);
                                    break;
                                case WidgetType.Polls:
                                    tempPlaceholder.Controls.Add(pollsWidget);
                                    break;
                                case WidgetType.Projects:
                                    tempPlaceholder.Controls.Add(projectsWidget);
                                    break;
                                case WidgetType.Promotion:
                                    tempPlaceholder.Controls.Add(promotionWidget);
                                    break;
                                case WidgetType.StaffDirectory:
                                    tempPlaceholder.Controls.Add(staffDirectoryWidget);
                                    break;
                                case WidgetType.TopSearches:
                                    tempPlaceholder.Controls.Add(topSearchesWidget);
                                    break;
                            }
                        }
                    }
                }



                //***** code to fill removed widgets ******//
                // check if the user does not have a custom display,  so we should display all the removed. 

                string currentUserWidgetConfiguration = currentUser.Profile.GetCustomProperty(Constants.UserCustomProperty.UserWidgets);
                string defaultWidgetsConfugration = Sitecore.Configuration.Settings.GetSetting("UserWidgetsDefault");

                if (!string.IsNullOrEmpty(currentUserWidgetConfiguration))
                { 
                    // the user has some customizations here, so we should add the removed widgets if the user have. 

                    // deserilze JSONs strings, it's easier to deal with LINQ !
                    var currentUserWidgets = JsonConvert.DeserializeObject<List<Widget>>(currentUserWidgetConfiguration);
                    var defaultUserWidgets = JsonConvert.DeserializeObject<List<Widget>>(defaultWidgetsConfugration);

                    // My awsome code here do the trick, Simply it's get the items that we want to remove!  \m/ \m/ \m/
                    var widgetsToRemove = defaultUserWidgets.Where(w => !currentUserWidgets.Any(w2 => w2.WidgetType == w.WidgetType));

                    foreach (var widget in widgetsToRemove)
                    {
                        string placeholerId = PLACEHOLDER_ID_CONVENTION.Replace("{NUMBER}", widget.PlaceholderOrder.ToString());
                        PlaceHolder tempPlaceholder = (PlaceHolder)this.FindControl(placeholerId);

                       
                       int type =(int)  widget.WidgetType;

                        switch (widget.WidgetType)
                        {
                            case WidgetType.Calender:

                                HtmlControl widgetContainerCalender = (HtmlControl)calenderWidget.FindControl("widgetCalender");
                                widgetContainerCalender.Attributes.Add("class", widgetContainerCalender.Attributes["class"] + " widget-removed");

                                widgetContainerCalender.Attributes.Add("data-widget-type",type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerCalender);

                                break;

                            case WidgetType.Circular:
                                
                               
                                HtmlControl widgetContainerCircular = (HtmlControl)circularWidget.FindControl("widgetCircular");
                                widgetContainerCircular.Attributes.Add("class", widgetContainerCircular.Attributes["class"] + " widget-removed");

                                widgetContainerCircular.Attributes.Add("data-widget-type", type.ToString());

                                tempPlaceholder.Controls.Add(widgetContainerCircular);

                                break;

                            case WidgetType.Departments:

                                HtmlControl widgetContainerDepartments = (HtmlControl)departmentWidget.FindControl("widgetDepartments");
                                                              
                                widgetContainerDepartments.Attributes.Add("class", widgetContainerDepartments.Attributes["class"] + " widget-removed");

                                widgetContainerDepartments.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerDepartments);

                                break;

                            case WidgetType.DocumentLibrary:

                                HtmlControl widgetContainerDocumentLibrary = (HtmlControl)documentWidget.FindControl("widgetDocumentLibrary");
                                widgetContainerDocumentLibrary.Attributes.Add("class", widgetContainerDocumentLibrary.Attributes["class"] + " widget-removed");
                                widgetContainerDocumentLibrary.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerDocumentLibrary);

                           
                                break;

                            case WidgetType.FAQ:

                                HtmlControl widgetContainerFAQ = (HtmlControl)faqWidget.FindControl("widgetFAQ");
                                widgetContainerFAQ.Attributes.Add("class", widgetContainerFAQ.Attributes["class"] + " widget-removed");
                                widgetContainerFAQ.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerFAQ);

                                break;

                            case WidgetType.GroupTel:

                                HtmlControl widgetContainerGroupTel = (HtmlControl)groupTelWidget.FindControl("widgetGroupTel");
                                widgetContainerGroupTel.Attributes.Add("class", widgetContainerGroupTel.Attributes["class"] + " widget-removed");
                                widgetContainerGroupTel.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerGroupTel);


                                break;

                            case WidgetType.HSBCvacancies:

                                HtmlControl widgetContainerHSBCvacancies = (HtmlControl)hSBCvacanciesWidget.FindControl("widgetHSBCvacancies");
                                widgetContainerHSBCvacancies.Attributes.Add("class", widgetContainerHSBCvacancies.Attributes["class"] + " widget-removed");
                                widgetContainerHSBCvacancies.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerHSBCvacancies);

                                break;

                            case WidgetType.InternalVacancies:

                                HtmlControl widgetContainerInternalVacancies = (HtmlControl)InternalVacanciesWidget.FindControl("widgetInternalVacancies");
                                widgetContainerInternalVacancies.Attributes.Add("class", widgetContainerInternalVacancies.Attributes["class"] + " widget-removed");
                                widgetContainerInternalVacancies.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerInternalVacancies);

                                break;

                            case WidgetType.Links:

                                HtmlControl widgetContainerLinks = (HtmlControl)linksWidget.FindControl("widgetLinks");
                                widgetContainerLinks.Attributes.Add("class", widgetContainerLinks.Attributes["class"] + " widget-removed");
                                widgetContainerLinks.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerLinks);

                                break;

                            case WidgetType.MyClassifieds:

                                HtmlControl widgetContainerMyClassifieds = (HtmlControl)myClassifiedsWidget.FindControl("widgetLinksMyClassifieds");
                                widgetContainerMyClassifieds.Attributes.Add("class", widgetContainerMyClassifieds.Attributes["class"] + " widget-removed");
                                widgetContainerMyClassifieds.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerMyClassifieds);

                                break;

                            case WidgetType.News:

                                HtmlControl widgetContainerNews = (HtmlControl)newsWidget.FindControl("widgetNews");
                                widgetContainerNews.Attributes.Add("class", widgetContainerNews.Attributes["class"] + " widget-removed");
                                widgetContainerNews.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerNews);
;
                                break;

                            case WidgetType.Polls:

                                HtmlControl widgetContainerPolls = (HtmlControl)pollsWidget.FindControl("widgetPolls");
                                widgetContainerPolls.Attributes.Add("class", widgetContainerPolls.Attributes["class"] + " widget-removed");
                                widgetContainerPolls.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerPolls);

                                break;

                            case WidgetType.Projects:

                                HtmlControl widgetContainerProjects = (HtmlControl)projectsWidget.FindControl("widgetProjects");
                                widgetContainerProjects.Attributes.Add("class", widgetContainerProjects.Attributes["class"] + " widget-removed");
                                widgetContainerProjects.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerProjects);

                                break;

                            case WidgetType.Promotion:

                                HtmlControl widgetContainerPromotion = (HtmlControl)promotionWidget.FindControl("widgetPromotion");
                                widgetContainerPromotion.Attributes.Add("class", widgetContainerPromotion.Attributes["class"] + " widget-removed");
                                widgetContainerPromotion.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerPromotion);

                                break;

                            case WidgetType.StaffDirectory:

                                HtmlControl widgetContainerStaffDirectory = (HtmlControl)staffDirectoryWidget.FindControl("widgetStaffDirectory");
                                widgetContainerStaffDirectory.Attributes.Add("class", widgetContainerStaffDirectory.Attributes["class"] + " widget-removed");
                                widgetContainerStaffDirectory.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainerStaffDirectory);


                                break;

                            case WidgetType.TopSearches:

                                HtmlControl widgetContainertopSearchesWidget = (HtmlControl)topSearchesWidget.FindControl("widgetTopSearches");
                                widgetContainertopSearchesWidget.Attributes.Add("class", widgetContainertopSearchesWidget.Attributes["class"] + " widget-removed");
                                widgetContainertopSearchesWidget.Attributes.Add("data-widget-type", type.ToString());
                                tempPlaceholder.Controls.Add(widgetContainertopSearchesWidget);

                                tempPlaceholder.Controls.Add(topSearchesWidget);
                                break;
                        }
 
                    }
                }
            }

            
        }
        #endregion

        
    }
}