using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restrictions.View
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var absolute = new Absolute();
            Absolute.Item item = absolute.Add("main");

            var subitem1 = item.Add("sub.1");
            var subitem2 = item.Add("sub.2");

            subitem2.Start.Move(5);

            item.Start.Move(10);
            item.Start.Move(-10);

            item.Start.Move(5);
            item.Start.Move(-20);

            //subitem.RestrictMinDuration(33, true);

            //item.RestrictMaxDuration(100);
            //item.RestrictMinDuration(5, false);

            //item.OffsetStart(10);

            //absolute.Res(item.Start).Restrictions.Restrict(15, Direction.Left);
            //absolute.Res(item.Finish).Restrictions.Restrict(30, Direction.Left);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
