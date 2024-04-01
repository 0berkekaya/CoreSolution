using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp.Database
{
    public static class Database
    {
        private static Dictionary<int, string> Objects = new Dictionary<int, string>();

        private static List<ChangeInformation> Changes = new List<ChangeInformation>();

        public static Dictionary<int, string> Get()
        {
            return Objects;
        }

        public static void Add(object newObject, string primaryKeyColumn = "Id", ChangeInformationSettings? changeInformationSettings = null)
        {
            throw new NotImplementedException();
            if (newObject == null)
                throw new ArgumentNullException(nameof(newObject));

            ChangeInformation changeInformation = new ChangeInformation(newObject, changeInformationSettings, primaryKeyColumn);
        }

        class ChangeInformation
        {
            public int? Id { get; set; }
            public string? ChangedValues { get; set; }
            public string? OldObject { get; set; }
            public string? NewObject { get; set; }
            public ChangeType ChangeType { get; set; }

            public ChangeInformation(object newObject, ChangeInformationSettings? changeInformationSettings, string primaryKeyPropertyName = "Id")
            {
                string newObjectJson = JsonConvert.SerializeObject(newObject);
                string oldObjectJson = string.Empty;

                changeInformationSettings ??= new ChangeInformationSettings();

                PropertyInfo? newObjectPrimaryKeyProperty = newObject.GetType().GetProperty(primaryKeyPropertyName);
                Id = (int?)newObjectPrimaryKeyProperty?.GetValue(newObject);

                bool isHave = false;
                object? oldObject = null;

                if (Id != null)
                {
                    oldObject = Objects.TryGetValue(Id.Value, out string? oldValue) ? JsonConvert.DeserializeObject(oldValue, newObject.GetType()) : null;
                    isHave = oldObject != null;
                }
                else
                    throw new Exception("Id değeri null");

                oldObjectJson = isHave ? JsonConvert.SerializeObject(oldObject) : string.Empty;

                ChangeType = !isHave ? ChangeType.New : ChangeType.Update;
                OldObject = isHave ? oldObjectJson : string.Empty;
                NewObject = newObjectJson;

                var stringBuilder = new StringBuilder();
                if (isHave)
                {
                    foreach (PropertyInfo property in newObject.GetType().GetProperties())
                    {
                        string propertyName = property.Name;
                        object? newValue = property.GetValue(newObject);
                        object? oldValue = property.GetValue(oldObject);
                        if (!Equals(oldValue, newValue))
                            stringBuilder.Append($"{propertyName}: {oldValue} => {newValue}\n");
                    }
                }


                ChangedValues = stringBuilder.ToString();

                if (ChangeType == ChangeType.New)
                    Objects[Id.Value] = newObjectJson;
                else if (ChangeType == ChangeType.Update)
                {
                    if (Objects.TryGetValue(Id.Value, out string? oldValue))
                    {
                        int index = Changes.FindIndex(x => x.OldObject == oldValue);
                        if (index != -1)
                            Changes[index].NewObject = newObjectJson;

                        Objects[Id.Value] = newObjectJson;
                    }
                    else
                    {
                        Objects[Id.Value] = newObjectJson;
                    }
                }

                Changes.Add(this);
            }
        }

        public class ChangeInformationSettings
        {
            public bool SaveChangedValues { get; set; }
            public bool SaveOldObject { get; set; }
            public bool SaveNewObject { get; set; }
        }

        enum ChangeType
        {
            New = 0,
            Update = 1,
            Delete = 2
        }
    }
}
