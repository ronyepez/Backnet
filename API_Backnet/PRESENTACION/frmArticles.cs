using ENTIDAD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using PagedList;
using System.Linq;

namespace PRESENTACION
{
    public partial class frmArticles : Form
    {
        private string idArticles = null;
        private bool Editar = false;
        private bool Eliminar = false;
        private string url = "https://localhost:44381/";

        validationTB vTB = new validationTB();
        public frmArticles()
        {
            InitializeComponent();
        }

        private void frmArticles_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            listArticles();

            iniciarControles();
        }

        private async void listArticles()
        {
            string respuesta = await GetHttp();
            List<ENTIDAD.getArticles> lst =
                JsonConvert.DeserializeObject<List<ENTIDAD.getArticles>>(respuesta);
            dgvArticles.DataSource = lst;

            iniciarControles();
        }

        private async Task<string> GetHttp()
        {
            WebRequest oRequest = WebRequest.Create(url + "services/articles");
            WebResponse oResponse = oRequest.GetResponse();
            StreamReader sr = new StreamReader(oResponse.GetResponseStream());
            return await sr.ReadToEndAsync();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //IMPLEMENTACION DEL METODO PUT    
            if (Editar == true)
            {
                DialogResult result = MessageBox.Show("¿Seguro que desea Modificar el registro seleccionado?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string urlEdit = url + "services/updArticles";

                    try
                    {
                        ArticleRequest oArticles = new ArticleRequest();
                        oArticles.id = int.Parse(txtId.Text);
                        oArticles.name = txtName.Text;
                        oArticles.description = txtDescription.Text;
                        oArticles.price = double.Parse(txtPrice.Text);
                        oArticles.total_in_shelf = int.Parse(txtTotalShelf.Text);
                        oArticles.total_in_vault = int.Parse(txtTotalVault.Text);
                        oArticles.store_id = int.Parse(txtCodStories.Text);

                        string resultado = Send<ArticleRequest>(urlEdit, oArticles, "PUT");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }

                listArticles();

                Editar = false;
                btnAdd.Enabled = true;
                btnAdd.Show();
                btnDelete.Enabled = false;
                btnGuardar.Hide();
                btnRefresh.Enabled = true;
                btnUpdate.Enabled = true;
                btnSearch.Enabled = true;
                txtName.Enabled = true;
                txtDescription.Enabled = true;
                txtPrice.Enabled = true;
                txtTotalShelf.Enabled = true;
                txtTotalVault.Enabled = true;
                txtCodStories.Enabled = true;
                iniciarControles();
                reiniciarControles();
            }
            else if (Eliminar == true)
            {
                DialogResult result = MessageBox.Show("¿Seguro que desea eliminar el registro seleccionado?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //IMPLEMENTACION DEL METODO DELETE    
                    string urlDelete = url + "services/delArticles?id=" + txtId.Text;

                    try
                    {
                        ArticleRequest oArticles = new ArticleRequest();
                        oArticles.id = int.Parse(txtId.Text);

                        string resultado = Send<ArticleRequest>(urlDelete, oArticles, "DELETE");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }

                listArticles();

                Eliminar = false;
                btnAdd.Enabled = true;
                btnAdd.Show();
                btnDelete.Enabled = true;
                btnGuardar.Hide();
                btnRefresh.Enabled = true;
                btnUpdate.Enabled = false;
                btnSearch.Enabled = true;
                txtName.Enabled = false;
                txtDescription.Enabled = false;
                txtPrice.Enabled = false;
                txtTotalShelf.Enabled = false;
                txtTotalVault.Enabled = false;
                txtCodStories.Enabled = false;
                iniciarControles();
                reiniciarControles();
            }
            else
            {
                if (txtName.Text == "" || txtDescription.Text == ""
                    || txtPrice.Text == "" || txtTotalShelf.Text == "" 
                    || txtTotalVault.Text == "" || txtCodStories.Text == "")
                {
                    epValidation.SetError(txtName, "Los campos no debe estar vacio");
                    epValidation.SetError(txtDescription, "Los campos no debe estar vacio");
                    epValidation.SetError(txtPrice, "Los campos no debe estar vacio");
                    epValidation.SetError(txtTotalShelf, "Los campos no debe estar vacio");
                    epValidation.SetError(txtTotalVault, "Los campos no debe estar vacio");
                    epValidation.SetError(txtCodStories, "Los campos no debe estar vacio");
                }
                else
                {
                    borrarMensajeError();
                    //IMPLEMENTACION DEL METODO POST
                    string urlAdd = url + "services/addArticles";

                    try
                    {
                        ArticleRequest oArticles = new ArticleRequest();
                        oArticles.name = txtName.Text;
                        oArticles.description = txtDescription.Text;
                        oArticles.price = double.Parse(txtPrice.Text);
                        oArticles.total_in_shelf = int.Parse(txtTotalShelf.Text);
                        oArticles.total_in_vault = int.Parse(txtTotalVault.Text);
                        oArticles.store_id = int.Parse(txtCodStories.Text);

                        string resultado = Send<ArticleRequest>(urlAdd, oArticles, "POST");
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
                listArticles();
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
            listArticles();
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
            txtName.Enabled = true;
            txtDescription.Enabled = true;
            txtPrice.Enabled = true;
            txtTotalShelf.Enabled = true;
            txtTotalVault.Enabled = true;
            txtCodStories.Enabled = true;
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
            txtDescription.Enabled = true;
            txtPrice.Enabled = true;
            txtTotalShelf.Enabled = true;
            txtTotalVault.Enabled = true;
            txtCodStories.Enabled = true;
            txtName.Focus();

            if (dgvArticles.SelectedRows.Count > 0)
            {
                Editar = true;
                txtId.Text = dgvArticles.CurrentRow.Cells["id"].Value.ToString();
                txtName.Text = dgvArticles.CurrentRow.Cells["name"].Value.ToString();
                txtDescription.Text = dgvArticles.CurrentRow.Cells["description"].Value.ToString();
                txtPrice.Text = dgvArticles.CurrentRow.Cells["price"].Value.ToString();
                txtTotalShelf.Text = dgvArticles.CurrentRow.Cells["total_in_shelf"].Value.ToString();
                txtTotalVault.Text = dgvArticles.CurrentRow.Cells["total_in_vault"].Value.ToString();
                txtCodStories.Text = dgvArticles.CurrentRow.Cells["store_id"].Value.ToString();
                idArticles = dgvArticles.CurrentRow.Cells["id"].Value.ToString();
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
            txtDescription.Enabled = false;
            txtPrice.Enabled = false;
            txtTotalShelf.Enabled = false;
            txtTotalVault.Enabled = false;
            txtCodStories.Enabled = false;
            txtName.Focus();

            if (dgvArticles.SelectedRows.Count > 0)
            {
                Eliminar = true;
                txtId.Text = dgvArticles.CurrentRow.Cells["id"].Value.ToString();
                txtName.Text = dgvArticles.CurrentRow.Cells["name"].Value.ToString();
                txtDescription.Text = dgvArticles.CurrentRow.Cells["description"].Value.ToString();
                txtPrice.Text = dgvArticles.CurrentRow.Cells["price"].Value.ToString();
                txtTotalShelf.Text = dgvArticles.CurrentRow.Cells["total_in_shelf"].Value.ToString();
                txtTotalVault.Text = dgvArticles.CurrentRow.Cells["total_in_vault"].Value.ToString();
                txtCodStories.Text = dgvArticles.CurrentRow.Cells["store_id"].Value.ToString();
                idArticles = dgvArticles.CurrentRow.Cells["id"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila!!");
                iniciarControles();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void reiniciarControles()
        {
            txtId.Clear();
            txtName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            txtTotalShelf.Clear();
            txtTotalVault.Clear();
            txtCodStories.Clear();
            txtName.Focus();
        }

        private void iniciarControles()
        {
            btnAdd.Enabled = true;
            btnAdd.Show();
            btnDelete.Enabled = true;
            btnGuardar.Hide();
            btnUpdate.Enabled = true;
            btnSearch.Enabled = true;
            txtName.Enabled = false;
            txtDescription.Enabled = false;
            txtPrice.Enabled = false;
            txtTotalShelf.Enabled = false;
            txtTotalVault.Enabled = false;
            txtCodStories.Enabled = false;
        }

        public void borrarMensajeError()
        {
            epValidation.SetError(txtName, "");
            epValidation.SetError(txtDescription, "");
            epValidation.SetError(txtPrice, "");
            epValidation.SetError(txtTotalShelf, "");
            epValidation.SetError(txtTotalVault, "");
            epValidation.SetError(txtCodStories, "");
        }

        private void txtPrice_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            float num;
            if (!float.TryParse(txtPrice.Text, out num))
            {
                epValidation.SetError(txtPrice, "Debe ingresar el formato correcto 123.00!!!");
            }
            else
            {
                epValidation.SetError(txtPrice, "");
            }
        }

        private void txtId_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num;
            if (!int.TryParse(txtId.Text,out num))
            {
                epValidation.SetError(txtId, "Solo debe ingresar números!!!");
            }
            else
            {
                epValidation.SetError(txtId, "");
            }
        }

        private void txtTotalShelf_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num;
            if (!int.TryParse(txtTotalShelf.Text, out num))
            {
                epValidation.SetError(txtTotalShelf, "Solo debe ingresar números!!!");
            }
            else
            {
                epValidation.SetError(txtTotalShelf, "");
            }
        }

        private void txtTotalVault_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num;
            if (!int.TryParse(txtTotalVault.Text, out num))
            {
                epValidation.SetError(txtTotalVault, "Solo debe ingresar números!!!");
            }
            else
            {
                epValidation.SetError(txtTotalVault, "");
            }
        }

        private void txtCodStories_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num;
            if (!int.TryParse(txtCodStories.Text, out num))
            {
                epValidation.SetError(txtCodStories, "Solo debe ingresar números!!!");
            }
            else
            {
                epValidation.SetError(txtCodStories, "");
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            vTB.soloLetras(e);
        }
    }
}
