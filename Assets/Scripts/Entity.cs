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
        private Vector2Int m_gridPosition;
        public Vector2Int GridPosition => m_gridPosition;
        private Vector2Int m_facingDirection;
        public Vector2Int FacingDirection => m_facingDirection;

        private bool m_isBusy = false;
        public bool IsBusy => m_isBusy;

        // tile
        private Tile m_currentTile;
        public Tile CurrentTile => m_currentTile;

        // objects
        [SerializeField] private Animator m_anim;
        [SerializeField] private DamageDisplay m_damageDisplay;

        public void Init(Vector2Int gridPosition)
        {
            m_gridPosition = gridPosition;
            m_health = m_maxHealth;
            m_currentTile = GridManager.Instance.GetTileAt(m_gridPosition);
            transform.position = m_currentTile.transform.position;
            m_currentTile.SetContent(new TileContent(TileType.Entity, gameObject));
            m_currentTile.SetHighlighted(true);
        }

        public void PlayerSetup()
        {
            foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                renderer.color = Color.yellow;
            name = "Player";
        }

        #region Movement
        public void ProcessMovement(Vector2Int direction)
        {
            // check that the move is valid
            Vector2Int newPosition = m_gridPosition + m_facingDirection;
            if (newPosition.x < 0 || newPosition.x >= GridManager.k_Width || newPosition.y < 0 || newPosition.y >= GridManager.k_Height)
            {
                Debug.LogWarning("Target position is invalid!");
                return;
            }

            // de-highlight the current tile
            m_currentTile.SetHighlighted(false);
            //m_currentTile.SetContent(new TileContent())

            // update the player's position
            m_gridPosition += m_facingDirection;

            // set the current tile to the new one
            m_currentTile = GridManager.Instance.GetTileAt(m_gridPosition);
            m_currentTile.SetHighlighted(true);
            StartCoroutine(MoveTowardsNewPosition(m_currentTile.transform.position));
        }

        private IEnumerator MoveTowardsNewPosition(Vector3 targetPosition)
        {
            m_isBusy = true;
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, EntityMetrics.MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            m_isBusy = false;
        }

        public void FaceDirection(Vector2Int direction)
        {
            m_facingDirection = direction;

            Vector3 worldDirection = new Vector3(direction.x, 0, direction.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            StopCoroutine("FaceTowardsNewDirection");
            StartCoroutine(FaceTowardsNewDirection(targetRotation));
        }

        private IEnumerator FaceTowardsNewDirection(Quaternion targetRotation)
        {
            m_isBusy = true;
            while (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, EntityMetrics.TURN_SPEED * Time.deltaTime);
                yield return null;
            }
            m_isBusy = false;
        }
        #endregion

        #region Combat
        public void Attack(Entity target)
        {
            StartCoroutine(AttackTarget(target));
        }

        private IEnumerator AttackTarget(Entity target)
        {
            m_isBusy = true;
            m_anim.Play("Phys Attack");
            yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);

            // check if there is a valid target
            if (target != null)
            {
                target.TakeDamage(m_baseDamage);
            }
            else
                Debug.Log("No target!");

            m_isBusy = false;
        }

        public void UseSkill(Skill skill)
        {
            Entity target = skill.GetTarget(this);
            StartCoroutine(UseSkillOnTarget(target, skill));
        }

        private IEnumerator UseSkillOnTarget(Entity target, Skill skill)
        {
            m_isBusy = true;
            m_anim.Play("Skill Attack");
            yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);

            // check if there is a valid target
            if (target != null)
            {
                target.TakeDamage(m_baseDamage);
            }
            else
                Debug.Log("No target!");

            m_isBusy = false;
        }

        /// <summary>
        /// Reduce the Entity's health by an amount.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The amount of damage the Entity took.</returns>
        public void TakeDamage(int value)
        {
            int oldHealth = m_health;
            m_health -= value;
            if (m_health < 0)
                m_health = 0;

            int damageTaken = oldHealth - m_health;
            m_damageDisplay.Init(damageTaken, SkillType.Regular);
        }

        /// <summary>
        /// Restore the Entity's health by an amount.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The amount of damage the Entity restored.</returns>
        public void HealDamage(int value)
        {
            int oldHealth = m_health;
            m_health += value;
            if (m_health > m_maxHealth)
                m_health = m_maxHealth;

            int amountHealed = m_health - oldHealth;
            m_damageDisplay.Init(amountHealed, SkillType.Heal);
        }
        #endregion
    }
}
