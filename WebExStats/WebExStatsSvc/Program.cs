using System.ServiceProcess;

namespace WebExStatsSvc
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new DataSvc()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
