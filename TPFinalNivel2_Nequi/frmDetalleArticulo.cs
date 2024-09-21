using Dominio;
using System;
using System.Windows.Forms;

namespace TPFinalNivel2_Nequi
{
    public partial class frmMostrarDetalleArticulo : Form
    {
        private Articulo articuloSeleccionado = null;

        public frmMostrarDetalleArticulo(Articulo articuloSeleccionado)
        {
            InitializeComponent();
            this.articuloSeleccionado = articuloSeleccionado;
        }

        private void frmMostrarDetalleArticulo_Load(object sender, EventArgs e)
        {
            cargarImagen(articuloSeleccionado.ImagenUrl);
            txtCodigo.Text = articuloSeleccionado.Codigo;
            txtNombre.Text = articuloSeleccionado.Nombre;
            txtDescripcion.Text = articuloSeleccionado.Descripcion;
            txtCategoria.Text = articuloSeleccionado.Categoria.ToString();
            txtMarca.Text = articuloSeleccionado.Marca.ToString();
            txtPrecio.Text = articuloSeleccionado.Precio.ToString();
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
    }
}
