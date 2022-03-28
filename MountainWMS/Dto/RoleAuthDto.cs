using System.Collections.Generic;
using M.Core.Entity;

namespace M.Core.Dto
{
    public class RoleAuthDto
    {
        public string RoleId { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }

        public List<RoleAuthChildrenDto> Children { get; set; }
    }
}