using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Restrictions;

namespace Planning
{
    static class _Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TestRestrictionsContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void TestRestrictionsContainer()
        {
            //var environment = new Environment<int>();

            //var interval = environment.Restrict();

            //interval.Restrict(5, Direction.Left);
            //interval.Restrict(6, Direction.Left);
            //interval.Restrict(3, Direction.Left);
            //interval.Restrict(6, Direction.Left, false);

            //interval.Restrict(15, Direction.Right);
            //interval.Restrict(10, Direction.Right);
            //interval.Restrict(15, Direction.Right, false);
            //interval.Restrict(20, Direction.Right);

            //var i1 = environment.Restrict();
            //i1.Restrict(2, Direction.Left);
            //i1.Restrict(7, Direction.Left);

            //i1.Restrict(18, Direction.Right);
            //i1.Restrict(10, Direction.Right);

            //var i2 = environment.Restrict();
            //i2.Restrict(2, Direction.Left);
            //i2.Restrict(3, Direction.Left);
            //i2.Restrict(4, Direction.Left);

            //i2.Restrict(8, Direction.Right);
            //i2.Restrict(9, Direction.Right);
            //i2.Restrict(10, Direction.Right);


        }
    }

    //public static class Extender
    //{
    //    public static bool IsGreater<T>(this T a, T b) where T : IComparable<T>
    //    {
    //        if (a == null || b == null)
    //            return false;

    //        return a.CompareTo(b) > 0;
    //    }

    //    public static bool IsLesser<T>(this T a, T b) where T : IComparable<T>
    //    {
    //        if (a == null || b == null)
    //            return false;

    //        return a.CompareTo(b) < 0;
    //    }

    //    public static bool IsGreaterOrEqual<T>(this T a, T b) where T : IComparable<T>
    //    {
    //        if (a == null || b == null)
    //            return false;

    //        return a.CompareTo(b) >= 0;
    //    }

    //    public static bool IsLesseOrEqual<T>(this T a, T b) where T : IComparable<T>
    //    {
    //        if (a == null || b == null)
    //            return false;

    //        return a.CompareTo(b) <= 0;
    //    }

    //    public static bool IsNotEqual<T>(this T a, T b) where T : IComparable<T>
    //    {
    //        if (a == null || b == null)
    //            return false;

    //        return a.CompareTo(b) != 0;
    //    }
    //}
}
