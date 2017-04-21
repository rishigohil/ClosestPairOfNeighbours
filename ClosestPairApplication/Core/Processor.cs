using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosestPairApplication.Core
{
    public class PointF
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Segment
    {
        public Segment(PointF p1, PointF p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public readonly PointF P1;
        public readonly PointF P2;

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (P1.X - P2.X) * (P1.X - P2.X)
                + (P1.Y - P2.Y) * (P1.Y - P2.Y);
        }
    }

    public static class Processor
    {
        public static float ShortestBruteForceClosestPair(List<PointF> pts)
        {
            var result = Closest_BruteForce(pts);
            return result.Length();
        }

        public static float ShortestDivideAndConquerDistance(List<PointF> pts)
        {
            var result = MyClosestDivide(pts);
            return result.Length();
        }

        private static Segment Closest_BruteForce(List<PointF> points)
        {
            var result = new Segment(points[0], points[1]);
            var bestLength = result.Length();

            for (int i = 0; i < points.Count; i++)
                for (int j = i + 1; j < points.Count; j++)
                    if (FindClosestDistanceBruceForce(points[i], points[j]) < bestLength)
                    {
                        result = new Segment(points[i], points[j]);
                        bestLength = result.Length();
                    }

            return result;
        }

        private static Segment MyClosestDivide(List<PointF> points)
        {
            return MyClosestRec(points.OrderBy(p => p.X).ToList());
        }

        private static Segment MyClosestRec(List<PointF> pointsByX)
        {
            int count = pointsByX.Count;
            if (count <= 4)
                return Closest_BruteForce(pointsByX);

            // left and right lists sorted by X, as order retained from full list
            var leftByX = pointsByX.Take(count / 2).ToList();
            var leftResult = MyClosestRec(leftByX);

            var rightByX = pointsByX.Skip(count / 2).ToList();
            var rightResult = MyClosestRec(rightByX);

            var result = rightResult.Length() < leftResult.Length() ? rightResult : leftResult;

            // There may be a shorter distance that crosses the divider
            // Thus, extract all the points within result.Length either side
            var midX = leftByX.Last().X;
            var bandWidth = result.Length();
            var inBandByX = pointsByX.Where(p => Math.Abs(midX - p.X) <= bandWidth);

            // Sort by Y, so we can efficiently check for closer pairs
            var inBandByY = inBandByX.OrderBy(p => p.Y).ToArray();

            int iLast = inBandByY.Length - 1;
            for (int i = 0; i < iLast; i++)
            {
                var pLower = inBandByY[i];

                for (int j = i + 1; j <= iLast; j++)
                {
                    var pUpper = inBandByY[j];

                    // Comparing each point to successivly increasing Y values
                    // Thus, can terminate as soon as deltaY is greater than best result
                    if ((pUpper.Y - pLower.Y) >= result.Length())
                        break;

                    if (FindClosestDistanceBruceForce(pLower, pUpper) < result.Length())
                        result = new Segment(pLower, pUpper);
                }
            }

            return result;
        }

        private static double FindClosestDistanceBruceForce(PointF p1, PointF p2)
        {
            var xCalc = (p1.X - p2.X);
            var yCalc = (p1.Y - p2.Y);
            var pointCalc = Math.Pow(xCalc, 2) + Math.Pow(yCalc, 2);
            var result = Math.Sqrt(pointCalc);

            return result;
        }
    }
}
