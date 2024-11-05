using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 target;
		Vector3 pacmanPosition = ghost.pacman.transform.position;
		Vector3 pacmanDirection = ghost.pacman.movement.direction;
		Node node = other.GetComponent<Node>();
		switch (ghost.ghostType)
		{
			case GhostType.Blinky:
                target = ghost.pacman.transform.position;
				break;
			case GhostType.Pinky:
				// Calculate the position four tiles in front of Pac-Man
				target = pacmanPosition + (4 * new Vector3(pacmanDirection.x, pacmanDirection.y, 0f));
				break;
			case GhostType.Inky:
				// Calculate Inky's target position based on Pac-Man and Blinky's positions
				Vector3 blinkyPosition = GameManager.Instance.ghosts[(int)GhostType.Blinky].transform.position;

				// Calculate the position two tiles in front of Pac-Man
				Vector3 targetPosition = pacmanPosition + (2 * new Vector3(pacmanDirection.x, pacmanDirection.y, 0f));

				// Calculate the vector from Blinky's position to the target position
				Vector3 vectorFromBlinky = targetPosition - blinkyPosition;

				// Double the length of the vector
				Vector3 extendedVector = targetPosition + vectorFromBlinky;

				// Set Inky's target to the endpoint of the extended vector
				target = extendedVector;
				break;
			case GhostType.Clyde:
				float distanceToPacman;
				distanceToPacman = Vector3.Distance(transform.position, ghost.pacman.transform.position);

				// Determine target based on Clyde's distance to Pac-Man
				if (distanceToPacman > 8f)
				{
					// Use Blinky's targeting method
					target = ghost.pacman.transform.position;
				}
				else
				{
					target = new Vector3(12.5f, -15.5f, 0f);
				}
				break;
			default:
				target = ghost.pacman.transform.position;
				break;
		}
		ghost.target.transform.position = target;
		// Do nothing while the ghost is frightened
		if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Find the available direction that moves closet to pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is less than the current
                // min distance then this direction becomes the new closest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (target - newPosition).sqrMagnitude;

                if (distance < minDistance && availableDirection != -ghost.movement.direction)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

}
