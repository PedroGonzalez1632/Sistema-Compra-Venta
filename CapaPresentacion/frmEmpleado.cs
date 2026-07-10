using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmEmpleado : Form
    {
        public frmEmpleado()
        {
            InitializeComponent();
        }

        private void frmEmpleado_Load(object sender, EventArgs e)
        {
            cboestado.Items.Add(new OpcionCombo { Valor = 1, Texto = "Activo" });
            cboestado.Items.Add(new OpcionCombo { Valor = 0, Texto = "No Activo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;


            List<Rol> listaRol = new CN_Rol().Listar();

            foreach (Rol item in listaRol) {
                cborol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.Descripcion });
            }

            cborol.DisplayMember = "Texto";
            cborol.ValueMember = "Valor";
            cborol.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvdata.Columns) {
                if (columna.Visible == true && columna.Name != btnseleccionar.Name) {
                    cbobuscar.Items.Add(new OpcionCombo { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobuscar.DisplayMember = "Texto";
            cbobuscar.ValueMember = "Valor";
            cbobuscar.SelectedIndex = 0;


            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario) {
                dgvdata.Rows.Add(new object[] { "", item.IdUsuario, item.NombreCompleto, item.Documento,  item.Correo, item.Telefono, item.Clave,
                item.oRol.IdRol, item.oRol.Descripcion, 
                item.Estado == true ? 1 : 0,
                item.Estado == true ? "Activo" : "No Activo"

                });
            }

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            dgvdata.Rows.Add(new object[]{"", txtid.Text, txtdocumento.Text, txtnombre.Text, txtcorreo.Text,
                txttelefono.Text, txtcontrasenia.Text, textBox6.Text,
                ((OpcionCombo)cborol.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cborol.SelectedItem).Texto.ToString(),
                ((OpcionCombo)cboestado.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cboestado.SelectedItem).Texto.ToString(),
            });
            limpiar();
        }

        private void limpiar() {
            txtid.Text = "0";
            txtdocumento.Text = "";
            txtnombre.Text = "";
            txtcorreo.Text = "";
            txttelefono.Text = "";
            txtcontrasenia.Text = "";
            textBox6.Text = "";
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.checkmini.Width / 4;
                var h = Properties.Resources.checkmini.Height / 4;

                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.checkmini, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }





    }
}
