using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUESYNC_C_
{
    internal class Program
    {
        // MÉTODOS DE FECHA Y HORA//
        public static string ObtenerFechaActual()
        {
            return DateTime.Now.ToString("yyyy-MM-dd"); // parámetro ordenado
        }
        public static string ObtenerHoraActual()
        {
            return DateTime.Now.ToString("HH:mm:ss"); // parámetro ordenado
        }
        // CAPTURA DE DATOS DEL CLIENTE//
        public static (string cliente, string telefono, string correo, string tipo_comp, string documento) CapturarDatosCliente()
        {
            Console.WriteLine("\n--- DATOS DEL CLIENTE ---");
            // 1. Nombre completo
            Console.Write("Nombre completo: ");
            string cliente = Console.ReadLine()?.Trim() ?? "";
            while (string.IsNullOrEmpty(cliente))
            {
                Console.Write("  [Error] El nombre no puede estar vacío. Ingrese nuevamente: ");
                cliente = Console.ReadLine()?.Trim() ?? "";
            }
            // 2. Teléfono (Validando que no esté vacío y que tenga exactamente 9 dígitos)
            Console.Write("Teléfono (9 dígitos): ");
            string telefono = Console.ReadLine()?.Trim() ?? "";
            while (string.IsNullOrEmpty(telefono) || telefono.Length != 9)
            {
                Console.Write("  [Error] Ingrese un teléfono válido (exactamente 9 dígitos): ");
                telefono = Console.ReadLine()?.Trim() ?? "";
            }
            // 3. Correo electrónico (Validando que no esté vacío y contenga @)
            Console.Write("Correo electrónico: ");
            string correo = Console.ReadLine()?.Trim().ToLower() ?? "";
            while (string.IsNullOrEmpty(correo) || !correo.Contains("@") || correo.Contains(" "))
            {
                Console.Write("  [Error] Ingrese un correo válido (@gmail.com, etc.): ");
                correo = Console.ReadLine()?.Trim().ToLower() ?? "";
            }
            // 4. Tipo de Comprobante
            Console.WriteLine("\nTipo de Comprobante: 1. Boleta | 2. Factura");
            Console.Write("Seleccione (1-2): ");
            string tipo_sel = Console.ReadLine()?.Trim() ?? "";
            while (tipo_sel != "1" && tipo_sel != "2")
            {
                Console.Write("  [Error] Seleccione 1 o 2: ");
                tipo_sel = Console.ReadLine()?.Trim() ?? "";
            }
            string tipo_comp = (tipo_sel == "2") ? "Factura" : "Boleta";
            // 5. Documento según comprobante (RUC de 11 dígitos o DNI de 8 dígitos)
            string documento = "";
            if (tipo_comp == "Factura")
            {
                Console.Write("Ingrese RUC (11 dígitos): ");
                documento = Console.ReadLine()?.Trim() ?? "";
                while (string.IsNullOrEmpty(documento) || documento.Length != 11)
                {
                    Console.Write("  [Error] Ingrese un RUC válido (11 dígitos): ");
                    documento = Console.ReadLine()?.Trim() ?? "";
                }
            }
            else
            {
                Console.Write("Ingrese DNI (8 dígitos): ");
                documento = Console.ReadLine()?.Trim() ?? "";
                while (string.IsNullOrEmpty(documento) || documento.Length != 8)
                {
                    Console.Write("  [Error] Ingrese un DNI válido (8 dígitos): ");
                    documento = Console.ReadLine()?.Trim() ?? "";
                }
            }
            return (cliente, telefono, correo, tipo_comp, documento);
        }
        static void Main(string[] args)
        {
        }
    }
}