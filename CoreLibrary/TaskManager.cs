using CoreLibrary;
using CoreLibrary.Interface;
using System.Collections.Concurrent;

public enum TaskPriority
{
    Low,
    Normal,
    Medium,
    High,
    VeryHigh
}

public enum TaskGroup
{
    Default,
    Berke,
    Apo,
    Talha,
    Rapor,
    Muhammet
}

public enum TaskStatus
{
    Pending,
    Running,
    Completed,
    Failed,
    Stopped
}

public class TaskManager
{
    private readonly ConcurrentDictionary<TaskGroup, ConcurrentDictionary<TaskPriority, ConcurrentQueue<ManagedTask>>> _taskQueues;
    private readonly ConcurrentDictionary<Guid, ManagedTask> _taskLookup;
    private readonly ConcurrentQueue<Task> _scheduledTasks;
    private static readonly Lazy<TaskManager> lazyInstance = new Lazy<TaskManager>(() => new TaskManager());
    private static readonly string _logFileName = "taskManagerLogs";
    public static TaskManager Instance => lazyInstance.Value;


    private TaskManager()
    {
        _taskQueues = new ConcurrentDictionary<TaskGroup, ConcurrentDictionary<TaskPriority, ConcurrentQueue<ManagedTask>>>();
        _taskLookup = new ConcurrentDictionary<Guid, ManagedTask>();
        _scheduledTasks = new ConcurrentQueue<Task>();
    }

    #region Add Methods
    public Guid AddTask(Action task, TaskGroup group = TaskGroup.Default, TaskPriority priority = TaskPriority.Normal, TimeSpan? initialDelay = null, TimeSpan? interval = null)
    {
        var managedTask = new ManagedTask(task, initialDelay, interval);
        var priorityQueue = _taskQueues.GetOrAdd(group, new ConcurrentDictionary<TaskPriority, ConcurrentQueue<ManagedTask>>())
                                        .GetOrAdd(priority, new ConcurrentQueue<ManagedTask>());
        priorityQueue.Enqueue(managedTask);
        _taskLookup[managedTask.Id] = managedTask;

        if (initialDelay.HasValue || interval.HasValue)
        {
            ScheduleTask(managedTask, initialDelay ?? TimeSpan.Zero, interval);
        }
        Logger.Instance.Add(LogLevel.Info, $"Task Added! Group: {group}, Priority: {priority}, ID: {managedTask.Id}", _logFileName);
        return managedTask.Id;
    }

    private void ScheduleTask(ManagedTask managedTask, TimeSpan initialDelay, TimeSpan? interval)
    {
        _scheduledTasks.Enqueue(Task.Run(async () =>
        {
            await Task.Delay(initialDelay);
            managedTask.Execute();
            while (interval.HasValue)
            {
                await Task.Delay(interval.Value);
                managedTask.Execute();
            }
        }));
    }
    #endregion

    #region Execute Methods
    public void ExecuteTasks(TaskGroup group, TaskPriority? priority = null)
    {
        if (_taskQueues.TryGetValue(group, out var priorityDict))
        {
            var queues = priority.HasValue ? new[] { priorityDict[priority.Value] } : priorityDict.Values;
            foreach (var queue in queues)
            {
                while (queue.TryDequeue(out var managedTask))
                {
                    managedTask.Execute();
                }
            }
            Logger.Instance.Add(LogLevel.Info, $"Executed tasks for Group: {group}", _logFileName);
        }
        else
        {
            Logger.Instance.Add(LogLevel.Error, $"No tasks found for Group: {group}", _logFileName);
        }
    }

    public void ExecuteTaskById(Guid id)
    {
        if (_taskLookup.TryGetValue(id, out var managedTask))
        {
            managedTask.Execute();
            Logger.Instance.Add(LogLevel.Info, $"Executed task with ID: {id}", _logFileName);
        }
        else
        {
            Logger.Instance.Add(LogLevel.Error, $"Task with ID: {id} not found", _logFileName);
        }
    }

    public void ExecuteAllTasks()
    {
        foreach (var group in _taskQueues.Keys)
        {
            ExecuteTasks(group);
        }
        Logger.Instance.Add(LogLevel.Info, "Executed all tasks", _logFileName);
    }
    #endregion

    #region Stop Methods
    public void StopTask(Guid id)
    {
        if (_taskLookup.TryGetValue(id, out var managedTask))
        {
            managedTask.Stop();
            Logger.Instance.Add(LogLevel.Info, $"Task with ID: {id} stopped", _logFileName);
        }
        else
        {
            Logger.Instance.Add(LogLevel.Error, $"Task with ID: {id} not found", _logFileName);
        }
    }

    public void StopAllTasks()
    {
        foreach (var managedTask in _taskLookup.Values)
        {
            managedTask.Stop();
        }
        Logger.Instance.Add(LogLevel.Info, "Stopped all tasks", _logFileName);
    }
    #endregion

    #region Status Methods
    public TaskStatus GetTaskStatus(Guid id)
    {
        if (_taskLookup.TryGetValue(id, out var managedTask))
        {
            return managedTask.CurrentStatus;
        }
        else
        {
            throw new ArgumentException($"Task with ID: {id} not found.");
        }
    }

    public IEnumerable<(Guid Id, TaskStatus Status)> GetAllTaskStatuses()
    {
        return _taskLookup.Select(kvp => (kvp.Key, kvp.Value.CurrentStatus));
    }
    #endregion

    #region Exta Methods

    #endregion
}

public class ManagedTask
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    private readonly Action _task;
    private readonly TimeSpan? _interval;
    private Timer? _timer;

    public TaskStatus CurrentStatus { get; private set; } = TaskStatus.Pending;

    public int ExecuteCount { get; private set; }
    public int SuccessCount { get; private set; }
    public int FailureCount { get; private set; }

    public ManagedTask(Action task, TimeSpan? initialDelay = null, TimeSpan? interval = null)
    {
        _task = task;
        _interval = interval;

        if (initialDelay.HasValue) _timer = new Timer(ExecuteCallback, null, initialDelay.Value, interval ?? Timeout.InfiniteTimeSpan);
    }

    private void ExecuteCallback(object? state) => Execute();

    public void Execute()
    {
        if (CurrentStatus == TaskStatus.Stopped) return;

        CurrentStatus = TaskStatus.Running;
        ExecuteCount++;
        IOperationResult result = TryCatch.Run(_task);

        if (result.Success)
        {
            SuccessCount++;
            CurrentStatus = TaskStatus.Completed;
        }
        else
        {
            FailureCount++;
            CurrentStatus = TaskStatus.Failed;
            result.Message = "Task Id : " + Id;
            Logger.Instance.Add(LogLevel.Error, Tools.ObjectToJsonString(result), "taskManagerLogs.txt");
        }

        if (!_interval.HasValue)
        {
            Stop();
        }
    }

    public void Stop()
    {
        CurrentStatus = TaskStatus.Stopped;
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }
}
