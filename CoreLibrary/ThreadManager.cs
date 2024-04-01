using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CoreLibrary
{
    //public static class ThreadManager
    //{
    //    private static Dictionary<Guid, ThreadObject> _threadObjects = new();

    //    public static void StartThread(string threadName)
    //    {
    //        bool threadIsHave = _threadObjects.Any(x => x.Value.ThreadName == threadName);
    //        bool isRunning = _threadObjects.Where(t => t.Value.ThreadName == threadName).FirstOrDefault().Value.IsRunning;

    //        if (!threadIsHave)
    //        {
    //            ThreadObject tObject = new ThreadObject()
    //            {
    //                ThreadName = threadName,
    //                TaskQueues = new ConcurrentQueue<Action>(),
    //                Threads = new List<Thread>(),
    //            };

    //            _threadObjects.Add(Guid.NewGuid(), tObject);

    //            tObject.Threads.Add(new Thread(new ThreadStart(() => Run(threadName))));
    //            tObject.Threads.ForEach(t => t.Start());
    //        }
    //        else if (isRunning)
    //            Console.WriteLine("Bu isimde zaten bir thread mevcut.");
    //        else
    //            Console.WriteLine("Bu isimdeki thread bitmiştir.");
    //    }
    //    public static void AddAction(string threadName, Action action)
    //    {
    //        ThreadObject? tObject = findThreadObject(threadName);
    //        if (tObject != null && tObject.TaskQueues.con)
    //        {
    //            // Belirtilen isimdeki thread'in kuyruğuna yeni bir aksiyon eklenir
    //            actionQueues[threadName].Enqueue(action);
    //        }
    //        else
    //            Console.WriteLine("Belirtilen isimde bir thread bulunamadı.");
    //    }
    //    public static void StopThread(string threadName)
    //    {
    //        if (threadDictionary.ContainsKey(threadName))
    //        {
    //            isRunning = false;
    //            threadDictionary[threadName].Join();
    //            threadDictionary.Remove(threadName);
    //            actionQueues.TryRemove(threadName, out _);
    //        }
    //        else
    //            Console.WriteLine("Belirtilen isimde bir thread bulunamadı.");
    //    }

    //    private static void Run(string threadName)
    //    {
    //        isRunning = true;
    //        while (isRunning)
    //        {
    //            Action? action;
    //            // Belirtilen isimdeki thread'in kuyruğundan bir aksiyon alınır
    //            if (actionQueues[threadName].TryDequeue(out action))
    //            {
    //                // Alınan aksiyon çalıştırılır
    //                action();
    //            }
    //            else
    //            {
    //                // Eğer kuyruk boşsa, bir süre uyutulur
    //                Thread.Sleep(100);
    //            }
    //        }
    //    }

    //    private static ThreadObject? findThreadObject(string threadName)
    //    {
    //        if (_threadObjects.Count() > 0 && _threadObjects != null)
    //        {
    //            return _threadObjects.Where(t => t.Value.ThreadName == threadName).FirstOrDefault().Value;
    //        }
    //        else
    //        {
    //            return null;
    //        }

    //    }
    //}

    //public class ThreadObject
    //{
    //    public string ThreadName { get; set; } = "Default-Thread";
    //    public bool IsRunning { get; set; } = false;
    //    public ConcurrentQueue<Action>? TaskQueues { get; set; }
    //    public List<Thread>? Threads { get; set; }
    //}
    //public enum ThreadLevel
    //{
    //    Low = 1,
    //    Normal = 2,
    //    Medium = 3,
    //    High = 4,
    //    VeryHigh = 5
    //}

}

