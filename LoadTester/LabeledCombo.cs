using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LoadTester
{
    public partial class LabeledCombo :UserControl
    {
        public LabeledCombo()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox Combo { get { return comboBox; } }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public Label Label { get { return label; } }

        public event EventHandler SelectedValueChanged;

        private void comboBox_SelectedValueChanged( object sender, System.EventArgs e )
        {
            OnSelectedValueChanged();
        }

        protected virtual void OnSelectedValueChanged()
        {
            var handler = SelectedValueChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
