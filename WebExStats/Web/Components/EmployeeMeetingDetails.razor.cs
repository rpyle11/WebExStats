using Microsoft.AspNetCore.Components;

namespace Stats.Web.Components
{
    public partial class EmployeeMeetingDetails
    {

        public string Email { get; set; }

  
        public string DisplayName { get; set; }

   
        public DateTime StartDate { get; set; }

  
        public DateTime EndDate { get; set; }

    
        public int EmployeeTotalTime { get; set; }

        private bool DialogVisible { get; set; }

        public string DialogTitle { get; set; }

        

        public void ShowDialog()
        {
          
            DialogVisible = true;

            StateHasChanged();
        }

 

       
    }
}
