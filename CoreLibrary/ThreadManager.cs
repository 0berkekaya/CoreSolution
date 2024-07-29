public class ThreadManager
{
    private static readonly Lazy<ThreadManager> instance = new Lazy<ThreadManager>(() => new ThreadManager());

    private Dictionary<Guid, (Thread thread, CancellationTokenSource cts)> threads = new Dictionary<Guid, (Thread, CancellationTokenSource)>();

    private ThreadManager() { }

    public static ThreadManager Instance => instance.Value;

    public Guid CreateThread(Action<CancellationToken> action)
    {
        Guid id = Guid.NewGuid();
        var cts = new CancellationTokenSource();
        Thread thread = new Thread(() => action(cts.Token));
        threads[id] = (thread, cts);
        thread.Start();
        return id;
    }

    public void StopThread(Guid id)
    {
        if (threads.ContainsKey(id))
        {
            threads[id].cts.Cancel();
            threads[id].thread.Join(); // Wait for the thread to finish
            threads.Remove(id);
        }
    }

    public void StopAllThreads()
    {
        foreach (var kvp in threads)
        {
            kvp.Value.cts.Cancel();
            kvp.Value.thread.Join(); // Wait for all threads to finish
        }
        threads.Clear();
    }

    public void ListThreads()
    {
        foreach (var kvp in threads)
        {
            Console.WriteLine($"Thread {kvp.Key}: IsAlive = {kvp.Value.thread.IsAlive}");
        }
    }

    public void ManageThread(Action<Thread> action, Guid id)
    {
        if (threads.ContainsKey(id))
        {
            action(threads[id].thread);
        }
    }
}
