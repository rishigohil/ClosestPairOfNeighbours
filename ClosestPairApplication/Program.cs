using ClosestPairApplication.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace ClosestPairApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClosestPairOfPoints_Result.txt");
            var resultText = new StringBuilder();
            var PointFs = new List<PointF>();
            var PointFSize = 1000;
            var iterationSize = 20000;
            var incrementSize = 1000;
            var processingSize = 5;
            var r = new Random();
            var processingTime = new List<float>();
            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            resultText.AppendLine("Processing Starts..");
            resultText.AppendLine("===========================================================");

            resultText.AppendLine("Calculating Closest Pair of Points");
            resultText.AppendLine(" ");

            for (int n = PointFSize; n <= iterationSize; n += incrementSize)
            {
                PointFs.Clear();

                for (int i = 0; i <= n; i++)
                {
                    PointFs.Add(new PointF()
                    {
                        X = (float)r.NextDouble(),
                        Y = (float)r.NextDouble()
                    });
                }

                resultText.AppendLine("===========================================================");
                resultText.AppendLine(string.Format("Bruce Force for Point: {0}", n));
                resultText.AppendLine("===========================================================");

                for (int i = 0; i < processingSize; i++)
                {
                    Stopwatch timeBruteForce = Stopwatch.StartNew();
                    var smallestDistanceBruteForce = Processor.ShortestBruteForceClosestPair(PointFs);
                    timeBruteForce.Stop();
                    resultText.AppendLine("-----------------------------------------------------------");
                    resultText.AppendLine(string.Format("Smallest Distance: {0}", smallestDistanceBruteForce));
                    resultText.AppendLine("-----------------------------------------------------------");
                    resultText.AppendLine(string.Format("Runtime for {0} attempt : {1}", i + 1, timeBruteForce.Elapsed.Milliseconds));
                    resultText.AppendLine("-----------------------------------------------------------");
                    processingTime.Add(timeBruteForce.ElapsedMilliseconds);
                }

                resultText.AppendLine("-----------------------------------------------------------");
                resultText.AppendLine(string.Format("Average Runtime of Brute Force: {0}", processingTime.Average()));
                resultText.AppendLine("-----------------------------------------------------------");
                resultText.AppendLine(" ");

                processingTime.Clear();

                resultText.AppendLine("===========================================================");
                resultText.AppendLine(string.Format("Divide & Conquer for Point: {0}", n));
                resultText.AppendLine("===========================================================");

                for (int i = 0; i < processingSize; i++)
                {
                    Stopwatch timeDivideConquer = Stopwatch.StartNew();
                    var smallestDistanceDivideConquer = Processor.ShortestDivideAndConquerDistance(PointFs);
                    timeDivideConquer.Stop();
                    resultText.AppendLine("-----------------------------------------------------------");
                    resultText.AppendLine(string.Format("Smallest Distance: {0}", smallestDistanceDivideConquer));
                    resultText.AppendLine(string.Format("Runtime for {0} attempt : {1}", i + 1, timeDivideConquer.Elapsed.Milliseconds));
                    resultText.AppendLine("-----------------------------------------------------------");
                    processingTime.Add((float)timeDivideConquer.ElapsedMilliseconds);
                }

                resultText.AppendLine("-----------------------------------------------------------");
                resultText.AppendLine(string.Format("Average Runtime of Divide & Conquer: {0}", processingTime.Average()));
                resultText.AppendLine("-----------------------------------------------------------");
                resultText.AppendLine(" ");
                processingTime.Clear();

                Helper.DrawTextProgressBar(n, iterationSize);
            }

            resultText.AppendLine("Processing Completed.");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            File.WriteAllText(destPath, resultText.ToString());
        }
    }
}
