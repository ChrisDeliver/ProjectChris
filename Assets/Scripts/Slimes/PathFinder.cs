using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{

    // ������ �����, ������� ����� �������� ��� ���������� ����
    public List<Vector2> pathToTarget;

    // ������ ��� ����������� �����
    public List<Node> checkedNodes = new List<Node>();

    // ������ ��������� �����
    public List<Node> freeNodes = new List<Node>();

    // ������ �����, ������� ����� ���������
    List<Node> waitingNodes = new List<Node>();

    // ����, � ������� ����� ��������� ����
    public GameObject target;

    // ����� ����, ������� �������� ������������ ��� ��������
    public LayerMask solidLayer;

    // ����� ��� ��������� ���� �� ����
    public List<Vector2> GetPath(Vector2 target)
    {

        pathToTarget = new List<Vector2>();
        checkedNodes = new List<Node>();
        waitingNodes = new List<Node>();


        // ��������� ���������� �������� ��������� � ���� �� ����� �����
        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        // ���� ������� ������� ��������� � �����, �� ���������� ������ ����
        if (StartPosition == TargetPosition) return pathToTarget;

        // ������� ��������� ���� � ��������� �� � ������ ����������� �����
        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        checkedNodes.Add(startNode);

        // �������� ���� ������� ��������� ���� � ��������� �� � ������ �����, ������� ����� ���������
        waitingNodes.AddRange(GetNeighbourNodes(startNode));

        while (waitingNodes.Count > 0)
        {
            // �������� ��� � ���������� ��������� F �� ������ �����, ������� ����� ���������
            Node nodeToCheck = waitingNodes.Where(x => x.F == waitingNodes.Min(y => y.F)).FirstOrDefault();

            // ���� ������� ������� ����, �� ��������� ���� �� ��� � ���������� ���
            if (nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            // ���������, ����� �� ������ � ������� ����. ���� ������, �� ������� �� �� ������ �����, ������� ����� ���������,
            // � ��������� � ������ ����������� �����
            Vector2Int nodePosition = new((int)nodeToCheck.Position.x, (int)nodeToCheck.Position.y);
            var walkable = !Physics2D.OverlapBox(nodePosition, new Vector2(1f, 1f), 0f, solidLayer);
            if (!walkable)
            {
                waitingNodes.Remove(nodeToCheck);
                checkedNodes.Add(nodeToCheck);
            }

            // ���� ����� ������ � ����, �� ������� �� �� ������ �����, ������� ����� ���������,
            // � ��������� � ������ ����������� �����, ���� ��� ��� �� ���� ���������

            else if (walkable)
            {
                waitingNodes.Remove(nodeToCheck);
                if (!checkedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                {
                    checkedNodes.Add(nodeToCheck);
                    waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }
            }
        }
        freeNodes = checkedNodes;

        return pathToTarget;
    }

    public List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    List<Node> GetNeighbourNodes(Node node)
    {
        var Neighbours = new List<Node>
        {
            new Node(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y - 1), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y + 1), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y - 1), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y - 1), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y + 1), node.TargetPosition, node),
            new Node(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y + 1), node.TargetPosition, node)
        };
        return Neighbours;
    }
}

public class Node
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public Node PreviousNode;
    public int F; // F=G+H
    public int G; // ���������� �� ������ �� ����
    public int H; // ���������� �� ���� �� ����

    public Node(int g, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}