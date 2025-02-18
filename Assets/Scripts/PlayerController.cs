///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.InputSystem;

namespace MysteryDungeon
{
	public class PlayerController : MonoBehaviour
	{
		private enum InputMode
		{
			Move,
			Look,
			Diagonal
		}

		private Entity m_entity;

        private Vector2 m_moveInput;

        private void Start()
        {
			m_entity = gameObject.AddComponent<Entity>();
        }

        private void Update()
        {
			if (m_entity.IsMoving || m_entity.IsTurning)
				return;
			
			if (m_moveInput == Vector2.zero)
				return;

			if (m_moveInput != m_entity.facingDirection)
				m_entity.FaceDirection(Vector2Int.RoundToInt(m_moveInput));
			else
				m_entity.ProcessMovement(Vector2Int.RoundToInt(m_moveInput));
        }

		public void GetMoveInput(InputAction.CallbackContext context)
		{
			m_moveInput = context.ReadValue<Vector2>();			
		}
    }
}
