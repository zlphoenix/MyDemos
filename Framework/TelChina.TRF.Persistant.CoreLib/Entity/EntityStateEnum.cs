using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.TRF.Persistant.CoreLib.Entity
{
    /// <summary>
    /// 实体状态,用于实体状态追踪
    /// </summary>
    public enum EntityStateEnum
    {
        Deleting = 8,
        Updating = 4,
        Inserting = 2,
        Unchanged = 1,
        Deleted = -1
    }
}
