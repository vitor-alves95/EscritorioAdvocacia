namespace EscritorioAdvocacia.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
   
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; } // O "checked" do checkbox
    }
}