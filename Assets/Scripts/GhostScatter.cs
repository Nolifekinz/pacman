using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GhostScatter : GhostBehavior
{
	private void OnDisable()
	{
		ghost.chase.Enable();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Vector3 mapCornerPosition;
		Node node = other.GetComponent<Node>();
		switch (ghost.ghostType)
		{
			case GhostType.Blinky:
				mapCornerPosition = new Vector3(-12.5f, -15.5f, 0f);
				break;
			case GhostType.Pinky:
				mapCornerPosition = new Vector3(12.5f, 12.5f, 0f);
				break;
			case GhostType.Inky:
				mapCornerPosition = new Vector3(-12.5f, 12.5f, 0f);
				break;
			case GhostType.Clyde:
				mapCornerPosition = new Vector3(12.5f, -15.5f, 0f);
				break;
			default:
				mapCornerPosition = new Vector3(0f, 0f, 0f);
				break;
		}
		//ghost.target.transform.position = mapCornerPosition;
		// Do nothing while the ghost is frightened
		if (node != null && enabled && !ghost.frightened.enabled)
		{
			Vector2 direction = Vector2.zero;
			float minDistance = float.MaxValue;

			// Find the available direction that moves closest to the map corner
			foreach (Vector2 availableDirection in node.availableDirections)
			{
				// Calculate the new position if we move in this direction
				Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
				float distance = (mapCornerPosition - newPosition).sqrMagnitude;

				// If the distance in this direction is less than the current min distance, update direction
				if (distance < minDistance && availableDirection != -ghost.movement.direction)
				{
					direction = availableDirection;
					minDistance = distance;
				}
			}

			// Set direction to move towards the map corner
			ghost.movement.SetDirection(direction);
		}
	}
}
