using System.Collections.Generic;
using M.Core.Entity;

namespace M.Core.Dto
{
    public class RoleMenuAuthDto
    {
        public string RoleId { get; set; }

        public List<Sys_rolemenuauth> Children { get; set; }
    }
}