using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KDTree<T>
{
    public KDTreeNode<T> Root { get; private set; }

    public KDTree(Vector3[] points)
    {
        Root = BuildTree(points.ToList(), 0);
    }

    private KDTreeNode<T> BuildTree(List<Vector3> points, int depth)
    {
        if (points.Count == 0) return null;

        int axis = depth % 3;
        points.Sort((a, b) => a[axis].CompareTo(b[axis]));

        int medianIndex = points.Count / 2;
        Vector3 medianPoint = points[medianIndex];

        return new KDTreeNode<T>
        {
            Point = medianPoint,
            Left = BuildTree(points.GetRange(0, medianIndex), depth + 1),
            Right = BuildTree(points.GetRange(medianIndex + 1, points.Count - medianIndex - 1), depth + 1)
        };
    }

    public Vector3 FindNearestNeighbor(Vector3 queryPoint)
    {
        return FindNearestNeighbor(Root, queryPoint, 0).Point;
    }

    private KDTreeNode<T> FindNearestNeighbor(KDTreeNode<T> node, Vector3 queryPoint, int depth)
    {
        if (node == null) return null;

        int axis = depth % 3;
        KDTreeNode<T> nextBranch = queryPoint[axis] < node.Point[axis] ? node.Left : node.Right;
        KDTreeNode<T> otherBranch = queryPoint[axis] < node.Point[axis] ? node.Right : node.Left;

        KDTreeNode<T> best = FindNearestNeighbor(nextBranch, queryPoint, depth + 1);
        if (best == null || Vector3.Distance(queryPoint, node.Point) < Vector3.Distance(queryPoint, best.Point))
        {
            best = node;
        }

        if (Mathf.Abs(queryPoint[axis] - node.Point[axis]) < Vector3.Distance(queryPoint, best.Point))
        {
            KDTreeNode<T> possibleBest = FindNearestNeighbor(otherBranch, queryPoint, depth + 1);
            if (possibleBest != null && Vector3.Distance(queryPoint, possibleBest.Point) < Vector3.Distance(queryPoint, best.Point))
            {
                best = possibleBest;
            }
        }

        return best;
    }
}

public class KDTreeNode<T>
{
    public Vector3 Point { get; set; }
    public KDTreeNode<T> Left { get; set; }
    public KDTreeNode<T> Right { get; set; }
}

