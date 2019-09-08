using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Görev_Yöneticisi
{
    static class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 9);           //aynı anda kaç thread çalışabilir ayarlandı = 9 tane
        

        static public int i;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            



            Trigger scheduler = new Trigger();
            // scheduler.runJob();
        }
    }
}
