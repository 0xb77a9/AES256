using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace AES256
{
    public partial class MainForm : Form
    {
        private Random rnd = new Random();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport( "user32.dll" )]
        public static extern int SendMessage ( IntPtr hWnd, int Msg, int wParam, int lParam );
        [System.Runtime.InteropServices.DllImport( "user32.dll" )]
        public static extern bool ReleaseCapture ();
        [DllImport( "Gdi32.dll", EntryPoint = "CreateRoundRectRgn" )]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );
        private AES_256 AES = new AES_256();
        public MainForm ()
        {
            InitializeComponent();
        }

        public static string GeneratePassword ( int Length, int NonAlphaNumericChars )
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            string allowedNonAlphaNum = "!@#$%^&*()_-+=[{]};:<>|./?";
            Random rd = new Random();

            if ( NonAlphaNumericChars > Length || Length <= 0 || NonAlphaNumericChars < 0 )
                throw new ArgumentOutOfRangeException();

            char[] pass = new char[Length];
            int[] pos = new int[Length];
            int i = 0, j = 0, temp = 0;
            bool flag = false;

            while ( i < Length - 1 )
            {
                j = 0;
                flag = false;
                temp = rd.Next( 0, Length );
                for ( j = 0; j < Length; j++ )
                    if ( temp == pos[j] )
                    {
                        flag = true;
                        j = Length;
                    }

                if ( !flag )
                {
                    pos[i] = temp;
                    i++;
                }
            }

            for ( i = 0; i < Length - NonAlphaNumericChars; i++ )
                pass[i] = allowedChars[rd.Next( 0, allowedChars.Length )];

            for ( i = Length - NonAlphaNumericChars; i < Length; i++ )
                pass[i] = allowedNonAlphaNum[rd.Next( 0, allowedNonAlphaNum.Length )];

            char[] sorted = new char[Length];
            for ( i = 0; i < Length; i++ )
                sorted[i] = pass[pos[i]];

            string Pass = new String( sorted );

            return Pass;
        }
        private void button1_Click ( object sender, EventArgs e )
        {
            textBox2.Text = GeneratePassword( rnd.Next( 32, 48 ), rnd.Next( 5, 10 ) );
        }
        private void button6_Click ( object sender, EventArgs e )
        {
            textBox8.Text = GeneratePassword( rnd.Next( 32, 48 ), rnd.Next( 5, 10 ) );
        }
        private void panel1_Paint ( object sender, PaintEventArgs e )
        {
            panel1.Region = System.Drawing.Region.FromHrgn( CreateRoundRectRgn( 0, 0, panel1.Width, panel1.Height, 20, 20 ) );
        }

        private void panel2_Paint ( object sender, PaintEventArgs e )
        {
            panel2.Region = System.Drawing.Region.FromHrgn( CreateRoundRectRgn( 0, 0, panel2.Width, panel2.Height, 20, 20 ) );
        }

        private void panel3_Paint ( object sender, PaintEventArgs e )
        {
            panel3.Region = System.Drawing.Region.FromHrgn( CreateRoundRectRgn( 0, 0, panel3.Width, panel3.Height, 20, 20 ) );
        }
        private void panel4_Paint ( object sender, PaintEventArgs e )
        {
            panel4.Region = System.Drawing.Region.FromHrgn( CreateRoundRectRgn( 0, 0, panel4.Width, panel4.Height, 20, 20 ) );
        }

        private void button5_Click ( object sender, EventArgs e )
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if ( openFileDialog.ShowDialog() == DialogResult.OK )
            {
                textBox9.Text = openFileDialog.FileName;
            }
        }
        private void button8_Click ( object sender, EventArgs e )
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if ( openFileDialog.ShowDialog() == DialogResult.OK )
            {
                textBox10.Text = openFileDialog.FileName;
            }
        }
        private void button4_Click ( object sender, EventArgs e )
        {
            try
            {
                if ( textBox8.Text.Length <= 31 )
                {
                    MessageBox.Show( "Your password must be 32 Lengh or more" );
                }
                else
                {
                    var TempEncrypted = AES.Encrypt( File.ReadAllText( textBox9.Text, Encoding.Default ), textBox8.Text );
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Path.GetFullPath( label13.Text );
                    saveFileDialog1.Title = "Save encrypted file";
                    saveFileDialog1.CheckPathExists = true;
                    saveFileDialog1.DefaultExt = Path.GetExtension( label13.Text );
                    saveFileDialog1.Filter = Path.GetExtension( textBox9.Text.Replace( ".", "" ) ) + " files (*" + Path.GetExtension( textBox9.Text ) + ")|*" + Path.GetExtension( textBox9.Text ) + "";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = Path.GetFileName( textBox9.Text.Replace( Path.GetExtension( textBox9.Text ), "" ) ) + "_BETA_Encryption" + Path.GetExtension( textBox9.Text );
                    if ( saveFileDialog1.ShowDialog() == DialogResult.OK )
                    {
                        File.WriteAllText( saveFileDialog1.FileName, TempEncrypted, Encoding.Default );
                        textBox10.Text = saveFileDialog1.FileName;
                        textBox7.Text = textBox8.Text;
                    }
                }
            }
            catch
            {
                MessageBox.Show( "An error occurred" );
            }
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            try
            {
                if ( textBox2.Text.Length <= 31 )
                {
                    MessageBox.Show( "Your password must be 32 Lengh or more" );
                }
                else
                {
                    textBox3.Text = AES.Encrypt( textBox1.Text, textBox2.Text );

                    textBox6.Text = textBox3.Text;
                    textBox5.Text = textBox2.Text;
                }
            }
            catch
            {
                MessageBox.Show( "An error occurred" );
            }
        }

        private void button3_Click ( object sender, EventArgs e )
        {
            try
            {
                if ( textBox5.Text.Length <= 31 )
                {
                    MessageBox.Show( "Your password must be 32 Lengh or more" );
                }
                else
                {
                    string Decrypted = AES.Decrypt( textBox6.Text, textBox5.Text );
                    if(Decrypted.Length <= 0 )
                    {
                        MessageBox.Show( "An error occurred" );
                        textBox4.Text = "";
                        return;
                    }
                    textBox4.Text = AES.Decrypt( textBox6.Text, textBox5.Text );
                }
            }
            catch
            {
                MessageBox.Show( "An error occurred" );
            }
        }

        private void button9_Click ( object sender, EventArgs e )
        {
            try
            {
                if ( textBox7.Text.Length <= 31 )
                {
                    MessageBox.Show( "Your password must be 32 Lengh or more" );
                }
                else
                {
                    var shit = File.ReadAllText( textBox10.Text, Encoding.Default );
                    string shit2 = AES.Decrypt( shit, textBox7.Text );
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Path.GetFullPath( textBox10.Text );
                    saveFileDialog1.Title = "Save decrypted file";
                    saveFileDialog1.CheckPathExists = true;
                    saveFileDialog1.DefaultExt = Path.GetExtension( textBox10.Text );
                    saveFileDialog1.Filter = Path.GetExtension( textBox10.Text.Replace( ".", "" ) ) + " files (*" + Path.GetExtension( textBox10.Text ) + ")|*" + Path.GetExtension( textBox10.Text ) + "";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = Path.GetFileName(textBox10.Text.Replace( "_BETA_Encryption", "" ));
                    if ( saveFileDialog1.ShowDialog() == DialogResult.OK )
                    {
                        File.WriteAllText( saveFileDialog1.FileName, shit2, Encoding.Default );
                    }
                }
            }
            catch
            {
                MessageBox.Show( "An error occurred" );
            }
        }
    }
}
