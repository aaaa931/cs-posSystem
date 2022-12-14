using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace good
{
    public interface igood
    {
        string name { get; }
        double price { get; }
        double amount { get; }
    }
    public class good
    {
        private static int num_of_id = 0;
        public int _id { get; }
        public double _price { get; }
        public double _amount { get; }
        public double _total { get; }
        public tool.type _type { get; }
        public string _name { get; }
        public string _date { get; }

        public good(string name, tool.type type, double price, double amount)
        {
            num_of_id++;

            this._id = num_of_id;
            this._date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            this._name = name;
            this._price = price;
            this._amount = amount;
            this._type = type;
            this._total = _price * _amount;
        }

        public double getTotal()
        {
            return _total;
        }

        public override string ToString()
        {
            return $"id: {_id}\ndate: {_date}\ntype: {_type}\nname: {_name}\nprice {_price}\namount: {_amount}\ntotal: {_total}";
        }

        public void showLog()
        {
            MessageBox.Show(this.ToString());
        }
        // rows.Add(new object[] {1, date, _type, _good, price, amount, _sum});
    }

    public class good_input : good
    {
        public good_input(string name, double price, double amount) : base(name, tool.type.進貨, price, amount) { }
    }

    public class good_output : good
    {
        public good_output(string name, double price, double amount) : base(name, tool.type.出貨, price, amount) { }
    }

    public class tool
    {
        public enum type { 進貨, 出貨 }
    }
    public class class1
    {
    }
}
