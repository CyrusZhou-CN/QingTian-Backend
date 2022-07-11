using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 递归工具类，用于遍历有父子关系的节点，例如菜单树，字典树等等
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeBuildUtil<T> where T : ITreeNode
    {
        /// <summary>
        /// 顶级节点的父节点Id
        /// </summary>
        private readonly long _rootParentId = 0;

        /// <summary>
        /// 构造树节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<T> DoTreeBuild(List<T> nodes)
        {
            nodes.ForEach(u => BuildChildNodes(nodes, u, new List<T>()));

            var results = new List<T>();
            nodes.ForEach(u =>
            {
                if (_rootParentId == u.GetPid())
                    results.Add(u);
            });
            return results;
        }

        /// <summary>
        /// 构造子节点集合
        /// </summary>
        /// <param name="totalNodes"></param>
        /// <param name="node"></param>
        /// <param name="childNodeLists"></param>
        private void BuildChildNodes(List<T> totalNodes, T node, List<T> childNodeLists)
        {
            var nodeSubLists = new List<T>();
            totalNodes.ForEach(u =>
            {
                if (u.GetPid().Equals(node.GetId()))
                    nodeSubLists.Add(u);
            });
            nodeSubLists.ForEach(u => BuildChildNodes(totalNodes, u, new List<T>()));
            if (nodeSubLists.Count > 0)
            {
                childNodeLists.AddRange(nodeSubLists);
            }
            if (childNodeLists.Count > 0)
            {
                node.SetChildren(childNodeLists);
            }
        }
    }
}
