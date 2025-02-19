///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;
using TMPro;

namespace MysteryDungeon
{
	public class DamageDisplay : MonoBehaviour
	{
        [SerializeField] TMP_Text m_displayText;

		[SerializeField, Min(0)] private float m_lifeTime;
		private float m_currentLifeTime = 0;

		private Quaternion m_initialRotation;

        private void Start()
        {
			m_displayText.gameObject.SetActive(false);
			m_initialRotation = m_displayText.rectTransform.rotation;
        }

        public void Init(int value, SkillType type)
		{
            m_displayText.gameObject.SetActive(true);
			m_displayText.rectTransform.localPosition = Vector3.zero;

            switch (type)
			{
				case SkillType.Regular:
					m_displayText.color = Color.red;
					m_displayText.text = "-";
					break;
				case SkillType.Magic:
					m_displayText.color = Color.blue;
					m_displayText.text = "-";
					break;
				case SkillType.Heal:
					m_displayText.color = Color.green;
					m_displayText.text = "+";
					break;
			}

			m_displayText.text += value.ToString();
			m_currentLifeTime = 0;
		}

        private void Update()
        {
			if (m_currentLifeTime < m_lifeTime)
			{
				m_displayText.rectTransform.localPosition += Vector3.up * Time.deltaTime;
				m_displayText.rectTransform.rotation = Camera.main.transform.rotation * m_initialRotation;

				m_currentLifeTime += Time.deltaTime;
				if (m_currentLifeTime >= m_lifeTime)
					m_displayText.gameObject.SetActive(false);
			}
        }
    }
}
