using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Image _currentBossHealth;
    [SerializeField] private Image _currentBossHealthBack;


    [SerializeField] private Image _currentBossStrength;
    [SerializeField] private Image _currentBossStrengthBack;


    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<CharacterHealthInfo>("UpdateBossHealth", UpdateBossHealth);
        EventManager.MainInstance.AddEventListening<CharacterHealthInfo>("UpdateBossStrenght", UpdateBossStrenght);
        EventManager.MainInstance.AddEventListening<CharacterHealthInfo>("RecoveryBossStrenght", RecoveryBossStrenght);
    }
    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<CharacterHealthInfo>("UpdateBossHealth", UpdateBossHealth);
        EventManager.MainInstance.RemoveEvent<CharacterHealthInfo>("UpdateBossStrenght", UpdateBossStrenght);
        EventManager.MainInstance.RemoveEvent<CharacterHealthInfo>("RecoveryBossStrenght", RecoveryBossStrenght);
    }

    private void UpdateBossHealth(CharacterHealthInfo healthInfo)
    {
        _currentBossHealth.fillAmount = healthInfo.Cur_HP / healthInfo.HealthData.HP_MAX;
        DevelopmentToos.WTF("hp+  " + healthInfo.Cur_HP);
        StopCoroutine(UpdateBossHealthBack());
        StartCoroutine(UpdateBossHealthBack());
    }
    private void UpdateBossStrenght(CharacterHealthInfo healthInfo)
    {
        _currentBossStrength.fillAmount = (healthInfo.Cur_Strength / healthInfo.HealthData.Strength_MAX);
        StopCoroutine(UpdateBossStrenghtBack());
        StartCoroutine(UpdateBossStrenghtBack());
    }

    IEnumerator UpdateBossHealthBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        bool isFinish = false;
        while (!isFinish)
        {
            if (Mathf.Abs(_currentBossHealth.fillAmount - _currentBossHealthBack.fillAmount) < 0.001f)
            {
                _currentBossHealthBack.fillAmount = _currentBossHealth.fillAmount;
                isFinish = true;
            }
            _currentBossHealthBack.fillAmount = Mathf.Lerp(_currentBossHealthBack.fillAmount, _currentBossHealth.fillAmount, 5f * Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }

    private void RecoveryBossStrenght(CharacterHealthInfo healthInfo)
    {
        StopCoroutine(RecoveryBossStrenghtBack());
        StartCoroutine(RecoveryBossStrenghtBack());
    }

    IEnumerator UpdateBossStrenghtBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        bool isFinish = false;
        while (!isFinish)
        {
            if (Mathf.Abs(_currentBossStrength.fillAmount - _currentBossStrengthBack.fillAmount) < 0.001f)
            {
                _currentBossStrengthBack.fillAmount = _currentBossStrength.fillAmount;
                isFinish = true;
            }
            _currentBossStrengthBack.fillAmount = Mathf.Lerp(_currentBossStrengthBack.fillAmount, _currentBossStrength.fillAmount, 5f * Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }

    IEnumerator RecoveryBossStrenghtBack()
    {
        yield return new WaitForSecondsRealtime(3f);

        bool isFinish = false;
        while (!isFinish)
        {
            if (Mathf.Abs(_currentBossStrength.fillAmount - 1) < 0.01f)
            {
                _currentBossStrength.fillAmount = 1;
                isFinish = true;
            }
            _currentBossStrength.fillAmount = Mathf.Lerp(_currentBossStrength.fillAmount, 1, 8f * Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }

}
