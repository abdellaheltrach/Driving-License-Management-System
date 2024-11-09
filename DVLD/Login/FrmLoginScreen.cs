using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Login
{
    public partial class FrmLoginScreen : Form
    {
        private clsUser _userService;

        public FrmLoginScreen()
        {
            InitializeComponent();

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
                    // Successful login
                    MessageBox.Show("Login successful.", "Login successful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // User is inactive

                }
            }
            else
            {
                // Invalid credentials
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
