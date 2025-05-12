using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class Sidebar : UserControl
    {
        // Event for when filters are changed
        public event EventHandler FiltersChanged;

        public Sidebar()
        {
            InitializeComponent();
            
            // Add event handlers to textboxes to detect changes
            MinBudget.TextChanged += Filter_TextChanged;
            MaxBudget.TextChanged += Filter_TextChanged;
            DurationInDays.TextChanged += Filter_TextChanged;
            GroupSize.TextChanged += Filter_TextChanged;
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            // Notify parent that filters have changed
            FiltersChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Sidebar_Load(object sender, EventArgs e)
        {
            // Clear default text values
            MinBudget.Texts = "";
            MaxBudget.Texts = "";
            DurationInDays.Texts = "";
            GroupSize.Texts = "";
        }

        // Properties to expose filter values
        public decimal? MinimumBudget
        {
            get
            {
                if (decimal.TryParse(MinBudget.Texts, out decimal value))
                    return value;
                return null;
            }
        }

        public decimal? MaximumBudget
        {
            get
            {
                if (decimal.TryParse(MaxBudget.Texts, out decimal value))
                    return value;
                return null;
            }
        }

        public int? Duration
        {
            get
            {
                if (int.TryParse(DurationInDays.Texts, out int value))
                    return value;
                return null;
            }
        }

        public int? GroupSizeValue
        {
            get
            {
                if (int.TryParse(GroupSize.Texts, out int value))
                    return value;
                return null;
            }
        }

        // Method to clear all filters
        public void ClearFilters()
        {
            MinBudget.Texts = "";
            MaxBudget.Texts = "";
            DurationInDays.Texts = "";
            GroupSize.Texts = "";
        }
    }
}
