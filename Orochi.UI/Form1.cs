using dnlib.DotNet;
using Orochi.Protections.Mutations;
using Orochi.Protections;
using System;
using System.Windows.Forms;
using Orochi.Core;
using System.Collections.Generic;
using System.Text;
using Orochi.Core.Interfaces;
using dnlib.DotNet.Emit;

namespace Orochi.UI
{
    public partial class Form1 : Form
    {
        ModuleDefMD module;
        public Form1()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                module = ModuleDefMD.Load(files[0]);
                Console.WriteLine($"Module loaded {module.Name}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region amk
            /*
            #region renamer
            if (listBox1.Items.Contains(AddSpacesToSentence("Renamer", false)))
            {
                #region renaming
                ClassesRenaming classesRenaming = new ClassesRenaming();
                FieldsRenaming fieldsRenaming = new FieldsRenaming();
                MethodsRenaming methodsRenaming = new MethodsRenaming();
                NamespacesRenaming namespacesRenaming = new NamespacesRenaming();
                PropertiesRenaming propertiesRenaming = new PropertiesRenaming();
                classesRenaming.Rename(module);
                fieldsRenaming.Rename(module);
                methodsRenaming.Rename(module);
                namespacesRenaming.Rename(module);
                propertiesRenaming.Rename(module);
                #endregion
            }
            #endregion
            #region mutations
            if (listBox1.Items.Contains(AddSpacesToSentence("MutationsProtection", false)))
            {
                #region Mutations
                foreach (TypeDef type in module.Types)
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        new MutationProtection().Start(method, module, 20);
                    }
                }
                #endregion
            }
            #endregion
            #region string encrypting
            if (listBox1.Items.Contains(AddSpacesToSentence("StringEncryption", false)))
            {
                #region string encryption
                new StringEncryption().Start();
                #endregion
            }
            #endregion
            */
            #endregion
            OrochiEngine o = new OrochiEngine(module);
            foreach (string somthng in listBox1.Items)
                o.ProtectionsTask.Add(somthng.ToString());
            o.ApplyChangesAndWrite();
            module = null;
            Console.WriteLine("Module is unloaded. Please re-load it.");
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
          
        }
        List<TypeDef> protections = new List<TypeDef>();
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(var item in ProtectionLister.listTypes())
            {
                comboBox1.Items.Add(item.Name);
                protections.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!listBox1.Items.Contains(comboBox1.Text))
               listBox1.Items.Add(comboBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
