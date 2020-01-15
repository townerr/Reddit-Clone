using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assign4
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private Form1 f1 = null;

        public LoginForm(Form form1)
        {
            f1 = form1 as Form1;
            InitializeComponent();
        }
        private void loginFormButton_Click(object sender, EventArgs e)
        {
            string username = usernameText.Text;
            string password = passwordText.Text;
            bool found = false;

            //loop through users checking if username is found
            foreach(User u in Program.users)
            {
                //if username is valid attempt to login with password
                if(username == u.Name)
                {
                    found = true;
                    if (Program.Login(u, password) == true)
                    {
                        MessageBox.Show("Login Successfull!");
                        f1.LoginSuccess(u);
                        Program.loggedInUserID = Convert.ToInt32(u.ID);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password.");
                    }

                    break;
                }
                else
                {
                    found = false;
                }
            }
            //show error if username not found
            if (found == false)
            {
                MessageBox.Show("Invalid Username.");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
