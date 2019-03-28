using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using NLog;

namespace webapp.form
{
    public class Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        private readonly Type _type;
        private readonly PropertyInfo[] _members;

        private readonly Dictionary<PropertyInfo, object[]> _allPropertiesAttributes =
            new Dictionary<PropertyInfo, object[]>();

        public bool IsValid => Errors.Count == 0;

        protected Form() : this(null)
        {
        }

        protected Form(NameValueCollection requestBody)
        {
            _type = GetType();
            _members = _type.GetProperties();
            foreach (PropertyInfo propertyInfo in _members)
            {
                object[] propertyAttributes = propertyInfo.GetCustomAttributes(true);
                _allPropertiesAttributes.Add(propertyInfo, propertyAttributes);
                if (propertyAttributes.Length == 0) continue;

                if (requestBody == null) continue;

                propertyInfo.SetValue(this, requestBody[propertyInfo.Name]);
                ValidateAttributes(propertyAttributes, propertyInfo);
            }
        }

        public bool Validate()
        {
            Errors.Clear();
            foreach (PropertyInfo propertyInfo in _members)
            {
                object[] myAttributes = _allPropertiesAttributes[propertyInfo];
                ValidateAttributes(myAttributes, propertyInfo);
            }

            return IsValid;
        }

        public void AddFlashMessagesToSession(Session session)
        {           
            string typeString = _type.Name;
            
            /*
            foreach ((string key, List<string> value) in Errors)
            {
                session.AddFlashMessagesToList("formValidationErrors." + typeString + "." + key, value);
            }
            */

            session.AddFlash("form", this);
            /*
            foreach (PropertyInfo propertyInfo in _members)
            {
                session.AddFlash(typeString + "." + propertyInfo.Name, 
                    propertyInfo.GetValue(this).ToString());
            }
            */
        }

        private void ValidateAttributes(object[] myAttributes, PropertyInfo fieldInfo)
        {
            foreach (Attribute attribute in myAttributes)
            {
                if (attribute is NotEmptyAttribute)
                {
                    CheckNotEmptyAttribute(fieldInfo, _type);
                }
            }
        }

        private void CheckNotEmptyAttribute(PropertyInfo fieldInfo, Type type)
        {
            try
            {
                string value = (string) fieldInfo.GetValue(this);

                if (string.IsNullOrEmpty(value))
                {
                    AddError(fieldInfo.Name, NotEmptyAttribute.ErrorString);
                }
            }
            catch (InvalidCastException e)
            {
                Logger.Error(e, $"NotEmpty attribute found on property {fieldInfo.Name} in class " +
                                $"{type.Name}, but it can only be used on string properties.");
                throw;
            }
        }

        private void AddError(string fieldName, string error)
        {
            Errors.TryGetValue(fieldName, out List<string> fieldErrors);
            if (fieldErrors == null)
            {
                fieldErrors = new List<string>();
                Errors.Add(fieldName, fieldErrors);
            }

            fieldErrors.Add(error);
        }
    }
}