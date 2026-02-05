using System.ComponentModel.DataAnnotations;

namespace Stats.Models;

public class EmployeeTotalTimeParameters
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "DisplayName is required")]
    [MaxLength(100, ErrorMessage = "DisplayName maximum length is 100 characters")]
    public string DisplayName { get; set; }
}