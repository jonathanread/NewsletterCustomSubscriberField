# Newsletter Custom Subscriber Fields
This project is a working example of adding a custom field to the subscriber object and implementing it in the subscribe form view.  Also implements 'required' checkbox in subscriber form custom.

## Getting Started
### What you will need
- Sitefinity 11.2 license
- Visual Studio
- SQL Express

### Using the Project
- Clone the project to your machine
- Restore nuget packages
- Build/run the project
- Install your Sitefinity license
- Admin user: test@admin.com
- Admin password: password

### Areas of customization
- Added **TermsAccepted** field to the subscriber MetaFields
- Added Custom MVC view in Bootstrap resource package, *SubscribeForm/SubscribeFormCustom* with implemented client side validation and checkbox
- ~/Custom holds override Models for subriber widget and implemention of adding custom meta field

### Other things to consider
- These custom fields are not implemented into the backend screen like subsriber listing or editing/creating a subscriber

## Supporting Information
- [Knowledge Base Custom Fields for Subscribers](https://knowledgebase.progress.com/articles/Article/using-custom-fields-in-the-subscribeform-widget)
- [Extending built-in widget models](https://www.progress.com/documentation/sitefinity-cms/extend-the-model-of-built-in-widgets-mvc)
- [Creating Custom Fields for Subscribers](https://www.progress.com/documentation/sitefinity-cms/for-developers-create-custom-fields-for-subscribers)



