using BakerySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BakerySystem.App_Start
{
    public class Helper
    {
        public static int status = 0;
        public static int oldstatus = 0;
        public static void startDeliveryFunction()
        {
            List<BKRY_DELIVERY> bkryList = null;
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.BKRY_DELIVERY.Where(x => x.DeliveryStatus == null || x.DeliveryStatus == oldstatus).ToList<BKRY_DELIVERY>();
                foreach (var item in bkryList)
                {
                    item.DeliveryStatus = status;                   
                }
                db.SaveChanges();
            }
            if (Helper.status == 3)
            {
                oldstatus = -1;
                status = 0;
            }
            else
            {
                oldstatus = status;
                status += 1;
            }
        }
    }
    public class AutoSaver
    {
        private int _save_interval;
        private bool _stop_saving = false;


        public AutoSaver(double interval)
        {
            _save_interval = (int)(interval * 1000);
            _stop_saving = false;
        }

        public async void StartSaving()
        {
            _stop_saving = false;
            await Task.Delay(_save_interval);
            while (!_stop_saving)
            {
                Helper.startDeliveryFunction();
                await Task.Delay(_save_interval);
                
            }
        }

        public void StopSaving()
        {
            _stop_saving = true;
        }
    }
}