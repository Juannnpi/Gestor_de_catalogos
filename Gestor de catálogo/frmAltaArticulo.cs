using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestor_de_catálogo
{
    public partial class frmAltaArticulo : Form
    {
        Articulo articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
            txtPrecio.Text = "00,00";
        }
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.Text = "Modificar";
            lblTitulo.Text = "Modificar artículo";
            this.articulo = articulo;
        }
        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboMarca.SelectedIndex = -1;
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboCategoria.SelectedIndex = -1;
                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;                    
                    //txtPrecio.Text = String.Format("{0:C2}", articulo.Precio);                    
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagen.Text = articulo.urlImagen;
                    cargarImagen(txtImagen.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void cboMarca_Leave(object sender, EventArgs e)
        {
            string texto = cboMarca.Text;
            cboMarca.SelectedIndex = cboMarca.FindString(texto);
            if (cboMarca.SelectedIndex == -1 ) 
            {
                cboMarca.Text = null;
            }
        }
        private void cboCategoria_Leave(object sender, EventArgs e)
        {
            string texto = cboCategoria.Text;
            cboCategoria.SelectedIndex = cboCategoria.FindString(texto);
            if (cboCategoria.SelectedIndex == -1)
            {
                cboCategoria.Text = null;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.urlImagen = txtImagen.Text;
                if (txtPrecio.Text != "")
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                if (txtCodigo.Text == "" || txtNombre.Text == "" || cboMarca.SelectedIndex == -1 || cboCategoria.SelectedIndex == -1)
                    MessageBox.Show("Falta cargar datos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txtPrecio.Text == "00,00" || txtPrecio.Text == "")
                {
                    txtPrecio.Select();
                    MessageBox.Show("Falta cargar el precio", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (articulo.Id == 0)
                        negocio.agregar(articulo);
                    else
                        negocio.modificar(articulo);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
        private bool soloNumeros(string texto)
        {
            foreach (char item in texto)
            {
                if (!(char.IsNumber(item)))
                    return false;
            }
            return true;
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            //string precio = txtPrecio.Text;
            //txtPrecio.Text = String.Format("{0:C2}", precio);
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            //txtPrecio.Text = String.Format("{0:C2}", sender);
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }
    }
}
