using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Objects;

namespace TelChina.TRF.Debug.ObjectStateVisualizer
{
    public partial class EntityTracer : Form
    {
        public EntityTracer()
        {
            InitializeComponent();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count == 0)
            {
                this.btnDetail.Enabled = false;
            }
            else
            {
                this.btnDetail.Enabled = true;
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            dynamic entity = this.dataGridView1.SelectedRows[0].DataBoundItem;
            var ctx = this.dataGridView1.Tag as ObjectContext;
            if (ctx != null && entity != null)
            {
                if (entity.EntityState == "Deleted")
                {
                    MessageBox.Show("删除状态的实体不能追踪原始信息");
                    return;
                }
                object EntityInfo = entity.EntityInfo as Object;
                Visualizer.VisualizeEntityState(ctx, EntityInfo);
            }
        }
    }
}
