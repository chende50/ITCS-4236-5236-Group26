using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    private Player player;
    private const string PLAYER = "Player";

    private void Start()
    {
        player = GameObject.FindWithTag(PLAYER).GetComponent<Player>();
        textBox.text = player.GetTime().ToString();
    }

    private void Update()
    {
        textBox.text = player.GetTime().ToString();
    }
}
