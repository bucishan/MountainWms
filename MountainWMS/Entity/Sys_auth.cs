using SqlSugar;
using System;

namespace M.Core.Entity
{
    public partial class Sys_auth
    {
        public Sys_auth()
        {
            IsDel = Convert.ToByte("1");
            CreateDate = DateTime.Now;
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long AuthId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// Desc:函数名称 用于元素事件绑定
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AuthType { get; set; }

        /// <summary>
        /// Desc:图标
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AuthIcon { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// Desc:1锁定   0不锁定
        /// Default:1
        /// Nullable:False
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// Desc:1未删除   0删除
        /// Default:1
        /// Nullable:False
        /// </summary>
        public byte IsDel { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>
        public long? CreateBy { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Desc:修改人
        /// Default:
        /// Nullable:True
        /// </summary>
        public long? ModifiedBy { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:True
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
