namespace MegaBios
{
    public class DeleteAccount
    {
        public static void RemoveAccount(List<TestAccount> jsonData, TestAccount account)
        {
            for (int i = 0; i < jsonData.Count; i++)
            {
                if (jsonData[i].Email == account.Email && jsonData[i].Wachtwoord == account.Wachtwoord)
                {
                    jsonData.RemoveAt(i);
                    break;
                }
            }
            JsonFunctions.WriteToJson("../../../customers.json", jsonData);
        }
    }
}