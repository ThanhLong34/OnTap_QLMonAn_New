using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTap_QLMonAn_New
{
    public class QuanLyNhomMonAn
    {
        public List<NhomMonAn> dsNhomMonAn;

        public QuanLyNhomMonAn()
        {
            dsNhomMonAn = new List<NhomMonAn>();
        }

        public void themNhomMonAn(NhomMonAn i)
        {
            dsNhomMonAn.Add(i);
        }

        public NhomMonAn timNhomMonAnTheoMa(int ma)
        {
            return dsNhomMonAn.Find(i => i.maNhom == ma);
        }
    }
}
