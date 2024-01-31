namespace Api.ViewModels
{

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public string Employee_Id { get; set; }
            public string Email { get; set; }
            public string Department { get; set; }
        }

        public class NavigationVM
        {
            public int Id { get; set; }
            public int? Type { get; set; }
            public string Name { get; set; }
            public string Route { get; set; }
            public int? Jumlah { get; set; }
            public int? Order { get; set; }
            public int? Visible { get; set; }
            public int? ParentNavigationId { get; set; }
            public DateTime? CreatedTime { get; set; }
            public DateTime? UpdatedTime { get; set; }
            public int? CreatedById { get; set; }
            public int? UpdatedById { get; set; }
            public string IconClass { get; set; }
            public bool? IsDeleted { get; set; }
            public int? DeletedById { get; set; }
            public DateTime? DeletedTime { get; set; }
            public int? Expanded { get; set; }
            public int? Activated { get; set; }
        }

        public class updateLogin_VM
        {
            public string npp { get; set; }
            public string dataLogBrowser { get; set; }
            public string dataLogOS { get; set; }
            public string ipAddress { get; set; }
            public string clientInfo { get; set; }
            public int isSukses { get; set; }
        }
    
}
