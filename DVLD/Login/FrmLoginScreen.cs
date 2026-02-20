using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;
using System.IO;
using DVLD.Users;
using Microsoft.Win32;




namespace DVLD.Login
{
    public partial class FrmLoginScreen : Form
    {

        public FrmLoginScreen()
        {
            InitializeComponent();
            lblLoginStatus.Visible = false;
            _LoadLoginDetails();

        }

        private void _SaveLoginDetails(string username, string password)
        {
            const string registryPath = @"SOFTWARE\DVLD_PROJECT\Credentials";

            if (chkRememberMe.Checked)
            {
                // Save login details in the Windows registry
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
                if (key != null)
                {
                    key.SetValue("Username", username);
                    key.SetValue("Password", password);
                    key.Close();
                }
            }
            else
            {
                // Clear the registry values
                RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);
                if (key != null)
                {
                    key.DeleteValue("Username", false);
                    key.DeleteValue("Password", false);
                    key.Close();
                }
            }
        }

        private void _LoadLoginDetails()
        {
            const string registryPath = @"SOFTWARE\DVLD_PROJECT\Credentials";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key != null)
            {
                string username = key.GetValue("Username", string.Empty) as string;
                string password = key.GetValue("Password", string.Empty) as string;

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    txtUserName.Text = username;
                    txtPassword.Text = password;
                    chkRememberMe.Checked = true;
                }
                else
                {
                    txtUserName.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    chkRememberMe.Checked = false;
                }

                key.Close();
            }
            else
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                chkRememberMe.Checked = false;
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

                    clsCurrentUser.CurrentUser= clsUser.FindUserById(userId); // initials the currant user
                   
                    this.Hide(); // hiding the login form


                    using (Form1 frm = new Form1())
                    {
                        frm.ShowDialog();
                    }


                    clsCurrentUser.CurrentUser = null; ; // reset the currant user
                    this.Show();
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
