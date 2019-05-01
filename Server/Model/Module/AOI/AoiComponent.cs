using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ETModel.AOI
{
    public class AoiComponent : Component
    {
        private readonly Dictionary<long, AoiNode> _nodes = new Dictionary<long, AoiNode>();

        private readonly AoiNodeLinkedList _xLinks = new AoiNodeLinkedList(10, AoiNodeLinkedListType.XLink);
        
        private readonly AoiNodeLinkedList _yLinks = new AoiNodeLinkedList(10, AoiNodeLinkedListType.YLink);

        public void Awake(){}

        /// <summary>
        /// 新加入AOI
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <param name="x">X轴位置</param>
        /// <param name="y">Y轴位置</param>
        /// <returns></returns>
        public AoiNode Enter(long id, float x, float y)
        {
            AoiNode node;
            if (_nodes.TryGetValue(id, out node)) return node;

            node = AoiPool.Instance.Fetch<AoiNode>().Init(id, x, y);

            _xLinks.Insert(node);

            _yLinks.Insert(node);

            _nodes[node.Id] = node;

            return node;
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <param name="area">区域距离</param>
        /// <param name="x">X轴位置</param>
        /// <param name="y">Y轴位置</param>
        /// <returns></returns>
        public AoiNode Update(long id, Vector2 area, float x, float y)
        {
            AoiNode node;
            return !_nodes.TryGetValue(id, out node) ? null : Update(node, area, x, y);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <param name="area">区域距离</param>       
        /// <returns></returns>
        public AoiNode Update(AoiNode node, Vector2 area)
        {
            return Update(node, area, node.Position.x, node.Position.y);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="node">Aoi节点</param>
        /// <param name="area">区域距离</param>
        /// <param name="x">X轴位置</param>
        /// <param name="y">Y轴位置</param>
        /// <returns></returns>
        public AoiNode Update(AoiNode node, Vector2 area, float x, float y)
        {
            // 把新的AOI节点转移到旧的节点里

            node.AoiInfo.MoveOnlySet = new HashSet<long>(node.AoiInfo.MovesSet.Select(d => d));

            // 移动到新的位置

            Move(node, x, y);

            // 查找周围坐标

            Find(node, area);

            // 差集计算

            node.AoiInfo.EntersSet = new HashSet<long>(node.AoiInfo.MovesSet.Except(node.AoiInfo.MoveOnlySet));
            
            // 把自己添加到进入点的人

            foreach (var enterNode in node.AoiInfo.EntersSet) GetNode(enterNode).AoiInfo.MovesSet.Add(node.Id);

            node.AoiInfo.LeavesSet = new HashSet<long>(node.AoiInfo.MoveOnlySet.Except(node.AoiInfo.MovesSet));

            node.AoiInfo.MoveOnlySet = new HashSet<long>(node.AoiInfo.MoveOnlySet.Except(node.AoiInfo.EntersSet)
                .Except(node.AoiInfo.LeavesSet));

            return node;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="node">Aoi节点</param>
        /// <param name="x">X轴位置</param>
        /// <param name="y">Y轴位置</param>
        private void Move(AoiNode node, float x, float y)
        {
            #region 移动X轴

            if (Math.Abs(node.Position.x - x) > 0)
            {
                if (x > node.Position.x)
                {
                    var cur = node.Link.XNode.Next;

                    while (cur != null)
                    {
                        if (x < cur.Value.Position.x)
                        {
                            _xLinks.Remove(node.Link.XNode);

                            node.Position.x = x;
                            
                            node.Link.XNode = _xLinks.AddBefore(cur, node);

                            break;
                        }
                        else if (cur.Next == null)
                        {
                            _xLinks.Remove(node.Link.XNode);
                            
                            node.Position.x = x;
                            
                            node.Link.XNode = _xLinks.AddAfter(cur, node);

                            break;
                        }

                        cur = cur.Next;
                    }
                }
                else
                {
                    var cur = node.Link.XNode.Previous;

                    while (cur != null)
                    {
                        if (x > cur.Value.Position.x)
                        {
                            _xLinks.Remove(node.Link.XNode);
                            
                            node.Position.x = x;
                            
                            node.Link.XNode = _xLinks.AddAfter(cur, node);

                            break;
                        }
                        else if (cur.Previous == null)
                        {
                            _xLinks.Remove(node.Link.XNode);
                            
                            node.Position.x = x;
                            
                            node.Link.XNode = _xLinks.AddAfter(cur, node);

                            break;
                        }

                        cur = cur.Previous;
                    }
                }
            }

            #endregion

            #region 移动Y轴

            if (Math.Abs(node.Position.y - y) > 0)
            {
                if (y > node.Position.y)
                {
                    var cur = node.Link.YNode.Next;

                    while (cur != null)
                    {
                        if (y < cur.Value.Position.y)
                        {
                            _yLinks.Remove(node.Link.YNode);
                            
                            node.Position.y = y;
                            
                            node.Link.YNode = _yLinks.AddBefore(cur, node);

                            break;
                        }
                        else if (cur.Next == null)
                        {
                            _yLinks.Remove(node.Link.YNode);
                            
                            node.Position.y = y;
                            
                            node.Link.YNode = _yLinks.AddAfter(cur, node);

                            break;
                        }

                        cur = cur.Next;
                    }
                }
                else
                {
                    var cur = node.Link.YNode.Previous;

                    while (cur != null)
                    {
                        if (y > cur.Value.Position.y)
                        {
                            _yLinks.Remove(node.Link.YNode);
                            
                            node.Position.y = y;
                            
                            node.Link.YNode = _yLinks.AddBefore(cur, node);

                            break;
                        }
                        else if (cur.Previous == null)
                        {
                            _yLinks.Remove(node.Link.YNode);
                            
                            node.Position.y = y;
                            
                            node.Link.YNode = _yLinks.AddAfter(cur, node);

                            break;
                        }

                        cur = cur.Previous;
                    }
                }
            }

            
            #endregion

            node.SetPosition(x, y);
        }

        /// <summary>
        /// 根据指定范围查找周围的坐标
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <param name="area">区域距离</param>
        public AoiNode Find(long id, Vector2 area)
        {
            AoiNode node;
            return !_nodes.TryGetValue(id, out node) ? null : Find(node, area);
        }

        /// <summary>
        /// 根据指定范围查找周围的坐标
        /// </summary>
        /// <param name="node">Aoi节点</param>
        /// <param name="area">区域距离</param>
        public AoiNode Find(AoiNode node, Vector2 area)
        {
            node.AoiInfo.MovesSet.Clear();
            
            for (var i = 0; i < 2; i++)
            {
                var cur = i == 0 ? node.Link.XNode.Next : node.Link.XNode.Previous;

                while (cur != null)
                {
                    if (Math.Abs(Math.Abs(cur.Value.Position.x) - Math.Abs(node.Position.x)) > area.x)
                    {
                        break;
                    }
                    else if (Math.Abs(Math.Abs(cur.Value.Position.y) - Math.Abs(node.Position.y)) <= area.y)
                    {
                        if (Vector2.Distance(node.Position, cur.Value.Position) <= area.x)
                        {
                            if (!node.AoiInfo.MovesSet.Contains(cur.Value.Id)) node.AoiInfo.MovesSet.Add(cur.Value.Id);
                        }
                    }

                    cur = i == 0 ? cur.Next : cur.Previous;
                }
            }

            for (var i = 0; i < 2; i++)
            {
               var cur = i == 0 ? node.Link.YNode.Next : node.Link.YNode.Previous;

                while (cur != null)
                {
                    if (Math.Abs(Math.Abs(cur.Value.Position.y) - Math.Abs(node.Position.y)) > area.y)
                    {
                        break;
                    }
                    else if (Math.Abs(Math.Abs(cur.Value.Position.x) - Math.Abs(node.Position.x)) <= area.x)
                    {
                        if (Vector2.Distance(node.Position, cur.Value.Position) <= area.x)
                        {
                            if (!node.AoiInfo.MovesSet.Contains(cur.Value.Id)) node.AoiInfo.MovesSet.Add(cur.Value.Id);
                        }
                    }

                    cur = i == 0 ? cur.Next :cur.Previous;
                }
            }

            return node;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <returns></returns>
        public AoiNode GetNode(long id)
        {
            AoiNode node;
            return _nodes.TryGetValue(id, out node) ? node : null;
        }

        /// <summary>
        /// 退出AOI
        /// </summary>
        /// <param name="id">一般是角色的ID等其他标识ID</param>
        /// <returns>需要通知的坐标列表</returns>
        public long[] LeaveNode(long id)
        {
            AoiNode node;
            if (!_nodes.TryGetValue(id, out node)) return null;

            _xLinks.Remove(node.Link.XNode);

            _yLinks.Remove(node.Link.YNode);

            _nodes.Remove(id);

            var aoiNodes = node.AoiInfo.MovesSet.ToArray();
            
            node.Dispose();

            return aoiNodes;
        }
    }
}