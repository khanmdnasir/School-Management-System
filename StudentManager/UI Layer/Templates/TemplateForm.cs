using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManager.UI_Layer.Templates
{
    public partial class TemplateForm : Form
    {
        public TemplateForm()
        {
            InitializeComponent();
        }

        [Category("Custom")]
        public bool IsUpdate { get; set; }

        private void TemplateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
