using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class Principal_VM
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public DateTime? StartLogin { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool? IsActive { get; set; }

        public string? Uid { get; set; }

    }
}
