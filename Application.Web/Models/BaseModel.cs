using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Application.Web.Models
{

    public abstract class BaseModel : IValidatableObject
    {


        #region Implementation of IValidatableObject
        public abstract System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
        #endregion


    }


    public class EnhancedMappedValidationResult<TEntity> : ValidationResult
    {
        /// <summary>
        /// Property in error on which the validation result error is assigned.
        /// Getter only because we want the exception to be set when the validation result
        /// is created.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public Expression<Func<TEntity, object>> Property { get; private set; }

        /// <summary>
        /// Initializes by setting the property and the error.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="errorMessage">The error message.</param>
        public EnhancedMappedValidationResult(Expression<Func<TEntity, object>> property, string errorMessage)
            : base(errorMessage, new List<string>())
        {
            Property = property;
            ((List<string>)base.MemberNames).Add(ExpressionHelper.GetExpressionText(Property));
        }
    }
}