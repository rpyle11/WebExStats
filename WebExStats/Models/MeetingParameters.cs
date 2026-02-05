using System.ComponentModel.DataAnnotations;

namespace Stats.Models
{
    public class MeetingParameters : IValidatableObject
    {
        [Required(ErrorMessage = "FromDate is required")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "ToDate is required")]
        public DateTime? ToDate {get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (!FromDate.HasValue || !ToDate.HasValue) return errors;
            if (FromDate.Value > ToDate.Value)
            {
                errors.Add(new ValidationResult("ToDate Must greater than FromDate", new[] { nameof(ToDate), nameof(FromDate) }));
            }


            return errors;

        }
    }
}
