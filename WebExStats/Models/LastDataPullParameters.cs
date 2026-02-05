using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Models
{
    public class LastDataPullParameters
    {
        [Required(ErrorMessage = "PullType is required")]
        [MaxLength(50, ErrorMessage = "PullType maximum length is 50 characters")]
        public string PullType { get; set; }
    }
}
