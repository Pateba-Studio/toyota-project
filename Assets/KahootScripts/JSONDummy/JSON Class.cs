using UnityEngine;
public class JSONClass
{
    public int id;
    public string name;
    public string avatar;
    public string email;

    public JSONClass(int _id, string _name, string _email, string _avatar)
    {
        id = _id;
        name = _name;
        email = _email;
        avatar = _avatar;
    }
}
