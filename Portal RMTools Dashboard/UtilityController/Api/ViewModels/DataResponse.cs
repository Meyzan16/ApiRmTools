namespace Api.ViewModels
{
    public class DropdownDataResponse
    {
        public int Appownerid { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Rolename { get; set; }
    }

    public class ListDataDropDown<T>
    {
        public List<T> items { get; set; }
        public int total_count { get; set; }
    }

    public class DropdownCalonDebiturReq_VM
    {
        public string Parameter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PegawaiLoginId { get; set; }
    }

    public class DropdownMasterMenu_VM
    {
        public int Id { get; set;}
        public string Name { get; set;}
    }

 

}
