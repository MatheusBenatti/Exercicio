using System;

namespace Questao1
{
    class ContaBancaria
    {
        public readonly int Numero;
        
        public string Titular { get; set; }

        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular, double? depositoInicial = null)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial ?? 0;
        }

        public void Deposito(double valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor de depósito deve ser positivo.");
            }
            Saldo += valor;
        }

        public void Saque(double valor)
        {
            double valorTotal = valor + TaxaSaque;

            Saldo -= valorTotal;
        }

    }
}
