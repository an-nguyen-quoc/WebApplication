using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp;

namespace WinApp.Views.Account
{
    public partial class FormEdit : Form
    {
        public FormEdit()
        {
            InitializeComponent();
        }

        public bool CreateState { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            var acc = (Models.Account)this.Tag;
            Boolean CreateState = this.CreateState; 
            this.textBox1.Text = acc.Id;
            this.textBox2.Text = acc.Role.ToString();
            this.textBox3.Text = acc.Name;
            this.textBox4.Text = acc.Email;
            this.textBox5.Text = acc.Password;
            if (CreateState == false)
            {
                this.label6.Visible = false; 
                this.textBox5.Visible = false;
                 
            }
            else
            {
                this.textBox2.Text = "USER";
            }    
            this.Name = acc.Id == null ? "insert" : "update";
            base.OnLoad(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            if (CreateState == true)
            {
                var UserCreate = new Models.UserCreate();
                UserCreate.Id = this.textBox1.Text;
                UserCreate.UserName = this.textBox1.Text;
                UserCreate.Email = this.textBox4.Text;
                UserCreate.Name = this.textBox2.Text;
                UserCreate.Password = this.textBox5.Text;

                UserCreate.ConfirmPassword = this.textBox5.Text;
                var accCtrl = new Controllers.HomeController();
                accCtrl.Signup(UserCreate);

                //var response = accCtrl.PostAsync(WinApp.Controllers.Env.SignupAPI, UserCreate);
            }
            
            //Engine.Execute("account/update", this.Name, this.textBox1.Text, this.textBox2.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
