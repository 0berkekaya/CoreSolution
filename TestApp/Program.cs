using CoreLibrary;
using CoreLibrary.Interface;
using Docker.DotNet.Models;
using Docker.DotNet;
using System.Runtime.InteropServices;
using TestApp.Docker;
using TestApp.Docker.Enums;

namespace TestApp
{
    public class TestApp
    {
        static async Task Main()
        {

            IOperationResult a = SadeceIslemBilgisiDon(5);
            //IOperationResult<int> b = ResultObjesiyleBirlikteDon(5);
            //Logger.Log.Success(Tools.ObjectToJsonString(a));
            //Console.WriteLine(Tools.ObjectToJsonString(a));


            //Console.WriteLine(TryCatch.Tools.ObjectToJsonString(a));



            //TaskManagerTestMethod();


            DockerManager dockerManager = new();

            //string containerId = await containerManager.CreateContainerAsync(ContainerName.Redis, command: "redis-server");
            await dockerManager.StartContainerWithPrefixAsync(ContainerName.Redis);

            // Mevcut Redis konteynerlerini kontrol et
            //var allRedisContainer = await containerManager.GetContainersAsync(ContainerName.Redis);
            //List<ushort> usedPorts = allRedisContainer.Select(c => c.PrivatePort).ToList();


            //await containerManager.StopAllContainersAsync();

            //await containerManager.RestartContainerAsync(true);

            await dockerManager.DeleteAllContainersWithContainerNameAsync(ContainerName.Redis);

        }

        private static IOperationResult SadeceIslemBilgisiDon(int berke) => TryCatch.Run(() =>
        {
            int a = 0;
            int b = 2;
            int c = 3;
            int result = a + (b * c) + berke;
            throw new NotFiniteNumberException();
            //Console.WriteLine("1. Metot : Doğru Çalıştım");
        }, new()
        {
            Logging = true
        });

        private static IOperationResult<int> ResultObjesiyleBirlikteDon(int berke) => TryCatch.Run(() =>
        {
            int a = 0;
            int b = 2;
            int c = 3;
            int result = a + (b * c) + berke;
            Console.WriteLine("2. Metot : Doğru Çalıştım");
            return result;
        });



        private static void TaskManagerTestMethod()
        {
            TaskManager.Instance.AddTask(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
                Console.WriteLine("Timersız : " + helloWorld);
            }, TaskGroup.Rapor, TaskPriority.VeryHigh);

            TaskManager.Instance.AddTask(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
                Console.WriteLine("Timersız : " + helloWorld);
            }, TaskGroup.Rapor, TaskPriority.VeryHigh);

            TaskManager.Instance.AddTask(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
                Console.WriteLine(helloWorld);
                throw new Exception("HelloMyWorld");
            }, TaskGroup.Rapor, TaskPriority.VeryHigh, null, TimeSpan.FromSeconds(1));

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

            //}, GroupId.Berke, PriorityLevel.Normal);

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

            //}, GroupId.Berke, PriorityLevel.Normal);

            //ActionManager.Manager.Add(() =>
            //{

            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
            //    throw new NotImplementedException();

            //}, GroupId.Berke, PriorityLevel.High);

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

            //}, GroupId.Muhammet, PriorityLevel.Low);

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

            //}, GroupId.Muhammet, PriorityLevel.Normal);

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
            //    throw new AppDomainUnloadedException();
            //}, GroupId.Muhammet, PriorityLevel.Normal);

            //ActionManager.Manager.Add(() =>
            //{
            //    var berke = 24;
            //    string helloWorld = string.Empty;
            //    berke.ToString(string.Empty);
            //    helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

            //}, GroupId.Muhammet, PriorityLevel.VeryHigh);


            //ActionManager.Manager.GetActionsCountWithGroupId(GroupId.Muhammet);
            //ActionManager.Manager.ExecuteWithPriorityLevel(PriorityLevel.VeryHigh);
            //ActionManager.Manager.ExecuteWithActionId(Guid.NewGuid());
            //ActionManager.Manager.ExecuteWithGroupId(GroupId.Berke);
            //ActionManager.Manager.ExecuteWithGroupId(GroupId.Default);

            //ActionObject? berke = actionManager.GetActionObjectWithId(Guid.NewGuid());

            //Console.WriteLine($"{ActionManager.Manager.GetActionsCountWithPriorityLevel(PriorityLevel.Normal)} bu kadar normal görev var.");


            //Console.WriteLine($"{ActionManager.Manager.GetActionCount()} Tüm görev sayısı görev var.");
            //Console.WriteLine($"{ActionManager.Manager.GetActionCount(ActionFilter.Success)} Başarılı görev var.");
            //Console.WriteLine($"{ActionManager.Manager.GetActionCount(ActionFilter.Error)} Başarısız görev var.");
            //Console.WriteLine($"{ActionManager.Manager.GetActionCount(ActionFilter.Null)} Çalışmamış görev var.");

            IEnumerable<(Guid Id, TaskStatus Status)> b = TaskManager.Instance.GetAllTaskStatuses();
            foreach (var item in b)
            {
                Console.WriteLine("ID : " + item.Id + "|" + Tools.ObjectToJsonString(item.Status));

            }
            while (true)
            {
                string? idString = Console.ReadLine();
                if (!string.IsNullOrEmpty(idString)) TaskManager.Instance.StopTask(Tools.StringToGuid(idString));

            }

        }

    }
}

