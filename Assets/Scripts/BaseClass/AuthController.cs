using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthController : MonoBehaviour {
    private static AuthController instance;
    public static AuthController Instance => instance;

    [SerializeField] private List<IWindows> windows = new List<IWindows>();

    private UserInfo user_data;
    private PrizeInfo prizeInfo;
    private int statsUser_ID;

    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(0.5f);
        OpenWindows(WindowsType.login);
    }

    public void InitWindows(IWindows window) {
        if (windows.Contains(window))
            return;

        windows.Add(window);
    }

    public void OpenWindows(WindowsType _type) {
        for (int i = 0; i < windows.Count; i++) {
            if (windows[i].GetWindowType() == _type) {
                windows[i].OpenWindow();
            }
            else
                windows[i].CloseWindows();
        }
    }

    public void SetUserData(UserInfo data) {
        user_data = data;
    }

    public UserInfo GetUserInfo() {
        return user_data;
    }

    public void SetPrizeData(PrizeInfo prize) {
        prizeInfo = prize;
    }

    public PrizeInfo GetPrizeInfo() {
        return prizeInfo;
    }

    public void ClearPrizeInfo() {
        prizeInfo = null;
    }

    public void SetStatsUserID(int _id) {
        statsUser_ID = _id;
    }

    public int GetStatsUserID() {
        return statsUser_ID;
    }

    public void ClearStatsID() {
        statsUser_ID = -1;
    }
}
