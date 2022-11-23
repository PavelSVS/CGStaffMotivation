using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoString : MonoBehaviour
{
    [SerializeField] private TMP_Text user_contact;
    [SerializeField] private TMP_Text prize_getOut_Count;
    [SerializeField] private TMP_Text user_name;

    [SerializeField] private Button b_lookPrizeInfo;

    private int select_user_id;

    private IEnumerator Start() {
        while (!AuthController.Instance)
            yield return new WaitForFixedUpdate();

        b_lookPrizeInfo.onClick.AddListener(() => {
            AuthController.Instance.SetStatsUserID(select_user_id);
            Debug.LogError(select_user_id);
            AuthController.Instance.OpenWindows(WindowsType.get_out_prize_info);
        });
    }

    public void Init(string _contact, string prizeCount, string _name, int selectUserID) {
        user_contact.text = _contact;
        prize_getOut_Count.text = prizeCount;
        user_name.text = _name;

        select_user_id = selectUserID;

        if (select_user_id < 0)
            b_lookPrizeInfo.gameObject.SetActive(false);
    }
}
