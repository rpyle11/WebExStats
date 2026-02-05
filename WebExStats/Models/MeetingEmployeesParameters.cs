using System.ComponentModel.DataAnnotations;

namespace Stats.Models;

public class MeetingEmployeesParameters
{
    [Required(ErrorMessage = "MeetingId is required")]
    [MaxLength(500, ErrorMessage = "MeetingId maximum length is 500 characters")]
    public string MeetingId { get; set; }
}