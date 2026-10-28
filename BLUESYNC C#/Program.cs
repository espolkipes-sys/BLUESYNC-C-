using System;
using System.Collections.Generic;
using System.IO;
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
            Console.Write(" Ingrese su nombre completo: ");
            string cliente = Console.ReadLine()?.Trim() ?? "";
            while (string.IsNullOrEmpty(cliente))
            {
                Console.Write("  [Error] El nombre no puede estar vacío. Ingrese nuevamente: ");
                cliente = Console.ReadLine()?.Trim() ?? "";
            }
            // 2. Teléfono (Validando que no esté vacío y que tenga exactamente 9 dígitos)
            Console.Write(" Ingrese su teléfono (9 dígitos): ");
            string telefono = Console.ReadLine()?.Trim() ?? "";
            while (string.IsNullOrEmpty(telefono) || telefono.Length != 9)
            {
                Console.Write("  [Error] Ingrese un teléfono válido (exactamente 9 dígitos): ");
                telefono = Console.ReadLine()?.Trim() ?? "";
            }
            // 3. Correo electrónico (Validando que no esté vacío y contenga @)
            Console.Write(" Ingrese su correo electrónico: ");
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
                Console.Write("Ingrese su  DNI (8 dígitos): ");
                documento = Console.ReadLine()?.Trim() ?? "";
                while (string.IsNullOrEmpty(documento) || documento.Length != 8)
                {
                    Console.Write("  [Error] Ingrese un DNI válido (8 dígitos): ");
                    documento = Console.ReadLine()?.Trim() ?? "";
                }
            }
            return (cliente, telefono, correo, tipo_comp, documento);
        }
        // Definición de la ruta del archivo plano
        public static string rutaArchivo = diccionarios.rutaArchivo;
        // 1. GUARDAR DATOS EN ARCHIVO
        public static void GuardarDatosArchivo()
        {
            Console.WriteLine("***************** GUARDAR COMPROBANTES EN ARCHIVO ****************");
            Console.WriteLine("*******************************************************************");
            if (diccionarios.total_registros == 0)
            {
                Console.WriteLine(" [Aviso] No hay comprobantes registrados para guardar.");
                return;
            }
            using (StreamWriter escribir = new StreamWriter(diccionarios.rutaArchivo))
            {
                for (int i = 0; i < diccionarios.total_registros; i++)
                {
                    // Usando exactamente los nombres de tu clase diccionarios.cs
                    string linea = $"{diccionarios.nroComp[i]}|{diccionarios.tipoComp[i]}|{diccionarios.cliente[i]}|{diccionarios.documento[i]}|{diccionarios.telefono[i]}|{diccionarios.servicio[i]}|{diccionarios.monto[i]}|{diccionarios.fecha[i]}|{diccionarios.hora[i]}";
                    escribir.WriteLine(linea);
                }
            }
            Console.WriteLine($" [Éxito] {diccionarios.total_registros} comprobante(s) guardado(s) correctamente.");
        }
        // 2. LEER DATOS DESDE ARCHIVO (MOSTRAR EN CONSOLA)
        public static void LeerDatosArchivo()
        {
            Console.WriteLine("***************** LEER DATOS DESDE ARCHIVO ****************");
            Console.WriteLine("***********************************************************");

            if (!File.Exists(diccionarios.rutaArchivo))
            {
                Console.WriteLine(" [Aviso] El archivo no existe todavía.");
                return;
            }
            Console.WriteLine($"{"NRO",-12} | {"TIPO",-8} | {"CLIENTE",-15} | {"DOC",-11} | {"MONTO",-10}");
            Console.WriteLine(new string('-', 65));
            using (StreamReader leer = new StreamReader(diccionarios.rutaArchivo))
            {
                string linea;
                while ((linea = leer.ReadLine()) != null)
                {
                    string[] datos = linea.Split('|');
                    if (datos.Length == 9)
                    {
                        string clienteCorto = datos[2].Length > 15 ? datos[2].Substring(0, 15) : datos[2];
                        Console.WriteLine($"{datos[0],-12} | {datos[1],-8} | {clienteCorto,-15} | {datos[3],-11} | S/ {datos[6],7}");
                    }
                }
            }
        }
        // 3. CARGAR DATOS DESDE ARCHIVO A LA MEMORIA RAM
        public static void CargarDatosArchivo()
        {
            if (!File.Exists(diccionarios.rutaArchivo))
            {
                Console.WriteLine(" [Info] Archivo de historial no encontrado. Se creará uno nuevo al guardar.");
                return;
            }
            string[] lineas = File.ReadAllLines(diccionarios.rutaArchivo);
            diccionarios.total_registros = 0; // Reiniciamos el contador antes de cargar
            for (int i = 0; i < lineas.Length && diccionarios.total_registros < diccionarios.max_registros; i++)
            {
                string[] datos = lineas[i].Split('|');
                if (datos.Length == 9)
                {
                    // Asignando a los arreglos exactos de la  clase diccionarios.cs
                    diccionarios.nroComp[diccionarios.total_registros] = datos[0];
                    diccionarios.tipoComp[diccionarios.total_registros] = datos[1];
                    diccionarios.cliente[diccionarios.total_registros] = datos[2];
                    diccionarios.documento[diccionarios.total_registros] = datos[3];
                    diccionarios.telefono[diccionarios.total_registros] = datos[4];
                    diccionarios.servicio[diccionarios.total_registros] = datos[5];
                    diccionarios.monto[diccionarios.total_registros] = Convert.ToDouble(datos[6]);
                    diccionarios.fecha[diccionarios.total_registros] = datos[7];
                    diccionarios.hora[diccionarios.total_registros] = datos[8];

                    diccionarios.total_registros++;
                }
            }
            Console.WriteLine($" [Info] {diccionarios.total_registros} registro(s) recuperado(s) del archivo local.");
        }
        static void Main(string[] args)
        {
        }
    }
}