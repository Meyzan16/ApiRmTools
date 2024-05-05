using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{

    public class RequestCreated
    {
        [DataType(DataType.Text, ErrorMessage = "Type is required")]
        public string? Type { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Range(1, 99, ErrorMessage = "The length of the Value character column is two-digit")]
        public int? Value { get; set; }


    }
}
