using System;
using System.Collections.Generic;
using System.Linq;

namespace Maybe2.Classes
{
    public class ForestLink<T>
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public T Data { get; set; }

        public ForestLink() { }
        public ForestLink(T data, string id, string parentId)
        {
            Data = data;
            Id = id;
            ParentId = parentId;
        }
    }

    public class ForestNode<T> : ForestLink<T>
    {
        public ForestNode<T> Parent;
        public List<ForestNode<T>> Children { get; set; }

        public int Level
        {
            get { return this.GetDept(); }
        }

        public ForestNode()
        {
            Children = new List<ForestNode<T>>();
        }

        public ForestNode(T data, string id, string parentId)
            : base(data, id, parentId)
        {
            Children = new List<ForestNode<T>>();
        }
    }

    public class ForestData<T>
    {
        //Root Node
        public readonly List<ForestNode<T>> RootNodes;
        //All nodes by indexKeys
        public readonly Dictionary<string, ForestNode<T>> KeysNodes;

        internal ForestData(Dictionary<string, ForestNode<T>> keysNodes, List<ForestNode<T>> treeNodes)
        {
            KeysNodes = keysNodes;
            RootNodes = treeNodes;
        }
    }

    public static class TreeHelper
    {
        public const string ROOT_NODE = "#";

        public static ForestData<T> ConvertToForest<T>(this IEnumerable<ForestLink<T>> nodes)
        {
            //create
            //var keyNodes = nodes.ToDictionary(c => c.Id, c => new ForestNode<T>
            var keyNodes = new Dictionary<string, ForestNode<T>>();


            nodes.ForEach(c => keyNodes[c.Id ?? ROOT_NODE] = new ForestNode<T>
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Data = c.Data
            });


            //build
            var grChilds = keyNodes.Values
                .Where(p => p.ParentId != null && p.Id != p.ParentId)
                .GroupBy(c => c.ParentId);

            grChilds.AsParallel().ForAll(c =>
            {
                ForestNode<T> pnode;
                if (keyNodes.TryGetValue(c.Key, out pnode))
                {
                    pnode.Children.AddRange(c);
                    pnode.Children.ForEach(ch => ch.Parent = pnode);
                }
            });

            //root nodes
            var treeNodes = keyNodes.Values.Where(c => c.Parent == null).ToList();

            return new ForestData<T>(keyNodes, treeNodes);
        }

        /// <summary>
        /// Converts a heirachacle Array of Tree Nodes into a flat array of nodes. The order
        /// of the returned nodes is the same as a depth-first traversal of each tree.
        /// </summary>
        /// <remarks>The relationships between Parent/Children are retained.</remarks>
        public static List<ForestNode<T>> ConvertToFlatArray<T>(this IEnumerable<ForestNode<T>> trees, Func<IEnumerable<ForestNode<T>>, IEnumerable<ForestNode<T>>> childSelector = null)
        {
            return DepthFirstTraversalOfList(trees, childSelector)
                .ToList();
        }


        public static int GetDept<T>(this ForestNode<T> node)
        {
            ForestNode<T> p = node; int dept = 0;
            while ((p = p.Parent) != null) dept++;
            return dept;
        }

        public static ForestNode<T> GetRootNode<T>(this ForestNode<T> node)
        {
            var cur = node;
            while (cur.Parent != null)
                cur = cur.Parent;
            return cur;
        }

        public static IEnumerable<ForestNode<T>> ClimbToRoot<T>(this ForestNode<T> startNode)
        {
            var current = startNode;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public static bool HasParentIds<T>(this ForestNode<T> node, IEnumerable<string> parentIds)
        {
            var hash = parentIds.Select(c => c ?? ROOT_NODE).ToDictionary(c => c);
            return ClimbToRoot(node).Any(c => hash.ContainsKey(c.Id));
        }

        public static bool HasParentId<T>(this ForestNode<T> node, string parentid)
        {
            return GetParentId(node, parentid) != null;
        }

        public static ForestNode<T> GetParentId<T>(this ForestNode<T> node, string parentid)
        {
            var id = parentid ?? ROOT_NODE;
            return ClimbToRoot(node).FirstOrDefault(c => c.Id == id);
        }

        public static IEnumerable<ForestNode<T>> FindAll<T>(this ForestData<T> data, IEnumerable<string> ids)
        {
            return ids
                .Distinct()
                .Select(c => data.KeysNodes.GetOrDefault(c ?? ROOT_NODE))
                .Where(c => c != null);
        }

        public static IEnumerable<ForestNode<T>> ClimbToRootAll<T>(this ForestData<T> data, IEnumerable<string> startIds)
        {
            return FindAll(data, startIds)
                .SelectMany(n => n.ClimbToRoot())
                .Distinct();
        }

        public static ForestData<T> Trim<T>(this ForestData<T> data, IEnumerable<string> ids)
        {
            //build new tree
            return data
                .ClimbToRootAll<T>(ids)
                .ConvertToForest<T>();
        }

        private static IEnumerable<T> doSelect<T>(IEnumerable<T> items, Func<IEnumerable<T>, IEnumerable<T>> selector)
        {
            if (selector == null)
                return items;
            return selector(items);
        }

        /// <summary>
        /// Returns an Iterator which starts at the given node, and traverses the tree in
        /// a depth-first search manner.
        /// </summary>
        /// <param name="startNode">The node to start iterating from.  This will be the first node returned by the iterator.</param>
        public static IEnumerable<ForestNode<T>> DepthFirstTraversal<T>(this ForestNode<T> startNode, Func<IEnumerable<ForestNode<T>>, IEnumerable<ForestNode<T>>> childSelector = null)
        {
            yield return startNode;
            foreach (var child in doSelect(startNode.Children, childSelector))
            {
                foreach (var grandChild in DepthFirstTraversal(child, childSelector))
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        /// Returns an Iterator which traverses a forest of trees in a depth-first manner.
        /// </summary>
        /// <param name="trees">The forest of trees to traverse.</param>
        public static IEnumerable<ForestNode<T>> DepthFirstTraversalOfList<T>(this IEnumerable<ForestNode<T>> trees, Func<IEnumerable<ForestNode<T>>, IEnumerable<ForestNode<T>>> childSelector = null)
        {
            foreach (var rootNode in doSelect(trees, childSelector))
            {
                foreach (var node in DepthFirstTraversal(rootNode, childSelector))
                {
                    yield return node;
                }
            }
        }

        /// <summary>
        /// Gets the siblings of the given node. Note that the given node is included in the
        /// returned list.  Throws an <see cref="Exception" /> if this is a root node.
        /// </summary>
        /// <param name="node">The node whose siblings are to be returned.</param>
        /// <param name="includeGivenNode">If false, then the supplied node will not be returned in the sibling list.</param>
        public static IEnumerable<ForestNode<T>> Siblings<T>(this ForestNode<T> node, bool includeGivenNode)
        {
            if (node.Parent == null)
            {
                if (includeGivenNode)
                {
                    yield return node;
                }
                yield break;
            }

            foreach (var sibling in node.Parent.Children)
            {
                if (!includeGivenNode && sibling.Id == node.Id)
                { // current node is supplied node; don't return it unless it was asked for.
                    continue;
                }
                yield return sibling;
            }
        }


        /// <summary>
        /// Traverses the tree in a breadth-first fashion.
        /// </summary>
        /// <param name="node">The node to start at.</param>
        /// <param name="returnRootNode">If true, the given node will be returned; if false, traversal starts at the node's children.</param>
        public static IEnumerable<ForestNode<T>> BreadthFirstTraversal<T>(this ForestNode<T> node, bool returnRootNode)
        {
            if (returnRootNode)
            {
                yield return node;
            }

            foreach (var child in node.Children)
            {
                yield return child;
            }


            foreach (var child in node.Children)
            {
                foreach (var grandChild in BreadthFirstTraversal(child, false))
                {
                    yield return grandChild;
                }
            }
        }

    }
}
