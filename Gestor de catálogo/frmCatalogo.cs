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
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Precio");
            cboCampo.SelectedIndex = 0;
        }
        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                ptgDetalles.SelectedObject = null;
                pbxImagen.Image = null;
                habilitarBotones();
            }
            else
                deshabilitarBotones();
        }
        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;
            if (filtro.Length > 2)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
                listaFiltrada = listaArticulos;
            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnasyFormato();
        }
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = cboCampo.SelectedItem.ToString();
            if (campo == "Nombre")
            {
                cboCriterio.DataSource = null;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.SelectedIndex = 0;
            }
            else if (campo == "Marca")
            {
                try
                {
                MarcaNegocio negocio = new MarcaNegocio();
                cboCriterio.DataSource = null;
                cboCriterio.DataSource = negocio.listar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (campo == "Categoría")
            {
                CategoriaNegocio negocio = new CategoriaNegocio();
                cboCriterio.DataSource = null;
                cboCriterio.DataSource = negocio.listar();
            }
            else if (campo == "Precio")
            {
                cboCriterio.DataSource = null;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");                
                cboCriterio.Items.Add("Menor a");
                cboCriterio.SelectedIndex = 0;
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string campo = cboCampo.Text;
            string criterio = cboCriterio.Text;
            string filtro = txtFiltroAv.Text;
            ArticulosNegocio negocio = new ArticulosNegocio();
            if (filtro == "" && campo != "Marca" && campo != "Categoría")
            {
                dgvCatalogo.DataSource = listaArticulos;
                MessageBox.Show("Falta completar el filtro", "Filtro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (campo == "Precio")
                    if (!soloNumeros(filtro))
                    {
                        MessageBox.Show("Completar solo con números", "Filtro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);
            }                
        }
        private void btnEliminarL_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }
        private void btnEliminarF_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void btnDetalle_Click(object sender, EventArgs e)
        {            
            if (dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccion = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                ptgDetalles.SelectedObject = seleccion;                
                cargarImagen(seleccion.urlImagen);
            }
            else
            {
                MessageBox.Show("Selecione un artículo", "Detalles", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dgvCatalogo.Columns["Codigo"].Visible = false;
            dgvCatalogo.Columns["Descripcion"].Visible = false;
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
        private void habilitarBotones()
        {
            btnModificar.Enabled = true;
            btnEliminarF.Enabled = true;
            btnEliminarL.Enabled = true;
            btnDetalle.Enabled = true;
        }
        private void deshabilitarBotones()
        {
            btnModificar.Enabled = false;
            btnEliminarF.Enabled = false;
            btnEliminarL.Enabled = false;
            btnDetalle.Enabled = false;
        }
        private bool soloNumeros(string texto)
        {
            foreach (char item in texto)
            {
                if (!(char.IsNumber(item)))
                    return false;
            }
            return true;
        }
        private void eliminar(bool logico = false)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            Articulo seleccion;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar el artículo?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    if (dgvCatalogo.CurrentRow == null)
                    {
                        MessageBox.Show("Primero seleccione un artículo.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    seleccion = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;                        
                    if (logico)
                        negocio.eliminarLogico(seleccion.Id);
                    else
                        negocio.eliminarFisico(seleccion.Id);
                }
                cargar();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccion = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                frmAltaArticulo modificar = new frmAltaArticulo(seleccion);
                modificar.ShowDialog();
                cargar();
            }
            else
            {
                MessageBox.Show("Seleccione el artículo a modificar", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {

        }
    }
}
