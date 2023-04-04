using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{

    // Список точек, которые нужно посетить для достижения цели
    public List<Vector2> pathToTarget;

    // Список уже проверенных нодов
    public List<Node> checkedNodes = new List<Node>();

    // Список свободных нодов
    public List<Node> freeNodes = new List<Node>();

    // Список нодов, которые нужно проверить
    List<Node> waitingNodes = new List<Node>();

    // Цель, к которой нужно построить путь
    public GameObject target;

    // Маска слоя, который является препятствием для движения
    public LayerMask solidLayer;

    // Метод для получения пути до цели
    public List<Vector2> GetPath(Vector2 target)
    {

        pathToTarget = new List<Vector2>();
        checkedNodes = new List<Node>();
        waitingNodes = new List<Node>();


        // Округляем координаты текущего положения и цели до целых чисел
        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        // Если текущая позиция совпадает с целью, то возвращаем пустой путь
        if (StartPosition == TargetPosition) return pathToTarget;

        // Создаем стартовую ноду и добавляем ее в список проверенных нодов
        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        checkedNodes.Add(startNode);

        // Получаем всех соседей стартовой ноды и добавляем их в список нодов, которые нужно проверить
        waitingNodes.AddRange(GetNeighbourNodes(startNode));

        while (waitingNodes.Count > 0)
        {
            // Получаем нод с наименьшим значением F из списка нодов, которые нужно проверить
            Node nodeToCheck = waitingNodes.Where(x => x.F == waitingNodes.Min(y => y.F)).FirstOrDefault();

            // Если найдена целевая нода, то вычисляем путь до нее и возвращаем его
            if (nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            // Проверяем, можно ли пройти в текущую ноду. Если нельзя, то удаляем ее из списка нодов, которые нужно проверить,
            // и добавляем в список проверенных нодов
            Vector2Int nodePosition = new((int)nodeToCheck.Position.x, (int)nodeToCheck.Position.y);
            var walkable = !Physics2D.OverlapBox(nodePosition, new Vector2(1f, 1f), 0f, solidLayer);
            if (!walkable)
            {
                waitingNodes.Remove(nodeToCheck);
                checkedNodes.Add(nodeToCheck);
            }

            // Если можно пройти в ноду, то удаляем ее из списка нодов, которые нужно проверить,
            // и добавляем в список проверенных нодов, если она еще не была проверена

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
    public int G; // расстояние от старта до ноды
    public int H; // расстояние от ноды до цели

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