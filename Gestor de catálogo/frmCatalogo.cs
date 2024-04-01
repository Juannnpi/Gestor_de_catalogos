using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;

namespace Gestor_de_catálogo
{
    public partial class frmCatalogo : Form
    {
        private List<Articulo> listaArticulos;
        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void frmCatalogo_Load(object sender, EventArgs e)
        {
            cargar();
        }
        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccion = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImagen(seleccion.urlImagen);
            }
        }
        private void cargar()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            listaArticulos = negocio.listar();
            dgvCatalogo.DataSource = listaArticulos;
            ocultarColumnasyFormato();
        }
        private void ocultarColumnasyFormato()
        {
            dgvCatalogo.Columns["Id"].Visible = false;
            dgvCatalogo.Columns["urlImagen"].Visible = false;
            dgvCatalogo.Columns["Precio"].DefaultCellStyle.Format = "C";           
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {

                pbxImagen.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }
        }

    }
}
