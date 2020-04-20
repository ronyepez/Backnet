using ENTIDAD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PRESENTACION
{
    public partial class frmStores : Form
    {
        private bool search = false;
        private string idStores = null;
        private bool Editar = false;
        private bool Eliminar = false;
        private string url = "https://localhost:44381/";

        validationTB vTB = new validationTB();

        public frmStores()
        {
            InitializeComponent();
        }

        private void frmStores_Load(object sender, EventArgs e)
        {
            txtId.Enabled = true;
            if (search == true)
            {
                refreshGetById();

            }
            else
            {
                listStores();
            }

            iniciarControles();
        }

        private async void refreshGetById(string resultSearch = null)
        {
            string respuesta = await GetHttpByID();
            getStores storeId =
                JsonConvert.DeserializeObject<getStores>(respuesta);

            dgvStores.DataSource = storeId;

            iniciarControles();
        }

        private async Task<string> GetHttpByID()
        {
            WebRequest oRequest = WebRequest.Create(url + "services/stores?id=" + txtId.Text);
            WebResponse oResponse = oRequest.GetResponse();
            StreamReader sr = new StreamReader(oResponse.GetResponseStream());
            return await sr.ReadToEndAsync();
        }

        private async void listStores()
        {
            string respuesta = await GetHttp();
            List<ENTIDAD.getStores> lst =
                JsonConvert.DeserializeObject<List<ENTIDAD.getStores>>(respuesta);
            dgvStores.DataSource = lst;

            iniciarControles();
        }

        private async Task<string> GetHttp()
        {
            WebRequest oRequest = WebRequest.Create(url + "services/stores");
            WebResponse oResponse = oRequest.GetResponse();
            StreamReader sr = new StreamReader(oResponse.GetResponseStream());
            return await sr.ReadToEndAsync();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Editar == true)
            {
                DialogResult result = MessageBox.Show("¿Seguro que desea Modificar el regustro seleccionado?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //IMPLEMENTACION DEL METODO PUT    
                    string urlEdit = url + "services/updStores";

                    try
                    {
                        StoresRequest oStores = new StoresRequest();
                        oStores.id = int.Parse(txtId.Text);
                        oStores.name = txtName.Text;
                        oStores.address = txtAddress.Text;

                        string resultado = Send<StoresRequest>(urlEdit, oStores, "PUT");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }

                listStores();

                Editar = false;
                btnAdd.Enabled = true;
                btnAdd.Show();
                btnDelete.Enabled = false;
                btnGuardar.Hide();
                btnRefresh.Enabled = true;
                btnUpdate.Enabled = true;
                btnSearch.Enabled = true;
                txtAddress.Enabled = true;
                txtName.Enabled = true;
                iniciarControles();
                reiniciarControles();
            }
            else if (Eliminar == true)
            {
                DialogResult result = MessageBox.Show("¿Seguro que desea eliminar el registro seleccionado?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //IMPLEMENTACION DEL METODO DELETE    
                    string urlDelete = url + "services/delStores?id=" + txtId.Text;

                    try
                    {
                        StoresRequest oStores = new StoresRequest();
                        oStores.id = int.Parse(txtId.Text);

                        string resultado = Send<StoresRequest>(urlDelete, oStores, "DELETE");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }

                listStores();

                Eliminar = false;
                btnAdd.Enabled = true;
                btnAdd.Show();
                btnDelete.Enabled = true;
                btnGuardar.Hide();
                btnRefresh.Enabled = true;
                btnUpdate.Enabled = false;
                btnSearch.Enabled = true;
                txtAddress.Enabled = false;
                txtName.Enabled = false;
                iniciarControles();
                reiniciarControles();
            }
            else
            {
                if (txtName.Text == "" || txtAddress.Text == "")
                {
                    epValidation.SetError(txtName, "Los campos no debe estar vacio");
                    epValidation.SetError(txtAddress, "Los campos no debe estar vacio");
                }
                else
                {
                    borrarMensajeError();
                    //IMPLEMENTACION DEL METODO POST
                    string urlAdd = url + "services/addStores";

                    try
                    {
                        StoresRequest oStores = new StoresRequest();
                        oStores.name = txtName.Text;
                        oStores.address = txtAddress.Text;

                        string resultado = Send<StoresRequest>(urlAdd, oStores, "POST");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }
            }
        }

        private string Send<T>(string url, T objectRequest, string method = "POST")
        {
            string result = "";

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();

                //serializamos el objeto
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

                //peticion
                WebRequest request = WebRequest.Create(url);

                //header
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 100000;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                MessageBox.Show("Proceso ejecutado correctamente!!!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                reiniciarControles();
                listStores();

            }
            catch (Exception e)
            {
                result = e.Message;
                MessageBox.Show("Error al ejecutar el proceso!!!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            reiniciarControles();
            iniciarControles();
            listStores();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Hide();
            btnDelete.Enabled = false;
            btnGuardar.Show();
            btnGuardar.Enabled = true;
            btnRefresh.Enabled = true;
            btnUpdate.Enabled = false;
            btnSearch.Enabled = true;
            txtId.Clear();
            txtId.Enabled = false;
            txtAddress.Enabled = true;
            txtName.Enabled = true;
            txtName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnAdd.Hide();
            btnGuardar.Show();
            btnGuardar.Enabled = true;
            btnRefresh.Enabled = true;
            btnDelete.Enabled = false;
            txtName.Enabled = true;
            txtName.Focus();
            txtAddress.Enabled = true;

            if (dgvStores.SelectedRows.Count > 0)
            {
                Editar = true;
                txtId.Text = dgvStores.CurrentRow.Cells["id"].Value.ToString();
                txtName.Text = dgvStores.CurrentRow.Cells["name"].Value.ToString();
                txtAddress.Text = dgvStores.CurrentRow.Cells["address"].Value.ToString();
                idStores = dgvStores.CurrentRow.Cells["id"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila!!");
                iniciarControles();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnAdd.Hide();
            btnGuardar.Show();
            btnGuardar.Enabled = true;
            btnRefresh.Enabled = true;
            btnUpdate.Enabled = false;
            txtName.Enabled = false;
            txtName.Focus();
            txtAddress.Enabled = false;

            if (dgvStores.SelectedRows.Count > 0)
            {
                Eliminar = true;
                txtId.Text = dgvStores.CurrentRow.Cells["id"].Value.ToString();
                txtName.Text = dgvStores.CurrentRow.Cells["name"].Value.ToString();
                txtAddress.Text = dgvStores.CurrentRow.Cells["address"].Value.ToString();
                idStores = dgvStores.CurrentRow.Cells["id"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila!!");
                iniciarControles();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //search = true;
            //if (!txtId.Text.Trim().Equals(""))
            //{
            //    refreshGetById(txtId.Text.Trim());
            //}
        }

        private void reiniciarControles()
        {
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtName.Focus();
        }

        private void iniciarControles()
        {
            btnAdd.Enabled = true;
            btnAdd.Show();
            btnDelete.Enabled = true;
            btnGuardar.Hide();
            btnRefresh.Enabled = true;
            btnUpdate.Enabled = true;
            btnSearch.Enabled = true;
            txtAddress.Enabled = false;
            txtName.Enabled = false;
        }

        public void borrarMensajeError()
        {
            epValidation.SetError(txtName, "");
            epValidation.SetError(txtAddress, "");
        }

        private void txtId_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num;
            if (!int.TryParse(txtId.Text, out num))
            {
                epValidation.SetError(txtId, "Solo debe ingresar números!!!");
            }
            else
            {
                epValidation.SetError(txtId, "");
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            vTB.soloLetras(e);
        }
    }
}
