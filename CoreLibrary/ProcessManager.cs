
using System.Diagnostics;

public class ProcessManager
{
    private static readonly Lazy<ProcessManager> instance = new Lazy<ProcessManager>(() => new ProcessManager());

    private Dictionary<Guid, Process> processes = new Dictionary<Guid, Process>();

    private ProcessManager() { }

    public static ProcessManager Instance => instance.Value;

    public Guid StartProcess(string? processName = null)
    {
        Guid id = Guid.NewGuid();
        Process process = new Process();
        processName ??= "notepad.exe";
        process.StartInfo.FileName = processName;
        process.Start();
        processes[id] = process;
        return id;
    }

    public void StopProcess(Guid id)
    {
        if (processes.ContainsKey(id))
        {
            processes[id].Kill();
            processes.Remove(id);
        }
    }

    public void StopAllProcesses()
    {
        foreach (var process in processes.Values)
        {
            process.Kill();
        }
        processes.Clear();
    }

    public void ListProcesses()
    {
        foreach (var kvp in processes)
        {
            Console.WriteLine($"Process {kvp.Key}: Id = {kvp.Value.Id}, Name = {kvp.Value.ProcessName}");
        }
    }

    public void ManageProcess(Action<Process> action, Guid id)
    {
        if (processes.ContainsKey(id))
        {
            action(processes[id]);
        }
    }
}
