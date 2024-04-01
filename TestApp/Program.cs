using CoreLibrary;

class TestApp
{
    static void Main()
    {

        taskManagerTest();

    }
    static void taskManagerTest()
    {
        ActionManager actionManager = new ActionManager();

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("3");
            });
        });

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("4");
            });

        }, GroupId.Berke, PriorityLevel.Normal);

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("5");
            });

        }, GroupId.Berke, PriorityLevel.High);

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("6");
            });

        }, GroupId.Muhammet, PriorityLevel.Low);

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("7");
            });

        }, GroupId.Muhammet, PriorityLevel.Normal);

        actionManager.Add(() =>
        {
            TryCatch.Run(() =>
            {
                var berke = 24;
                string helloWorld = string.Empty;
                berke.ToString(string.Empty);
                helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

                throw new CustomExceptions.BerkeExcpt("8");
            });

        }, GroupId.Muhammet, PriorityLevel.VeryHigh);

        //actionManager.ExecuteWithPriorityLevel(PriorityLevel.Normal);
        //actionManager.ExecuteWithActionId(Guid.NewGuid());
        //actionManager.ExecuteWithGroupId(GroupId.Berke);
        actionManager.ExecuteWithGroupId(GroupId.Muhammet);

        Console.WriteLine($"{actionManager.GetActionsCountWithPriorityLevel(PriorityLevel.Normal)} bu kadar normal görev var.");
        Console.WriteLine($"{actionManager.GetAllActionCount()} toplam görev var.");
    }

}

