using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain
{
    public class AddToRoleModel
    {
        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string SelectedRole { get; set; }
    }
}
