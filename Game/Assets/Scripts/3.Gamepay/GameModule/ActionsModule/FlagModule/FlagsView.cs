using TMPro;
using UnityEngine;

public class FlagsView : MonoBehaviour
{
    [SerializeField] private TMP_Text m_counter;

    public void Init(int _initFlagsLeft)
    {
        UpdateFlagsCounter(_initFlagsLeft);
    }

    public void UpdateFlagsCounter(int _flagsLeft)
    {
        if (_flagsLeft < 0)
            return;

        if (_flagsLeft < 10)
            m_counter.text = $"00{_flagsLeft}";
        else if (_flagsLeft < 100)
            m_counter.text = $"0{_flagsLeft}";
        else
            m_counter.text = $"{_flagsLeft}";
    }
}
