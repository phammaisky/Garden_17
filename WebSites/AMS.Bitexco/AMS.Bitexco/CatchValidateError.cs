namespace AMS
{
    public class CatchValidateError
    {
        //try
        //   {
        //       db.SaveChanges();
        //       return Json(new { success = true });
        //   }
        //   catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //   {
        //       var errorMessages = ex.EntityValidationErrors
        //       .SelectMany(x => x.ValidationErrors)
        //       .Select(x => x.ErrorMessage);

        //       // Join the list to a single string.
        //       var fullErrorMessage = string.Join("; ", errorMessages);

        //       // Combine the original exception message with the new one.
        //       var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //       // Throw a new DbEntityValidationException with the improved exception message.
        //       throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //   }
    }
}