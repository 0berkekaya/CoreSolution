using CoreLibrary;

class TestApp
{
    static void Main(string[] args)
    {

        OperationResult a = sadeceIslemBilgisiDon();
        OperationResult<int> b = resultObjesiyleBirlikteDon();


        //taskManagerTest();

    }

    private static OperationResult sadeceIslemBilgisiDon() => TryCatch.Run(() =>
    {
        int a = 0;
        int b = 2;
        int c = 3;
        int result = a + (b * c);
        Console.WriteLine("1. Metot : Doğru Çalıştım");
    });

    private static OperationResult<int> resultObjesiyleBirlikteDon() => TryCatch.Run(() =>
    {
        int a = 0;
        int b = 2;
        int c = 3;
        int result = a + (b * c);
        Console.WriteLine("2. Metot : Doğru Çalıştım");
        return result;
    });



    static void taskManagerTest()
    {
        ActionManager actionManager = new();
        ActionManager actionManager1 = new();

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');
        });

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Berke, PriorityLevel.Normal);

        actionManager1.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Berke, PriorityLevel.Normal);

        actionManager.Add(() =>
        {

            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Berke, PriorityLevel.High);

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Muhammet, PriorityLevel.Low);

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Muhammet, PriorityLevel.Normal);

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Muhammet, PriorityLevel.Normal);

        actionManager.Add(() =>
        {
            var berke = 24;
            string helloWorld = string.Empty;
            berke.ToString(string.Empty);
            helloWorld = "     Hello World       ".Trim().PadLeft(2, '0');

        }, GroupId.Muhammet, PriorityLevel.VeryHigh);


        actionManager.GetActionsCountWithGroupId(GroupId.Muhammet);
        actionManager.ExecuteWithPriorityLevel(PriorityLevel.Normal);
        actionManager.ExecuteWithActionId(Guid.NewGuid());
        actionManager.ExecuteWithGroupId(GroupId.Berke);
        actionManager.ExecuteWithGroupId(GroupId.Muhammet);

        //ActionObject? berke = actionManager.GetActionObjectWithId(Guid.NewGuid());

        Console.WriteLine($"{actionManager.GetActionsCountWithPriorityLevel(PriorityLevel.Normal)} bu kadar normal görev var.");
        Console.WriteLine($"{actionManager.GetAllActionCount()} toplam görev var.");
    }

}

