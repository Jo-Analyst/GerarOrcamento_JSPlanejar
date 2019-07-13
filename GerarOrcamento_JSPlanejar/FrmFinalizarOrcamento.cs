﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerarOrcamento_JSPlanejar
{
    public partial class FrmFinalizarOrcamento : Form
    {
        decimal ValorPeças, ValorTotal, ValorAcrescimo;
        string EstimativaEntrega;
        int Cod_cliente;

        public FrmFinalizarOrcamento(decimal ValorPeças, int CodCliente)
        {
            InitializeComponent();
            this.ValorPeças = ValorPeças;
            ValorTotal = ValorPeças;
            lblValorPeças.Text = "R$ " + ValorPeças;
            lblValorTotal.Text = "R$ " + ValorPeças;
            Cod_cliente = CodCliente;
        }

        public bool voltar {get;set;}
        string conclusao;

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            voltar = true;
            Close();
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (txtProdutoFinal.Text != "" && lblEstimativaEntrega.Text != "")
            {
                voltar = false;
                GerarOrcamento();
                conclusao = "fim";
                this.Close();
            }
            else if (txtProdutoFinal.Text == "")
            {
                MessageBox.Show("Preencha a descrição do produto final!", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (lblEstimativaEntrega.Text == "")
            {
                MessageBox.Show("Informe a previsão de entrega!", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GerarOrcamento()
        {
            try
            {
                ClassOrcamento orcamento = new ClassOrcamento();
                orcamento._ValorPeças_Orcamento = ValorPeças;
                orcamento._ValorMaoObra = ValorAcrescimo;
                orcamento._ValorTotal_Orcamento = ValorTotal;
                orcamento._DataPedido = DateTime.Now.ToShortDateString();
                orcamento._EstimativaEntrega = EstimativaEntrega;
                orcamento._Situacao = "Pendente";
                orcamento._ProdutoFinal = txtProdutoFinal.Text.Trim();
                orcamento._CodCliente = Cod_cliente;
                orcamento.GerarOrcamento();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmFinalizarOrcamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conclusao == "fim")
                voltar = false;
            else
                voltar = true;
        }

        private void btnDataEntrega_Click(object sender, EventArgs e)
        {
            FrmPrevisaoEntrega previsaoEntrega = new FrmPrevisaoEntrega();
            previsaoEntrega.ShowDialog();
            if (!string.IsNullOrEmpty(previsaoEntrega.DataEntrega))
            {
                if (previsaoEntrega.Validade == false)
                {
                    lblEstimativaEntrega.Text = " Dia " + previsaoEntrega.DataEntrega + " - " + previsaoEntrega.Tempo + " Dia(s)";
                    EstimativaEntrega = previsaoEntrega.DataEntrega;
                }
                else
                {
                    lblEstimativaEntrega.Text = "Sem previsão de entrega";
                    EstimativaEntrega = lblEstimativaEntrega.Text;
                }
            }
        }

        private void txtAcrescimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Método que permite a entrada de número e uma virgula após inserção de números
            try
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    if (((int)e.KeyChar) != ((int)Keys.Back))
                        if (e.KeyChar != ',')
                            e.Handled = true;
                        else if (txtAcrescimo.Text.IndexOf(',') > 0)
                            e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAcrescimo_Leave(object sender, EventArgs e)
        {
            if (txtAcrescimo.Text != "")
            {
                ValorAcrescimo = decimal.Parse(txtAcrescimo.Text);
                ValorTotal = ValorPeças + ValorAcrescimo;
                lblValorTotal.Text = "R$ " + ValorTotal;
                txtAcrescimo.Text = decimal.Parse(txtAcrescimo.Text.Trim()).ToString("0.00");
            }
            else
            {
                ValorTotal -= ValorAcrescimo;
                lblValorTotal.Text = "R$ " + ValorTotal;
                ValorAcrescimo = 0.00m;
            }
        }
    }
}
