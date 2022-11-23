using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebData
{
    private static string headerName = "HTTP_X_REQUESTED_WITH";
    public static string HeaderName => headerName;
    private static string headerValue = "XMLHttpRequest";
    public static string HeaderValue => headerValue;

    private static string context_type = "HTTP_ACCEPT";
    public static string ContexTypeName => context_type;
    private static string context_type_value = "application/json";
    public static string ContexTypeValue => context_type_value;

    private static string domain = "https://cloudsgoods.com";
    public static string Domain => domain;

    private static string userPath = "/api";
    public static string UserPath => domain + userPath;

    private static string request_prise_info = "/api/games/stocks";
    public static string RequestPrizeInfo => domain + request_prise_info;

    private static string give_prize_out = "/api/games/stocks";
    public static string GivePrizeOut = domain + give_prize_out;
}

[System.Serializable]
internal struct AppData {
    public UserInfo user_data { get { return user_data; } set { user_data = value; } }
    public PrizeInfo prizeInfo { get { return prizeInfo; } set { prizeInfo = value; } }
    public int statsUser_ID { get { return statsUser_ID; } set { statsUser_ID = value; } }
}

public enum WindowsType { login, stats, qr_read, prize_demo, get_out_prize_info };

public interface IWindows {
    public static WindowsType w_type;

    public WindowsType GetWindowType();

    public void OpenWindow();

    public void CloseWindows();
}

public class CallResult {  
    public string success;
    public string[] error = new string[0];
}

[System.Serializable]
public class UserCallResult : CallResult {
    public UserInfo user;
}

[System.Serializable]
public class UserInfo {
    public int id;
    public string login;
    public string name; 
    public string tmp_email;
    public string email;
    public string phone;
    public string type;
    public string register_date;
    public int show_info_block;
    public int cloud_filesize;
    public int auto_renew_tariff;
    public int number_players;

    public string gender;
    public int gender_updated;

    public string date_of_birth;
    public int date_of_birth_updated;

    public string about_me;
    public string balance;
    public string api_key;
    public int coins;

    public int available_games_stocks;

    public int available_avatars;
    public int available_avatars_games;
}

[System.Serializable]
public class PrizeCalResult : CallResult {
    public PrizeInfo[] data;
}

[System.Serializable]
public class PrizeInfo { 
    public int id;
    public int games_stocks_id;
    public int game_prizes_id;
    public int place;

    public string coupon_number;

    public string created_at;
    public string updated_at;

    public int active;
    public int reserved;

    public PrizeBlock prize;
    public WinInfo win_user;
}

[System.Serializable]
public class PrizeBlock {
    public GamePrizesData games_prizes_data;
}

[System.Serializable]
public class GamePrizesData { 
    public ObjectPrizeData object_data;
}

[System.Serializable]
public class ObjectPrizeData {
    public string title;
    public string default_look_preview;
}

[System.Serializable]
public class WinInfo {
    public int id;
    public int user_id;
    public int received;
    public string received_at;

    public string qr_code;

    public WinUserInfo user;
}

[System.Serializable]
public class WinUserInfo {
    public string phone;
    public string email;
}

[System.Serializable]
public class StatsUserData : CallResult {
    public WinUserInfo[] _value;
}