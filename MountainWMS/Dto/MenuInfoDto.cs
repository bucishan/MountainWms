using System.Collections.Generic;

namespace M.Core.Dto
{
    public class MenuInfoDto
    {
        public string MenuId { get; set; }

        public string MenuName { get; set; }
        public string ParentMenuId { get; set; }

        public string ParentMenuName { get; set; }

    }
}