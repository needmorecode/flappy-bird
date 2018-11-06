﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class NotificationCenter : INotificationCenter
{
    private static INotificationCenter singleton;

    private event EventHandler GameOver;
    private event EventHandler ScoreAdd;
	private event EventHandler GameReset;
	private event EventHandler GameStart;

    private NotificationCenter()
        : base()
    {
        // 在这里添加需要分发的各种消息
        eventTable["GameOver"] = GameOver;
        eventTable["ScoreAdd"] = ScoreAdd;
		eventTable["GameReset"] = GameReset;
		eventTable ["GameStart"] = GameStart;
    }

    public static INotificationCenter GetInstance()
    {
        if (singleton == null)
            singleton = new NotificationCenter();
        return singleton;
    }
}

// NotificationCenter的抽象基类
public abstract class INotificationCenter
{

    protected Dictionary<string, EventHandler> eventTable;

    protected INotificationCenter()
    {
        eventTable = new Dictionary<string, EventHandler>();
    }

    // PostNotification -- 将名字为name，发送者为sender，参数为e的消息发送出去
    public void PostNotification(string name)
    {
        this.PostNotification(name, null, EventArgs.Empty);
    }
    public void PostNotification(string name, object sender)
    {
        this.PostNotification(name, sender, EventArgs.Empty);
    }
    public void PostNotification(string name, object sender, EventArgs e)
    {
        if (eventTable[name] != null)
        {
            eventTable[name](sender, e);
        }
    }

    // 添加或者移除了一个回调函数。
    public void AddEventHandler(string name, EventHandler handler)
    {
        eventTable[name] += handler;
    }
    public void RemoveEventHandler(string name, EventHandler handler)
    {
        eventTable[name] -= handler;
    }

}
