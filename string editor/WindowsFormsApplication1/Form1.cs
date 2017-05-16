using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Mono.Cecil;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        AssemblyDefinition x1;
        string sd;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var se = new SaveFileDialog();
                if (se.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    sd = se.FileName;
                    Thread x = new Thread(s);
                    x.IsBackground = true;
                    x.Start();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void s(object obj)
        {
            x1.Write(sd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            var o = new OpenFileDialog();
            if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = o.FileName;
                Thread x = new Thread(s1);
                x.IsBackground = true;
                x.Start();
            }
        }

        private void s1(object obj)
        {
            try
            {
                x1 = AssemblyDefinition.ReadAssembly(textBox1.Text);
                foreach (ModuleDefinition mod in x1.Modules)
                {
                    foreach (TypeDefinition td in mod.Types)
                    {
                        IterateType(td);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void IterateType(TypeDefinition td)
        {
            foreach (MethodDefinition md in td.Methods)
            {
                if (md.HasBody)
                {
                    for (int i = 0; i < md.Body.Instructions.Count - 1; i++)
                    {
                        Instruction inst = md.Body.Instructions[i];
                        if (inst.OpCode == OpCodes.Ldstr)
                        {
                            listView1.Items.Add(inst.Operand.ToString());
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            StringHelper.ReplaceString(textBox2.Text, textBox3.Text, x1);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            textBox2.Text = listView1.SelectedItems[0].Text;
        }
    }
}
