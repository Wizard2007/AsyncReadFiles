using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AsyncReadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileList = Directory.GetFiles(@"c:\tmp\", "*.*", SearchOption.AllDirectories);
            Console.WriteLine("Start Task");
            
            var t = Task.Factory.StartNew(async () =>
            {
                Console.WriteLine(await ProcessFileList(fileList));
            });

            var c = t.ContinueWith((antecedent) => { antecedent.Wait(); Console.WriteLine("ContinueWith - Task"); }, TaskContinuationOptions.RunContinuationsAsynchronously);

            c.Wait();
            Console.WriteLine("Task Completed ");
            Console.ReadKey();
            
        }

        static async Task<int> ProcessFileList(string[] fileList)
        {
            int total = 0;
            foreach (var fileName in fileList)
            {
                Console.WriteLine($"start proces : {fileName}");
                using (StreamReader reader = new StreamReader(fileName))
                {
                    var text = await reader.ReadToEndAsync();
                    total += CountChars(text);
                    Console.WriteLine($"processed : {fileName}");
                }
            }
            return total;
        }
        static int CountChars(string text)
        {
            int count = 0;
            foreach(var symbol in text)
            {
                if (symbol == 'a')
                {
                    count++;
                }
            }
            return count;
        }
    }
}
