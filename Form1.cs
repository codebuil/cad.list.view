namespace batView
{
    public partial class Form1 : Form
    {
        private List<string> listaDeItens = new List<string>();
        int value = -1;
        public Form1()
        {
            InitializeComponent();
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
    }
    }
