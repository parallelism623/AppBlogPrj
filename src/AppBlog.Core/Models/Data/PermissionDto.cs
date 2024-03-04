using Core.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
    public class PermissionDto
    {
        public string RoleId {  get; set; }   
        public IList<RoleClaimsDto> Claims { get; set; }
    }
}
