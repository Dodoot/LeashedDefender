using UnityEngine;

public class HUDManager : Singleton<HUDManager>
{
    private const string ANIMATOR_LIFE_PARAM = "IsActive";

    [SerializeField] private Animator _animatorLife1 = null;
    [SerializeField] private Animator _animatorLife2 = null;
    [SerializeField] private Animator _animatorLife3 = null;

    public static void Refresh()
    {
        var life = GameManager.Human.CurrentLife;

        _instance._animatorLife1.SetBool(ANIMATOR_LIFE_PARAM, life >= 1);
        _instance._animatorLife2.SetBool(ANIMATOR_LIFE_PARAM, life >= 2);
        _instance._animatorLife3.SetBool(ANIMATOR_LIFE_PARAM, life >= 3);
    }
}
