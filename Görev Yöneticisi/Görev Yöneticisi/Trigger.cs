using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Görev_Yöneticisi
{
    class Trigger
    {
        public async static void runJob()
        {
            try
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();    
                IScheduler sched = await schedFact.GetScheduler();                                   //Yeni bir zamanlayıcı oluşturulup çalıştırılıyor
                if (!sched.IsStarted) 
                    await sched.Start();

                IJobDetail job = JobBuilder.Create<Quartz.IJob>().WithIdentity("Job", null).Build(); //Oluşturduğumuz görev(Job) hazırlanıyor

                ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder                              //10 sn de bir exe ler çalışır
                    .Create().WithIdentity("Job")
                    .StartAt(DateTime.UtcNow)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10))
                    .Build();
                await sched.ScheduleJob(job, trigger);                                               //Görev tetikleyici ile zamanlanıyor

                ManualResetEvent resetEvent = new ManualResetEvent(false);                           //uygulama bekletiliyor
                resetEvent.WaitOne();
            }
            catch
            {
                
            }
        }
    }
}
