using System;
using System.Windows.Forms;

namespace PRESENTACION
{
    public partial class frmMain : Form
    {
        private Boolean searchStories, searchArticles;

        public frmMain()
        {
            InitializeComponent();
        }

        private void articlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmArticles articles = new frmArticles();
            articles.Show(this);
        }

        private void storiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStores stories = new frmStores();
            stories.Show(this);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
