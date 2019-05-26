using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start schedular app.");
            int[] campaignIds = {150,150,150,150,150,150,151,152,153,154,200};
            //CallWorkerFunctionSimple(campaignIds);
            CallWorkerFunctionWithParent(campaignIds);
            Console.WriteLine("End schedular app.");
            //Environment.ExitCode = 0;
            //Environment.Exit(Environment.ExitCode);
            Console.ReadLine();
        }
        private static void CallWorkerFunctionSimple(int[] campaignIds) {
            List<Task> lstTasks = new List<Task>();
            foreach (int i in campaignIds)
            {
                //Run Console App as worker process
                //Task.Run(()=>StartWorkerProcess(i));

                /***WOrker functioncode****/
                lstTasks.Add(Task.Factory.StartNew(() => StartWorkerFunction(i)));
                //Run other function as worker function
                //Task.Run(() => StartWorkerFunction(i));
            }
            Task.WaitAll(lstTasks.ToArray());
        }
        private static void CallWorkerFunctionWithParent(int[] campaignIds)
        {
            List<Task> lstTasks = new List<Task>();
            Task parentTask = Task.Factory.StartNew(() =>
            {
                foreach (int i in campaignIds)
                {
                    //Run Console App as worker process
                    //Task.Run(()=>StartWorkerProcess(i));

                    /***WOrker functioncode****/
                    Task.Factory.StartNew(() => StartWorkerFunction(i),creationOptions: TaskCreationOptions.AttachedToParent); 
                    //Run other function as worker function
                    //Task.Run(() => StartWorkerFunction(i));
                }
            }, creationOptions: TaskCreationOptions.LongRunning);
            parentTask.Wait();
            //Task.WaitAll(lstTasks.ToArray());
        }
        private void CallWorkerProcess(int[] campaignIds) {
            foreach (int i in campaignIds)
            {
                //Run Console App as worker process
                Task.Run(()=>StartWorkerProcess(i));
            }
        }
        private static void StartWorkerFunction(int campaignId) {
            Console.WriteLine("Worker started of " + campaignId);
            //System.Threading.Thread.Sleep(15000);
            double fact = 1;
            for (int i = 1; i <= campaignId; i++)
            {
                fact = fact * i;
            }
            Console.WriteLine("Factorial of " + campaignId + ":" + fact.ToString());
        }
        private static void StartWorkerProcess(int campaignId)
        {
            Console.WriteLine("Starting worker process for evantid:" + campaignId.ToString());
            ProcessStartInfo startinfo = new ProcessStartInfo(ConfigurationManager.AppSettings["CampaignWorkerAppPath"]);
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = campaignId.ToString();
            //startinfo.UseShellExecute = true;
            startinfo.WindowStyle = ProcessWindowStyle.Normal;
            Process process = Process.Start(startinfo);
            process.WaitForExit();
            var exitCode = process.ExitCode;
            if (exitCode == 0)
            {
                Console.WriteLine("ExitCode for worker process evantid:" + campaignId.ToString() + ", ExitCode:" + exitCode);
            }
            
            //System.Threading.Thread.Sleep(2000);

        }
    }
}
