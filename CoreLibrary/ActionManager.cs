using System.Text.RegularExpressions;

namespace CoreLibrary
{
    public class ActionManager
    {
        private readonly Dictionary<GroupId, Dictionary<PriorityLevel, List<ActionObject>>> _actionDict = [];
        private readonly string _successAddActionMessage = "Görev Grubu: {0} , ID: {1} , Sonuç: {2}";
        public void Add(Action action, GroupId groupId = GroupId.Default, PriorityLevel level = PriorityLevel.Normal)
        {
            ActionObject? actionObject = null;

            Transaction transaction = TryCatch.Run(() =>
            {
                if (!_actionDict.ContainsKey(groupId))
                    _actionDict.Add(groupId, new Dictionary<PriorityLevel, List<ActionObject>>());

                if (!_actionDict[groupId].ContainsKey(level))
                    _actionDict[groupId][level] = new List<ActionObject>();

                actionObject = new ActionObject(action);
                _actionDict[groupId][level].Add(actionObject);
                //throw new InvalidOperationException();
            });

            Console.WriteLine(string.Format(_successAddActionMessage, groupId, actionObject?.Id, (transaction.IsSuccessful == true ? "Başarılı" : transaction?.Error?.Message)));
        }

        #region Execute Methods
        public void ExecuteWithGroupId(GroupId groupId)
        {
            if (_actionDict.TryGetValue(groupId, out Dictionary<PriorityLevel, List<ActionObject>>? levelDict))
                foreach (List<ActionObject> actions in levelDict.Values)
                    actions.ForEach(action => action.Execute());
        }
        public void ExecuteWithActionId(Guid id)
        {
            IEnumerable<ActionObject> actionsToExecute = _actionDict.Values
                                    .SelectMany(levelDict => levelDict.Values.SelectMany(actions => actions))
                                    .Where(action => action.Id == id);

            if (actionsToExecute.Any())
                foreach (ActionObject action in actionsToExecute)
                    action.Execute();
            else
                Console.WriteLine($"[{id}] Id'ye Ait İşlem Bulunamadı.");
        }
        public void ExecuteWithPriorityLevel(PriorityLevel level)
        {
            IEnumerable<ActionObject> actionsToExecute = _actionDict.Values
                                .Where(levelDict => levelDict.ContainsKey(level))
                                .SelectMany(levelDict => levelDict[level]);

            if (actionsToExecute.Any())
                foreach (ActionObject action in actionsToExecute)
                    action.Execute();
            else
                Console.WriteLine($"[{level}] Öncelikli Kayıtlara Ait İşlem Bulunamadı.");
        }
        #endregion

        #region Count Methods
        public int GetActionsCountWithPriorityLevel(PriorityLevel level)
        {
            IEnumerable<ActionObject> actionsToExecute = _actionDict.Values
                    .Where(levelDict => levelDict.ContainsKey(level))
                    .SelectMany(levelDict => levelDict[level]);

            return actionsToExecute.Count();
        }
        public int GetAllActionCount()
        {
            IEnumerable<ActionObject> allActions = _actionDict.Values
                                .SelectMany(levelDict => levelDict.Values.SelectMany(actions => actions.Where(x => x.Transaction == null)));
            return allActions.Count();
        }
        #endregion
    }

    public class ActionObject(Action action)
    {
        public delegate void ActionDelegate();
        public ActionDelegate ActionPointer { get; set; } = new ActionDelegate(action);

        public Guid Id { get; set; } = Guid.NewGuid();
        public Transaction? Transaction { get; set; }
        public void Execute() => Transaction = TryCatch.Run(ActionPointer.Invoke);
    }

    public enum PriorityLevel
    {
        Low = 1,
        Normal = 2,
        Medium = 3,
        High = 4,
        VeryHigh = 5
    }
    public enum GroupId
    {
        Default = 0,
        Berke = 1,
        Apo = 2,
        Talha = 3,
        Rapor = 4,
        Muhammet = 5
    }
}
