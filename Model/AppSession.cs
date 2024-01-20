
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Enigma_Client_V2.Model
{
    public static class AppSession
    {
        public static User current_user { get; set; }
        public static Session current_session { get; set; }
        public static List<EProgramm> eProgramms { get; set; }
        public static List<SessionTariff> appTariffs { get; set; }
        private static CompClub_dbContext context;
        public static CompClub_dbContext Context
        {
            get
            {
                if (context is null)
                {
                    context = new CompClub_dbContext();
                }
                return context;
            }

        }
    }
    public class SessionTariff : Tariff, INotifyPropertyChanged
    {
        private float sum_price;
        public float Sum_price
        {
            get { return sum_price; }
            set
            {
                sum_price = value;
                OnPropertyChanged("Sum_price");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
