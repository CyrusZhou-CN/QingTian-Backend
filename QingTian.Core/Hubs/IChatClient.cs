using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 聊天集线器
    /// </summary>
    public interface IChatClient
    {

        Task ForceExist(string str);

        Task AppendNotice(SysNotice notice);
    }
}
