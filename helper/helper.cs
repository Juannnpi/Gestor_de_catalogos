using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using negocio;
using dominio;

namespace helper
{
    public class helper
    {
        public void ocultarColumnasyFormato()
        {
            dgvCatalogo.Columns["Id"].Visible = false;
            dgvCatalogo.Columns["urlImagen"].Visible = false;
            dgvCatalogo.Columns["Precio"].DefaultCellStyle.Format = "C";
        }
    }
}
