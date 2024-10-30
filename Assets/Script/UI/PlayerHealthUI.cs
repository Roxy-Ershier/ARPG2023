using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private Image _currentPlayerHealth;
    [SerializeField] private Image _currentPlayerHealthBack;


    [SerializeField] private Image _currentPlayerStrength;
    [SerializeField] private Image _currentPlayerStrengthBack;


    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<CharacterHealthInfo>("UpdatePlayerHealthAndStrenghtUI", UpdatePlayerHealthAndStrenght);
    }
    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<CharacterHealthInfo>("UpdatePlayerHealthAndStrenghtUI", UpdatePlayerHealthAndStrenght);
    }

    private void UpdatePlayerHealthAndStrenght(CharacterHealthInfo healthInfo)
    {
        _currentPlayerHealth.fillAmount = healthInfo.Cur_HP / healthInfo.HealthData.HP_MAX;
        _currentPlayerStrength.fillAmount = (healthInfo.Cur_Strength / healthInfo.HealthData.Strength_MAX);
        StopCoroutine(UpdatePlayerHealthAndStrenghtBack());
        StartCoroutine(UpdatePlayerHealthAndStrenghtBack());
    }

    IEnumerator UpdatePlayerHealthAndStrenghtBack()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        bool isFinish=false;
        while (!isFinish)
        {
            if(Mathf.Abs(_currentPlayerHealth.fillAmount- _currentPlayerHealthBack.fillAmount)<0.01f&&
               Mathf.Abs(_currentPlayerStrength.fillAmount - _currentPlayerStrengthBack.fillAmount) < 0.01f)
            {
                _currentPlayerHealthBack.fillAmount = _currentPlayerHealth.fillAmount;
                _currentPlayerStrengthBack.fillAmount= _currentPlayerStrength.fillAmount;
                isFinish = true;
            }
            _currentPlayerHealthBack.fillAmount = Mathf.Lerp(_currentPlayerHealthBack.fillAmount, _currentPlayerHealth.fillAmount, 2f * Time.deltaTime);
            _currentPlayerStrengthBack.fillAmount = Mathf.Lerp(_currentPlayerStrengthBack.fillAmount, _currentPlayerStrength.fillAmount, 2f * Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }

}
