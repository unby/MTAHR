using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;

namespace ManagementGui.ViewModel.Validation
{
    /// <summary>
    /// A base classe for ViewModel classes which supports validation using IDataErrorInfo interface. Properties must defines
    /// validation rules by using validation attributes defined in System.ComponentModel.DataAnnotations.
    /// </summary>
    public class ValidationViewModelBase : ViewModelBase, IDataErrorInfo, IValidationExceptionHandler
    {
        private readonly Dictionary<string, Func<ValidationViewModelBase, object>> propertyGetters;
        private readonly Dictionary<string, ValidationAttribute[]> validators;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public string this[string propertyName]
        {
            get
            {
                if (this.propertyGetters.ContainsKey(propertyName))
                {
                    var propertyValue = this.propertyGetters[propertyName](this);
                    var errorMessages = this.validators[propertyName]
                        .Where(v => !v.IsValid(propertyValue))
                        .Select(v => v.ErrorMessage).ToArray();
                    ValidateProperties();
                    return string.Join(Environment.NewLine, errorMessages);
                }

                return string.Empty;
            }
        }
        private void ValidateProperties()
        {
            this.AllPropertiesValid = TotalPropertiesWithValidationCount == ValidPropertiesCount;
        }
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get
            {
                var errors = from validator in this.validators
                             from attribute in validator.Value
                             where !attribute.IsValid(this.propertyGetters[validator.Key](this))
                             select attribute.ErrorMessage;

                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        /// <summary>
        /// Gets the number of properties which have a validation attribute and are currently valid
        /// </summary>
        public int ValidPropertiesCount
        {
            get
            {
                var query = from validator in this.validators
                            where validator.Value.All(attribute => attribute.IsValid(this.propertyGetters[validator.Key](this)))
                            select validator;

                var count = query.Count() - this.validationExceptionCount;
                return count;
            }
        }
        
        /// <summary>
        /// Gets the number of properties which have a validation attribute
        /// </summary>
        public int TotalPropertiesWithValidationCount
        {
            get
            {
                return this.validators.Count();
            }
        }

        public ValidationViewModelBase()
        {
            this.validators = this.GetType()
                .GetProperties()
                .Where(p => this.GetValidations(p).Length != 0)
                .ToDictionary(p => p.Name, p => this.GetValidations(p));

            this.propertyGetters = this.GetType()
                .GetProperties()
                .Where(p => this.GetValidations(p).Length != 0)
                .ToDictionary(p => p.Name, p => this.GetValueGetter(p));
        }

        private ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        private Func<ValidationViewModelBase, object> GetValueGetter(PropertyInfo property)
        {
            return new Func<ValidationViewModelBase, object>(viewmodel => property.GetValue(viewmodel, null));
        }

        private int validationExceptionCount;

        public void ValidationExceptionsChanged(int count)
        {
            this.validationExceptionCount = count;
            this.RaisePropertyChanged("ValidPropertiesCount");
        }


        #region FormView

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            this.PropertyChangedCompleted(propertyName);
            base.RaisePropertyChanged(propertyName);
        }

        private bool _allPropertiesValid;

        private readonly string formName;

        /// <summary>
        /// Gets a value indicating whether the form is valid in its current state. If all properties
        /// wich validation are valid, this property returns true.
        /// </summary>
        public bool AllPropertiesValid
        {
            get
            {
                return this._allPropertiesValid;
            }
            protected set
            {
                this._allPropertiesValid = value;
                this.RaisePropertyChanged("AllPropertiesValid");
            }
        }

        /// <summary>
        /// Gets the name of the form.
        /// </summary>
        public string FormName
        {
            get
            {
                return this.formName;
            }
        }


        protected void PropertyChangedCompleted(string propertyName)
        {
            // test prevent infinite loop while settings AllPropertiesValid 
            // (which causes an PropertyChanged to be raised)
            if (propertyName != "AllPropertiesValid")
            {
                // update the AllPropertiesValid status
                if (string.IsNullOrEmpty(this.Error) && this.ValidPropertiesCount == this.TotalPropertiesWithValidationCount)
                {
                    this.AllPropertiesValid = true;
                }
                else
                {
                    this.AllPropertiesValid = false;
                }
            }
        }
#endregion
    }
}
