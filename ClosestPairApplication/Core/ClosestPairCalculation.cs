using System;
using System.Collections.Generic;
using System.Linq;

namespace ClosestPairApplication.Core
{
    public static class ClosestPairCalculation
    {
        public static double ClosestPairBruteForce(List<Plot> plotList, int n)
        {
            double result = double.MaxValue;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; ++j)
                {
                    var closestDistance = FindClosestDistanceBruceForce(plotList[i], plotList[j]);
                    result = closestDistance;
                }
            }
            return result;
        }

        private static double FindClosestDistanceBruceForce(Plot p1, Plot p2)
        {
            var xCalc = (p1.X - p2.X);
            var yCalc = (p1.Y - p2.Y);
            var pointCalc = Math.Pow(xCalc, 2) + Math.Pow(yCalc, 2);
            var result = Math.Sqrt(pointCalc);

            return result;
        }

        public static double ClosestPaidDivideAndConquer(List<Plot> plotList, int n)
        {
            plotList = plotList.OrderBy(a => a.X).ToList();

            if (n <= 3)
                return ClosestPairBruteForce(plotList, n);

            int midPlotPoint = n / 2;
            var midPlot = plotList[midPlotPoint];

            var rightPlots = plotList.Skip(midPlotPoint).Take(n - midPlotPoint).ToList();
            var distanceLeft = ClosestPaidDivideAndConquer(plotList, midPlotPoint);
            var distanceRight = ClosestPaidDivideAndConquer(rightPlots, n - midPlotPoint);

            var minimumDistance = CalculateMinimumDistance(distanceLeft, distanceRight);

            var closestStrip = new List<Plot>();

            for (int i = 0; i < n; i++)
            {
                var xPoint = Math.Abs(plotList[i].X - midPlot.X);
                if (xPoint < minimumDistance)
                {
                    closestStrip.Add(plotList[i]);
                }
            }

            var closestDistanceStripped = MinimumDistanceStrippedPlot(closestStrip, closestStrip.Count, minimumDistance);

            var result = CalculateMinimumDistance(minimumDistance, closestDistanceStripped);

            return result;
        }

        private static double CalculateMinimumDistance(double x, double y)
        {
            var minDistance = (x < y) ? x : y;
            return minDistance;
        }

        private static double MinimumDistanceStrippedPlot(List<Plot> plotList, int n, double d)
        {
            var resultPoint = d;
            plotList = plotList.OrderBy(a => a.Y).ToList();

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n && (plotList[j].Y - plotList[i].Y) < resultPoint; ++j)
                {
                    var calculateClosestDistance = FindClosestDistanceBruceForce(plotList[i], plotList[j]);
                    if (calculateClosestDistance < resultPoint)
                    {
                        resultPoint = FindClosestDistanceBruceForce(plotList[i], plotList[j]);
                    }
                }
            }

            return resultPoint;
        }
    }
}
