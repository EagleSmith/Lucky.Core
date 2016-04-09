using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility
{
    /// <summary>
    /// 用与绑定 DropdownList 
    /// </summary>
    [Serializable]
    public class ListItemEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 显示的内容
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        public string ParentID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected { get; set; }
    }
}
