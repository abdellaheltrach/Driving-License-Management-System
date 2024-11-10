using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;
using System.IO;

namespace DVLD.Login
{
    public partial class FrmLoginScreen : Form
    {
        private clsUser _userService;

        public FrmLoginScreen()
        {
            InitializeComponent();
            lblLoginStatus.Visible = false;
            _LoadLoginDetails();

        }

        private void _SaveLoginDetails(string username, string password)
        {
            string filePath = "LoginDetails.txt";
            if (chkRememberMe.Checked)
            {
                
                File.WriteAllText(filePath, $"{username}\n{password}");
            }
            else
            {
                File.WriteAllText(filePath,"");
            }

        }

        private void _LoadLoginDetails()
        {
            string filePath = "LoginDetails.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 2)
                {
                    txtUserName.Text = lines[0];  // Set the saved username
                    txtPassword.Text = lines[1];  // Set the saved password
                    chkRememberMe.Checked = true;
                }
                else 
                {
                    txtUserName.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    chkRememberMe.Checked = false;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            // Get the username and password from the textboxes
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validate the credentials using UserService
            int userId = clsUser.VerifyUserCredentials(username, password);

            if (userId != -1)
            {
                // User exists and credentials are correct, check if user is active
                if (clsUser.IsUserActive(userId))
                {
                    _SaveLoginDetails(username, password);

                    Form1 frm = new Form1();
                    this.Hide(); 
                    frm.ShowDialog();  
                    this.Close();
                }
                else
                {
                    // User is inactive
                    lblLoginStatus.Text = "*User is inactive!";
                    lblLoginStatus.Visible = true;
                }
            }
            else
            {
                // Invalid credentials
                lblLoginStatus.Text = "*Incorrect Username or Password!";
                lblLoginStatus.Visible = true;
            }

        }

    }
}
