using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textBox;
	[SerializeField] private EnemySpawnManager enemySpawnManager;
	private Player player;
	private const string PLAYER = "Player";
	private float startTime;

	private void Start()
	{
		player = GameObject.FindWithTag(PLAYER).GetComponent<Player>();
		startTime = player.GetTime();
		Debug.Log(startTime);
		textBox.text = "Survived for: 0s";
	}

	private void Update()
	{
		textBox.text = "Survived for: " + Mathf.RoundToInt(player.GetTime() - startTime).ToString() + "s\n";
		textBox.text += "Next wave in: " + Mathf.RoundToInt(enemySpawnManager.GetNextSpawnAt() - player.GetTime()).ToString() + "s\n";
	}
}
