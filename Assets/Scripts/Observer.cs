using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Observer : MonoBehaviour {
	private INotificationCenter center = NotificationCenter.GetInstance();
	private Dictionary<string, EventHandler> handlers = new Dictionary<string, EventHandler>();

    void Awake() {
        //handlers = new Dictionary<string, EventHandler>();
        //center = NotificationCenter.GetInstance();
    }

    void OnDestroy() {
        foreach (KeyValuePair<string, EventHandler> kvp in handlers) {
            center.RemoveEventHandler(kvp.Key, kvp.Value);
        }
    }

    public void AddEventHandler(string name, EventHandler handler) {
        center.AddEventHandler(name, handler);
        handlers.Add(name, handler);
    }
}
