using System.Collections.Generic;
using M.Core.Entity;

namespace M.Core.Dto
{
    public class MenuAuthDto
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuUrl { get; set; }
        public string MenuType { get; set; }

        public long? MenuParent { get; set; }
        public int? Sort { get; set; }
        public byte? Status { get; set; }
        public string Remark { get; set; }

        public List<Sys_auth> Children { get; set; }
    }
}