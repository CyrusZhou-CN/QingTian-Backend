using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 树基类
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// 获取节点id
        /// </summary>
        /// <returns></returns>
        long GetId();

        /// <summary>
        /// 获取节点父id
        /// </summary>
        /// <returns></returns>
        long GetPid();

        /// <summary>
        /// 设置Children
        /// </summary>
        /// <param name="children"></param>
        void SetChildren(IList children);
    }
}
