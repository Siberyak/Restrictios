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
            Absolute.Item item = absolute.Add();

            item.RestrictMaxDuration(100);
            item.RestrictMinDuration(5, false);

            item.OffsetStart(10);

            absolute.Res(item.Start).Restrictions.Restrict(15, Direction.Left);
            absolute.Res(item.Finish).Restrictions.Restrict(30, Direction.Left);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
