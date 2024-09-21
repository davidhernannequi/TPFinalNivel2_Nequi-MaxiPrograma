using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;        
using Negocio;

namespace TPFinalNivel2_Nequi
{
    public partial class frmAltaArticulo : Form
    {

        private Articulo articulo = null;
        OpenFileDialog archivo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void frmDetalleArticulo_Load(object sender, EventArgs e)

        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cmbMarca.DataSource = marcaNegocio.listadoMarcas();
                cmbMarca.ValueMember = "Id";
                cmbMarca.DisplayMember = "Descripcion";
                cmbCategoria.DataSource = categoriaNegocio.listadoCategorias();
                cmbCategoria.ValueMember = "Id";
                cmbCategoria.DisplayMember = "Descripcion";
                

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cmbCategoria.SelectedValue = articulo.Categoria.Id;
                    cmbMarca.SelectedValue = articulo.Marca.Id;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                
                if (articulo == null)
                    articulo = new Articulo();
                
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = new Marca();
                articulo.Marca = (Marca)cmbMarca.SelectedItem;
                articulo.Categoria = new Categoria();
                articulo.Categoria = (Categoria)cmbCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.ImagenUrl = txtImagenUrl.Text;

                if (articulo.Id != 0)
                {
                    if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text))
                        MessageBox.Show("Código, Nombre son campos obligatorios");
                    else if (!(soloNumeros(txtPrecio.Text)) || txtPrecio.Text == "")
                        MessageBox.Show("El Precio es obligatorio, solo se aceptan Numeros en este campo");
                    else
                    {
                        negocio.ModificarArticulo(articulo);
                        MessageBox.Show("Articulo Modificado Exitosamente");
                        Close();
                    }
                }
                else
                {
                    negocio.AgregarArticulo(articulo);
                    MessageBox.Show("Articulo Agregado Exitosamente");
                    Close();
                }

                if(archivo != null && !(txtImagenUrl.Text.ToLower().Contains("http")))
                    File.Copy(txtImagenUrl.Text, ConfigurationManager.AppSettings["imgLocal"] + archivo.SafeFileName);
                
                
            }
            catch (FormatException ex)
            {
                if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text))
                    MessageBox.Show("Código, Nombre son campos obligatorios");
                else if (!(soloNumeros(txtPrecio.Text)) || txtPrecio.Text == "")
                    MessageBox.Show("El Precio es obligatorio, solo se aceptan Numeros en este campo");
                else
                    MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pctBoxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pctBoxArticulo.Load("https://i0.wp.com/theperfectroundgolf.com/wp-content/uploads/2022/04/placeholder.png?fit=1200%2C800&ssl=1");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Multiselect = false;
            archivo.Filter = "jpg|*.jpg; |png|*.png";

            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(txtImagenUrl.Text);
            }
        }

        private bool soloNumeros(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }

            return true;
        }

    }
}
