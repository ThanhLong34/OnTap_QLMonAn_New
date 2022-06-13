using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTap_QLMonAn_New
{
    public class NhomMonAn
    {
        public int maNhom { get; set; }
        public string tenNhom { get; set; }

        public NhomMonAn()
        {

        }

        public NhomMonAn(int maNhom, string tenNhom)
        {
            this.maNhom = maNhom;
            this.tenNhom = tenNhom;
        }
    }
}
