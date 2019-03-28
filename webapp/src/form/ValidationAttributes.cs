using System;

namespace webapp
{
    public class NotEmptyAttribute : Attribute
    {
        public static string ErrorString = "form.validation.errors.NotEmpty";
    }

    public class BoundAttribute : Attribute
    {
        
    }
}