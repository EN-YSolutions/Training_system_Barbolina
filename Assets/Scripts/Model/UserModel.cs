using UnityEngine;

[CreateAssetMenu]
public class UserModel : ScriptableObject
{
    public string Id;
    public string UsernName;
    public string Password;
    public string IdCources;
    public int ProgressPoint;

    public UserModel(string id, string username, string password)
    {
        Id = id;
        UsernName = username;
        Password = password;
    }
}
