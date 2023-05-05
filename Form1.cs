using static System.Windows.Forms.LinkLabel;

namespace batView
{
    public partial class Form1 : Form
    {
        private List<string> listaDeItens = new List<string>();
        int value = -1;
        double scale = 600.00;
        double units = 10.00;

        public Form1()
        {
            InitializeComponent();
        }
        double wasm_draw3Dx(double x, double y, double z)
        {
            double zz = (z + 1.00) * 1.50;
            double zzz = scale / zz;
            double zzzz = scale / 2 - zzz / 2;
            double zzzzz = (zzz / units) * x;
            double zzzzzz = zzzz + zzzzz;
            return zzzzzz;
        }
        double wasm_draw3Dy(double x, double y, double z)
        {
            double zz = (z + 1.00) * 1.50;
            double zzz = scale / zz;
            double zzzz = scale / 2 - zzz / 2;
            double zzzzz = (zzz / units) * (units - y);
            double zzzzzz = zzzz + zzzzz - (z * 2);
            return zzzzzz;
        }
        void setScale(double sc)
        {
            scale = sc;
        }
        void setunits(double sc)
        {
            units = sc;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Limpa a lista atual
            listaDeItens.Clear();
            value = -1;

            // Carrega os itens do arquivo de texto
            string nomeArquivo = textBox1.Text;
            if (File.Exists(nomeArquivo))
            {
                StreamReader sr = new StreamReader(nomeArquivo);
                string linha;
                while ((linha = sr.ReadLine()) != null)
                {
                    listaDeItens.Add(linha);
                }
                sr.Close();
            }

            // Atualiza a lista exibida na tela
            AtualizarLista();
        }
        private void AtualizarLista()
        {
            // Limpa a lista exibida na tela
            lstItens.Items.Clear();

            // Adiciona os itens da lista atual à lista exibida na tela
            foreach (string item in listaDeItens)
            {
                lstItens.Items.Add(item);
            }
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Salva a lista no arquivo de texto
            string nomeArquivo = textBox1.Text;
            StreamWriter sw = new StreamWriter(nomeArquivo);
            foreach (string item in listaDeItens)
            {
                sw.WriteLine(item);
            }
            sw.Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Adiciona um item à lista
            string novoItem = textBox2.Text;
            listaDeItens.Add(novoItem);

            // Atualiza a lista exibida na tela
            AtualizarLista();
            value = -1;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Remove o item selecionado da lista
            int indiceSelecionado = lstItens.SelectedIndex;
            if (indiceSelecionado >= 0)
            {
                listaDeItens.RemoveAt(indiceSelecionado);

                // Atualiza a lista exibida na tela
                AtualizarLista();
            }
            value = -1;
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exibe o item selecionado em uma mensagem
            int indiceSelecionado = lstItens.SelectedIndex;
            if (indiceSelecionado >= 0)
            {
                string valorSelecionado = listaDeItens[indiceSelecionado];
                string titulo = string.Format("Item {0}: {1}", indiceSelecionado, valorSelecionado);
                textBox2.Text= valorSelecionado;
            }
            value = indiceSelecionado;

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Edita o item selecionado da lista
            int indiceSelecionado = lstItens.SelectedIndex;
            if (indiceSelecionado >= 0)
            {
                string novoValor = textBox2.Text;
                listaDeItens[indiceSelecionado] = novoValor;

                // Atualiza a lista exibida na tela
                AtualizarLista();
            }
            value = indiceSelecionado;
        }

        private void redrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw();
        }
        private void draw()
        {
            int xx = 0;
            int yy = 0;
            int xx2 = 0;
            int yy2 = 0;
            int counter = 0;
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.Clear(Color.Blue);

                foreach (string item in listaDeItens)
            {
                string[] s = item.Split(",");
                    if (s.Length > 4)
                    {
                        xx = int.Parse(s[1]);
                        yy = int.Parse(s[2]);
                        xx2 = int.Parse(s[3]);
                        yy2 = int.Parse(s[4]);
                        

                        {
                            
                            Pen pen = new Pen(Color.White);
                            g.DrawLine(pen, xx, yy, xx2, yy2);
                            if (value > -1 && value == counter)
                            {
                                Pen pen2 = new Pen(Color.Black);
                                g.DrawLine(pen2, xx, yy, xx2, yy2);
                            }
                            counter++;
                        }
                    }
                }
                pictureBox1.Invalidate();
                
            }
                
            }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listaDeItens.Clear();
            value = -1;
            AtualizarLista();
        }

        private void lstItens_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Exibe o item selecionado em uma mensagem
            int indiceSelecionado = lstItens.SelectedIndex;
            if (indiceSelecionado >= 0)
            {
                string valorSelecionado = listaDeItens[indiceSelecionado];
                value= indiceSelecionado;
                textBox2.Text = valorSelecionado;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(800,800);
        }

        private void loadbinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Limpa a lista atual
            listaDeItens.Clear();
            value = -1;
            if (textBox1.Text != "" && File.Exists(textBox1.Text))
            {

                
                const string Header = "batcad";
                const int RecordSize = 20; // 5 integers * 4 bytes each

                using (var fileStream = new FileStream(textBox1.Text, FileMode.Open))
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    // Read header

                    var headerBytes = binaryReader.ReadBytes(Header.Length);
                    var header = System.Text.Encoding.ASCII.GetString(headerBytes);
                    if (header != Header)
                    {

                        return;
                    }

                    // Read file size
                    Int32 fileSize = binaryReader.ReadInt32();
                    var recordCount = fileSize;

                    // Read records
                    for (int i = 0; i < recordCount; i++)
                    {

                        int firstInt = (int)binaryReader.ReadInt32();
                        if (firstInt == 1)
                        {

                            var ints = new int[5];
                            for (int j = 0; j < 4; j++)
                            {
                                ints[j] = (int)binaryReader.ReadInt32();
                            }


                            listaDeItens.Add("line," + ints[0].ToString()+ "," + ints[1].ToString() + "," + ints[2].ToString() + "," + ints[3].ToString() +"");

                        }




                    }
                    
                    binaryReader.Close();
                    
                }
                // Atualiza a lista exibida na tela
                AtualizarLista();
            }

        }

        private void savebinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                const string Header = "batcad";

                using (var fileStream = new FileStream(textBox1.Text, FileMode.Create))
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    // Write header
                    var headerBytes = System.Text.Encoding.ASCII.GetBytes(Header);
                    binaryWriter.Write(headerBytes);

                    // Write file size
                    var fileSize = (int)lstItens.Items.Count;
                    binaryWriter.Write(fileSize);
                    int v = 1;
                    // Write data
                    foreach (string item in listaDeItens)
                    {
                        string[] listss = item.Split(",");
                        if (listss.Length > 4)
                        {
                            int xxx = int.Parse(listss[1]);
                            int yyy = int.Parse(listss[2]);
                            int xxx2 = int.Parse(listss[3]);
                            int yyy2 = int.Parse(listss[4]);
                            
                            binaryWriter.Write(v);
                            binaryWriter.Write(xxx);
                            binaryWriter.Write(yyy);
                            binaryWriter.Write(xxx2);
                            binaryWriter.Write(yyy2);
                        }
                    }
                    binaryWriter.Close();

                }
            }

        }

        private void load3dbatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            if (textBox1.Text != "" && File.Exists(textBox1.Text))
            {
                
                bool b = false;
                string[] linhas = System.IO.File.ReadAllLines(textBox1.Text); // Lê todas as linhas do arquivo

                foreach (string linha in linhas) // Percorre cada linha do arquivo
                {
                    if (linha.StartsWith("line")) // Verifica se a linha começa com a string "line,0,0,100"
                    {
                        // Separa as coordenadas da linha
                        string[] coords = linha.Split(',');
                        if (coords.Count() > 6)
                        {
                            int x1 = (int)wasm_draw3Dx((double)int.Parse(coords[1].ToString()), (double)int.Parse(coords[2].ToString()), (double)int.Parse(coords[3].ToString()));
                            int y1 = (int)wasm_draw3Dy((double)int.Parse(coords[1].ToString()), (double)int.Parse(coords[2].ToString()), (double)int.Parse(coords[3].ToString()));
                            int x2 = (int)wasm_draw3Dx((double)int.Parse(coords[4].ToString()), (double)int.Parse(coords[5].ToString()), (double)int.Parse(coords[6].ToString()));
                            int y2 = (int)wasm_draw3Dy((double)int.Parse(coords[4].ToString()), (double)int.Parse(coords[5].ToString()), (double)int.Parse(coords[6].ToString()));
                            listaDeItens.Add("line," + x1.ToString() + "," + y1.ToString() + "," + x2.ToString() + "," + y2.ToString() + "");
                            
                            // Desenha a linha na picture1 em branco
                            
                        }
                    }
                }
                // Atualiza a lista exibida na tela
                AtualizarLista();

            }
        }

        private void load3dbinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Limpa a lista atual
            listaDeItens.Clear();
            value = -1;
            if (textBox1.Text != "" && File.Exists(textBox1.Text))
            {


                const string Header = "bat3d";
                const int RecordSize = 20; // 5 integers * 4 bytes each

                using (var fileStream = new FileStream(textBox1.Text, FileMode.Open))
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    // Read header

                    var headerBytes = binaryReader.ReadBytes(Header.Length);
                    var header = System.Text.Encoding.ASCII.GetString(headerBytes);
                    if (header != Header)
                    {

                        return;
                    }

                    // Read file size
                    Int32 fileSize = binaryReader.ReadInt32();
                    var recordCount = fileSize;

                    // Read records
                    for (int i = 0; i < recordCount; i++)
                    {

                        int firstInt = (int)binaryReader.ReadInt32();
                        if (firstInt == 1)
                        {

                            var ints = new int[9];
                            for (int j = 0; j < 6; j++)
                            {
                                ints[j] = (int)binaryReader.ReadInt32();
                            }
                            int x1 = (int)wasm_draw3Dx((double)ints[0], (double)ints[1], (double)ints[2]);
                            int y1 = (int)wasm_draw3Dy((double)ints[0], (double)ints[1], (double)ints[2]);
                            int x2 = (int)wasm_draw3Dx((double)ints[3], (double)ints[4], (double)ints[5]); 
                            int y2 = (int)wasm_draw3Dy((double)ints[3], (double)ints[4], (double)ints[5]);
                            listaDeItens.Add("line," + x1.ToString() + "," + y1.ToString() + "," + x2.ToString() + "," + y2.ToString() + "");

                            

                        }




                    }

                    binaryReader.Close();

                }
                // Atualiza a lista exibida na tela
                AtualizarLista();
            }
        }
    }
    }
