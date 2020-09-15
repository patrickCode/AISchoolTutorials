using System;
using Newtonsoft.Json;
using ICM_Prediction_POC.Model;

namespace ICM_Prediction_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ICM PREDICTION MODEL");
            Console.WriteLine("=============================================");

            Console.WriteLine("Preparing model...");
            IUserDataProvider inMemDataProvider = new InMemoryUserDataProvider();
            IDataPointFactory dataPointFactory = new DataPointFactory(inMemDataProvider);
            IModel naiveBayesModel = new NaiveBayesModel();

            ModelRunner runner = new ModelRunner(naiveBayesModel, dataPointFactory);
            runner.Prepare();

            Console.WriteLine("Model prepared");

            do
            {
                var userData = new UserData();

                Console.WriteLine("Enter Issue details: ");

                Console.Write("\tIs it a partner user (y/n): ");
                ConsoleKeyInfo key = Console.ReadKey();
                userData.IsPartner = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tDoes user have profile in AAD (y/n): ");
                key = Console.ReadKey();
                userData.ProfileExistsInAAD = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tDoes user have profile in One Profile (y/n): ");
                key = Console.ReadKey();
                userData.ProfileExistyInOP = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tDoes claims exists for user (y/n): ");
                key = Console.ReadKey();
                userData.ClaimsExists = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tIs the Login Expired Windows appearing continously for the user (y/n): ");
                key = Console.ReadKey();
                userData.LoginExpiredMessage = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tDoes application insights have a lot of result code 0 logged for the user (y/n): ");
                key = Console.ReadKey();
                userData.ResultCodeZero = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.Write("\tIs the error AADSTS525 logged in Application Insights for the user (y/n): ");
                key = Console.ReadKey();
                userData.AADSTS525Issue = ConvertKeyToBool(key);
                Console.WriteLine();

                Console.WriteLine("Is this correct (y/n)?");
                Console.WriteLine(JsonConvert.SerializeObject(userData));
                key = Console.ReadKey();
                if (!ConvertKeyToBool(key))
                    continue;

                Console.WriteLine();
                Console.WriteLine("Predicting issue...");
                string issue = runner.PredictIssue(userData);
                Console.WriteLine($"PREDICTED ISSUE: {issue}");
                

                Console.WriteLine("=============================================");

                Console.WriteLine("Do you want to continue");
                key = Console.ReadKey();
                if (!ConvertKeyToBool(key))
                    break;

            } while (true);
        }

        private static bool ConvertKeyToBool(ConsoleKeyInfo key)
        {
            char choice = key.KeyChar;
            return choice == 'y' || choice == 'Y' || choice == '1';
        }
    }
}
