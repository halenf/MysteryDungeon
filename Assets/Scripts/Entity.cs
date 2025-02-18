///<summary>
/// Author: Halen
///
///
///
///</summary>

using System.Collections;
using UnityEngine;

namespace MysteryDungeon
{
	public class Entity : MonoBehaviour
	{
		private int m_health;
		private int m_maxHealth = 100;

        private int m_baseDamage = 10;

        // position/direction
        public Vector2Int gridPosition;
        public Vector2Int facingDirection;

        // tile
        private Tile m_currentTile;
        public Tile CurrentTile => m_currentTile;

        // movement
        private bool m_isMoving = false;
        public bool IsMoving => m_isMoving;
        private bool m_isTurning = false;
        public bool IsTurning => m_isTurning;

        private void Start()
        {
            m_health = m_maxHealth;
            gridPosition = Vector2Int.zero;
        }

        #region Movement
        public void ProcessMovement(Vector2Int direction)
        {
            // de-highlight the current tile
            m_currentTile?.SetHighlighted(false);

            // update the player's position
            gridPosition += facingDirection;

            // set the current tile to the new one
            m_currentTile = GridManager.Instance.GetTile(gridPosition);
            m_currentTile.SetHighlighted(true);
            StartCoroutine(MoveTowardsNewPosition(m_currentTile.transform.position));
        }

        private IEnumerator MoveTowardsNewPosition(Vector3 targetPosition)
        {
            m_isMoving = true;
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, EntityMetrics.MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            m_isMoving = false;
        }

        public void FaceDirection(Vector2Int direction)
        {
            facingDirection = direction;

            Vector3 worldDirection = new Vector3(direction.x, 0, direction.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            StopCoroutine("FaceTowardsNewDirection");
            StartCoroutine(FaceTowardsNewDirection(targetRotation));
        }

        private IEnumerator FaceTowardsNewDirection(Quaternion targetRotation)
        {
            m_isTurning = true;
            while (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, EntityMetrics.TURN_SPEED * Time.deltaTime);
                yield return null;
            }
            m_isTurning = false;
        }
        #endregion

        public void TakeDamage(int damage)
        {
            m_health -= damage;
        }
    }
}
