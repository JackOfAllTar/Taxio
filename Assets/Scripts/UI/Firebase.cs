using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;

public class Firebase : MonoBehaviour
{
    [SerializeField] private string url = "https://taxio452-default-rtdb.asia-southeast1.firebasedatabase.app/";
    [SerializeField] private string secret = "3wYjFjE8E8Xj2iTTS20JHK5JUkeWU5QNEr32rXai";

    [System.Serializable]
    public class User
    {
        [System.Serializable]
        public class UserData
        {
            public string Name;
            public int Number;
            public int Points;

            public UserData(string name, int number, int points)
            {
                this.Name = name;
                this.Number = number;
                this.Points = points;
            }
        }
        public List<UserData> userData;
    }
    public User user;
    public event Action<User> OnLoaded;

    private void Start()
    {
        GetData();
    }

    public void GetData()
    {
        string urlData = $"{url}User.json?auth={secret}";

        RestClient.Get<User>(urlData).Then(response =>
        {
            user = response;
            OnLoaded?.Invoke(user);
        }).Catch(error =>
        {
            print(error.Message);
        });
    }

    public void SetData()
    {
        string urlData = $"{url}User.json?auth={secret}";

        RestClient.Put<User>(urlData, user).Then(response =>
        {
            Debug.Log("Data uploaded");
        }).Catch(error =>
        {
            Debug.Log("Error on set to server");
        });
    }

    public void Submit(User user)
    {
        string urlData = $"{url}User.json?auth={secret}";
        RestClient.Put<User>(urlData, user).Then(response =>
        {
            Debug.Log("Submit Data to RealtimeDatabase!");
        }).Catch(error =>
        {
            Debug.Log("Can not Submit Data! Data is Empty OR Null.");
        });
    }
}
