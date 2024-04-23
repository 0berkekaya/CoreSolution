
namespace CoreLibrary
{
    public class ActionManager
    {
        private readonly Dictionary<GroupId, Dictionary<PriorityLevel, List<ActionObject>>> _actionDict = [];
        private readonly string _successAddActionMessage = "Görev Grubu: {0} , ID: {1} , Sonuç: {2}";
        public void Add(Action action, GroupId groupId = GroupId.Default, PriorityLevel level = PriorityLevel.Normal)
        {
            ActionObject? actionObject = null;

            OperationResult operationResult = TryCatch.Run(() =>
            {
                if (!_actionDict.ContainsKey(groupId))
                    _actionDict.Add(groupId, []);

                if (!_actionDict[groupId].ContainsKey(level))
                    _actionDict[groupId][level] = [];

                actionObject = new ActionObject(action);
                _actionDict[groupId][level].Add(actionObject);
            });

            Console.WriteLine(string.Format(_successAddActionMessage, groupId, actionObject?.Id, operationResult.IsSuccessful ? "Başarılı" : operationResult.Error?.Message));
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
        public int GetActionsCountWithGroupId(GroupId groupId)
        {
            if (_actionDict.TryGetValue(groupId, out var priorityDict))
                // Tüm PriorityLevel'lar için ActionObject sayısını topla
                return priorityDict.Values.Sum(actionList => actionList.Count);

            // Eğer verilen groupId'ye ait bir kayıt yoksa 0 döndür
            return 0;
        }
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
                                .SelectMany(levelDict => levelDict.Values.SelectMany(actions => actions.Where(x => x.ActionResult == null)));
            return allActions.Count();
        }
        #endregion

        #region Get Methods
        public ActionObject? GetActionObjectWithId(Guid id)
        {
            return _actionDict.Values
            .SelectMany(priorityDict => priorityDict.Values.SelectMany(list => list))
            .FirstOrDefault(action => action.Id == id);
        }
        #endregion
    }

    public class ActionObject(Action action)
    {
        public delegate void ActionDelegate();
        public ActionDelegate ActionPointer { get; set; } = new ActionDelegate(action);
        public Guid Id { get; set; } = Guid.NewGuid();
        public OperationResult? ActionResult { get; set; }
        public void Execute() => ActionResult = TryCatch.Run(ActionPointer.Invoke);
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
