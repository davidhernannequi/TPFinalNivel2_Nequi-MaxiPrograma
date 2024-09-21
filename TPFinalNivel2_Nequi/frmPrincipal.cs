using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Negocio;
using Dominio;
using System.Diagnostics;

namespace TPFinalNivel2_Nequi
{
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listaArticulos;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void Gestor_Load(object sender, EventArgs e)
        {
            cargar();
            cmbCampo.Items.Add("Nombre");
            cmbCampo.SelectedIndex = 0;
            cmbCampo.Items.Add("Categoria");
            cmbCampo.Items.Add("Marca");
            cmbCampo.Items.Add("Precio");
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            
            if(dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;

                cargarImagen(seleccionado.ImagenUrl);
            }
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulos = negocio.listar();
                dgvArticulo.DataSource = listaArticulos;
                ocultarColumnas();
                pctBoxArticulo.Load(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                cargarImagen("ImagenUrl");
            }
        }
        private void cargarImagen (string imagen)
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

        private void ocultarColumnas ()
        {
            dgvArticulo.Columns["ImagenUrl"].Visible = false;
            dgvArticulo.Columns["Id"].Visible = false;
        }

        private void cmbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cmbCampo.SelectedItem.ToString();
            if (opcion == "Nombre")
            {
                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Contiene:");
                cmbCriterio.Items.Add("Comienza con:");
                cmbCriterio.Items.Add("Termina con:");
                cmbCriterio.SelectedIndex = 0;
            }
            else if (opcion == "Categoria")
            {
                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Contiene:");
                cmbCriterio.Items.Add("Comienza con:");
                cmbCriterio.Items.Add("Termina con:");
                cmbCriterio.SelectedIndex = 0;
            }
            else if (opcion == "Marca")
            {
                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Contiene:");
                cmbCriterio.Items.Add("Comienza con:");
                cmbCriterio.Items.Add("Termina con:");
                cmbCriterio.SelectedIndex = 0;
            }
            else if (opcion == "Precio")
            {

                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Mayor a:");
                cmbCriterio.Items.Add("Menor a:");
                cmbCriterio.Items.Add("Igual a:");
                cmbCriterio.SelectedIndex = 0;
            }

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                string campo = cmbCampo.SelectedItem.ToString();
                string criterio = cmbCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;


                if (!(string.IsNullOrEmpty(filtro)))
                    dgvArticulo.DataSource = negocio.filtrar(campo, criterio, filtro);
                else
                    cargar();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (cmbCampo.SelectedItem.ToString() == "Precio")
            {

                if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo detalleArticulo = new frmAltaArticulo();
            detalleArticulo.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo articuloSeleccionado = new Articulo();
            
            if (dgvArticulo.CurrentRow != null)
            {
                articuloSeleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                frmAltaArticulo modificar = new frmAltaArticulo(articuloSeleccionado);
                modificar.ShowDialog();
                cargar();
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo seleccionado = new Articulo();
                
                DialogResult resultado = MessageBox.Show("¿Queres eliminarlo definitivamente?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                    negocio.eliminarFisico(seleccionado.Id);
                    cargar();
                }
            }
                
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            Articulo articuloDetallado = new Articulo();

            if (dgvArticulo.CurrentRow != null)
            {
                articuloDetallado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                frmMostrarDetalleArticulo articulo = new frmMostrarDetalleArticulo(articuloDetallado);
                articulo.ShowDialog();
                cargar();
                
            }
        }

        
    }
}
