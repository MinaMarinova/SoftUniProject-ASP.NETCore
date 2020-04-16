namespace FreelancePool.Web.Infrastructure.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class NotEmptyCollectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count != 0;
            }

            return false;
        }
    }
}
