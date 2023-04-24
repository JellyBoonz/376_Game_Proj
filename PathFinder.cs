using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
	Node start;

    /**
        Instantiated grid object
    */
	void Awake() {
		grid = GetComponent<Grid> ();
	}

	void Update() {
		FindPath (seeker.position, target.position);
	}

    public Grid getGrid(){
        return grid;
    }

    /**
        A* algorithm. Adapted from pseudocode found on brilliant.ord/wiki/a-star-search/

    */
	public void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.getNodeFromPoint(startPos);
		Node targetNode = grid.getNodeFromPoint(targetPos);

        // Set of nodes to be evaluated
		List<Node> open = new List<Node>();

        // Set of nodes already evaluated
		HashSet<Node> closed = new HashSet<Node>();
		open.Add(startNode);

		while (open.Count > 0) {
			Node node = open[0];

			// Looking for node with lowest f score in open list
			for (int i = 1; i < open.Count; i ++) {
				if (open[i].fCost < node.fCost || open[i].fCost == node.fCost) {
					if (open[i].hCost < node.hCost)
						node = open[i];
				}
			}

			// Moving current node to closed list
			open.Remove(node);
			closed.Add(node);

            // We have reached our destination node
			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node)) {
				if (!neighbour.walkable || closed.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);

				// if gCost of neighbor is lower than current node or it is not in the open set
				if (newCostToNeighbour < neighbour.gCost || !open.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					// if nieghbor is not in the open set
					if (!open.Contains(neighbour))
						open.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	// void setStartNode(Node startNode){
	// 	start = startNode;
	// }

	// public Node getStartNode(){
	// 	Debug.Log(start.worldPosition.normalized);
	// 	return start;
	// }

    /**
        Function for finding the amount of horizontal, vertical, and 
        diagonal moves from nodeA to nodeB
    */
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return (14 * dstY) + 10 * (dstX - dstY);
		return (14 * dstX) + 10 * (dstY - dstX);
	}
}
