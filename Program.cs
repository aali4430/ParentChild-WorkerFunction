using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ParentWorkerApp
{
    class Program
    {
        static int len = 0;
        
        static Stopwatch stopWatch = null ;
        static double[] iarray= new double [10];
        static int counter = 0;
        static void Main(string[] args)
        {
            
                len = 0;
                stopWatch = Stopwatch.StartNew();
                Console.WriteLine("Start schedular app.");
                int[] campaignIds = { 2000, 3000, 4000 };
                //CallWorkerProcess(campaignIds);
                CallWorkerFunctionSimple(campaignIds);
                //CallWorkerFunctionWithParent(campaignIds);
                Console.WriteLine("End schedular app.");
            
            
            //Environment.ExitCode = 0;
            //Environment.Exit(Environment.ExitCode);
            
            Console.ReadLine();
            

        }
            private static async void CallWorkerFunctionSimple(int[] campaignIds) {
            
            List<Task> lstTasks = new List<Task>();
            //Console.WriteLine("----------Parallel-------------------");
            //Parallel.ForEach(campaignIds, async (i) =>
            // {
            //     /********  With Task . Run **********/
            //     Console.WriteLine("Queueing Task for " + i);
            //     BigInteger d = await Task.Run(() => StartWorkerFunction(i));
            //     Console.WriteLine("End Queuing task for" + i);
            //     /****** With Task.Factory ********/
            //     //Console.WriteLine("Queueing Task for " + i);
            //     //await Task.Factory.StartNew(() => StartWorkerFunction(i));
            //     //Console.WriteLine("End Queuing task for" + i);





            // });
            Console.WriteLine("----------Simple-------------------");
            foreach (int i in campaignIds)
            {
                /********  With Task . Run **********/
                Console.WriteLine("Queueing Task for " + i);
                BigInteger d = await Task.Run(() => StartWorkerFunction(i));
                Console.WriteLine("End Queuing task for" + i);
                /****** With Task.Factory ********/
                //Console.WriteLine("Queueing Task for " + i);
                //await Task.Factory.StartNew(() => StartWorkerFunction(i));
                //Console.WriteLine("End Queuing task for" + i);
            }
            //Task.WaitAll(lstTasks.ToArray());

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
        private static void CallWorkerProcess(int[] campaignIds) {
            foreach (int i in campaignIds)
            {
                //Run Console App as worker process
                Task.Run(()=>StartWorkerProcess(i));
            }
        }
        private static async Task<BigInteger> StartWorkerFunction(int campaignId) {
            //System.Threading.Thread.Sleep(5000);
            Console.WriteLine("Worker started of " + campaignId+", Thread:"+ System.Threading.Thread.CurrentThread.ManagedThreadId);

            //System.Threading.Thread.Sleep(15000);

            BigInteger result = await Task.Run(() => {
                BigInteger fact = 1;
                for (BigInteger i = 1; i <= campaignId; i++)
                {
                    fact = fact * i;
                    
                }
                return fact;
            });
            Console.WriteLine("Worker ended of " + campaignId);
            len++;
            //counter++;
            if (len == 3)
            {
                Console.WriteLine("Worker ended of " + stopWatch.Elapsed.TotalSeconds);
               
            }
            
                
            return result;
            
          
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
