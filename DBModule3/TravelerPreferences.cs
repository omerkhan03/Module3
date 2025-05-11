using System;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TravelerPreferences : Form
    {
        public TravelerPreferences()
        {
            InitializeComponent();
            // Add some common travel preferences to start with
            preferencesChecklist.Items.AddRange(new string[] { 
                "Adventure Travel",
                "Cultural Experience",
                "Beach Vacation",
                "Mountain Hiking",
                "City Tours",
                "Historical Sites",
                "Food & Cuisine",
                "Luxury Travel",
                "Budget Travel",
                "Eco Tourism",
                "Photography Tours",
                "Shopping Trips",
                "Religious Tourism",
                "Sports & Recreation",
                "Wellness & Spa"
            });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }

        private void TravelerPreferences_Load(object sender, EventArgs e)
        {

        }

        private void preferencesChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void registerbutton_Click(object sender, EventArgs e)
        {
            if (preferencesChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one preference.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Save preferences to database
            MessageBox.Show("Preferences saved successfully!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //TravelerDashboard travelerDashboard = new TravelerDashboard();
            //travelerDashboard.Show();
            this.Close();

        }

        private void sataButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(customPreferenceTextBox.Text))
            {
                preferencesChecklist.Items.Add(customPreferenceTextBox.Text);
                customPreferenceTextBox.Clear();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}