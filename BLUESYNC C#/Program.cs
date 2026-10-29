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
         // 1  MÉTODOS DE FECHA Y HORA//
        public static string ObtenerFechaActual()
        {
            return DateTime.Now.ToString("yyyy-MM-dd"); // parámetro ordenado
        }
        public static string ObtenerHoraActual()
        {
            return DateTime.Now.ToString("HH:mm:ss"); // parámetro ordenado
        } 
        // 2  CAPTURA DE DATOS DEL CLIENTE//
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
        // 3. GUARDAR DATOS EN ARCHIVO
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
        //  LEER DATOS DESDE ARCHIVO (MOSTRAR EN CONSOLA)
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
        //  CARGAR DATOS DESDE ARCHIVO A LA MEMORIA RAM
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
        // 4.  SERVICIOS  //
        // SERVICIO DE CONSTRUCCIÓN DE PISCINAS
        public static void ServicioConstruccion()
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("      1. SERVICIO DE CONSTRUCCIÓN DE PISCINAS      ");
            Console.WriteLine("==================================================");
            (string cliente, string telefono, string correo, string tipo_comp, string documento) = CapturarDatosCliente();

            Console.WriteLine("\n--- REQUERIMIENTOS DEL SERVICIO ---");
            Console.Write("Largo (m): ");
            double largo = Convert.ToDouble(Console.ReadLine());
            Console.Write("Ancho (m): ");
            double ancho = Convert.ToDouble(Console.ReadLine());
            double area = largo * ancho;

            Console.WriteLine("\nSeleccione Tipo de Acabado:");
            Console.WriteLine($"1. Básico (S/ {diccionarios.precioBasico} x m²)");
            Console.WriteLine($"2. Premium (S/ {diccionarios.precioPremium} x m²)");
            Console.Write("Opción (1-2): ");
            string opc_acabado = Console.ReadLine()?.Trim() ?? "";

            string acabado_nombre = (opc_acabado == "2") ? "Premium" : "Básico";
            double costo_material = area * (opc_acabado == "2" ? diccionarios.precioPremium : diccionarios.precioBasico);

            Console.WriteLine("\n--- SELECCIÓN DE MANO DE OBRA ---");
            for (int i = 0; i < diccionarios.idMo.Length; i++)
            {
                Console.WriteLine($"{diccionarios.idMo[i]}. {diccionarios.nomMo[i]} - S/ {diccionarios.costoMo[i]:F2} por día");
            }

            Console.Write("Seleccione cuadrilla (1-3): ");
            string opc_mo = Console.ReadLine()?.Trim() ?? "";
            while (opc_mo != "1" && opc_mo != "2" && opc_mo != "3")
            {
                Console.Write("  [Error] Ingrese opción 1, 2 o 3: ");
                opc_mo = Console.ReadLine()?.Trim() ?? "";
            }

            // Convertimos la opción (1, 2, 3) a índice de arreglo (0, 1, 2)
            int indexMo = Convert.ToInt32(opc_mo) - 1;
            double costoDiaMo = diccionarios.costoMo[indexMo];

            Console.Write("Días estimados de construcción: ");
            int dias =int.Parse(Console.ReadLine());
            double costo_mo = costoDiaMo * dias;

            double monto_total = costo_material + costo_mo;
            string fecha = ObtenerFechaActual();
            string hora = ObtenerHoraActual();

            // Generador de número de comprobante secuencial
            string nro_comp = $"B001-{diccionarios.total_registros + 1:D5}";
            string detalle = $"Construcción {acabado_nombre} ({area:F1}m²)";

            // Verificamos si hay espacio disponible
            if (diccionarios.total_registros >= diccionarios.max_registros)
            {
                Console.WriteLine("  [Error] Se ha alcanzado el límite máximo de registros.");
                return;
            }

            // Llamada al método Registrar encapsulado en diccionarios.cs
            diccionarios.Registrar(nro_comp, tipo_comp, cliente, documento, telefono, detalle, monto_total, fecha, hora);

            // Guardamos automáticamente en el archivo plano
            GuardarDatosArchivo();

            Console.WriteLine($"\n[Éxito] Comprobante {nro_comp} generado el {fecha} a las {hora}");
            Console.WriteLine($"        Total a pagar: S/ {monto_total:N2}");
        }
        // SERVICIO DE MANTENIMIENTO DE PISCINA//   
        public static void ServicioMantenimiento()
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("      2. SERVICIO DE MANTENIMIENTO DE PISCINAS    ");
            Console.WriteLine("==================================================");
            (string cliente, string telefono, string correo, string tipo_comp, string documento) = CapturarDatosCliente();
            Console.WriteLine("\n--- REQUERIMIENTOS DEL SERVICIO ---");
            Console.Write("Horas de trabajo: ");
            double horas = double.Parse(Console.ReadLine());
            double costo_base = horas * diccionarios.tarifaHora;

            Console.Write($"¿Incluir Kit de Químicos (S/ {diccionarios.precioKitQuimicos})? (S/N): ");
            string incluir = Console.ReadLine()?.Trim().ToUpper() ?? "";
            while (incluir != "S" && incluir != "N")
            {
                Console.Write("  [Error] Ingrese S o N: ");
                incluir = Console.ReadLine()?.Trim().ToUpper() ?? "";
            }
            double costo_kit = (incluir == "S") ? diccionarios.precioKitQuimicos : 0.0;
            double monto_total = costo_base + costo_kit;
            string fecha = ObtenerFechaActual();
            string hora = ObtenerHoraActual();
            string nro_comp = $"B001-{diccionarios.total_registros + 1:D5}";
            string detalle = "Mantenimiento (" + horas + " horas)";

            if (diccionarios.total_registros >= diccionarios.max_registros)
            {
                Console.WriteLine("  [Error] Se ha alcanzado el límite máximo de registros.");
                return;
            }
            diccionarios.Registrar(nro_comp, tipo_comp, cliente, documento, telefono, detalle, monto_total, fecha, hora);

            GuardarDatosArchivo();
            Console.WriteLine($"\n[Éxito] Comprobante {nro_comp} generado el {fecha} a las {hora}");
            Console.WriteLine($"        Total a pagar: S/ {monto_total}");
        }
        //  SERVICIO DE SUMINISTRO DE MATERIALES//
        public static void ServicioSuministros()
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("      3. SERVICIO DE SUMINISTRO DE MATERIALES    ");
            Console.WriteLine("==================================================");
            (string cliente, string telefono, string correo, string tipo_comp, string documento) = CapturarDatosCliente();

            double monto_total = 0.0;
            string detalle_items = "";
            bool primer_item = true;

            while (true)
            {
                Console.WriteLine("\n--- CATÁLOGO DE MATERIALES ---");
                for (int i = 0; i < diccionarios.idMat.Length; i++)
                {
                    Console.WriteLine(diccionarios.idMat[i] + ". " + diccionarios.nomMat[i] + " - S/ " + diccionarios.precMat[i] + " (Stock: " + diccionarios.stockMat[i] + ")");
                }
                Console.WriteLine("0. Finalizar compra");

                Console.Write("\nSeleccione producto a comprar (0-4): ");
                string sel = Console.ReadLine()?.Trim() ?? "";

                if (sel == "0")
                    break;
                // 1. Buscamos si la opción ingresada existe en el catálogo
                int indexMat = -1;
                for (int i = 0; i < diccionarios.idMat.Length; i++)
                {
                    if (diccionarios.idMat[i] == sel)
                    {
                        indexMat = i;
                        break;
                    }
                }
                // Si no se encontró, la opción no es válida
                if (indexMat == -1)
                {
                    Console.WriteLine("  [Error] Opción no válida. Ingrese un número del catálogo (0 al 4).");
                    continue;
                }
                //  Verificamos si hay stock disponible
                if (diccionarios.stockMat[indexMat] <= 0)
                {
                    Console.WriteLine("  [Aviso] Producto sin stock disponible.");
                    continue;
                }
                //  Pedimos la cantidad que desea llevar
                Console.Write("Cantidad para '" + diccionarios.nomMat[indexMat] + "': ");
                int cant = int.Parse(Console.ReadLine());
                if (cant > diccionarios.stockMat[indexMat])
                {
                    Console.WriteLine("  [Error] Solo quedan " + diccionarios.stockMat[indexMat] + " unidades.");
                    continue;
                }
                //  Restamos del stock en el diccionario
                diccionarios.stockMat[indexMat] = diccionarios.stockMat[indexMat] - cant;

                //  Calculamos el subtotal y lo sumamos al total general
                double subtotal = cant * diccionarios.precMat[indexMat];
                monto_total = monto_total + subtotal;

                //  Juntamos el nombre y la cantidad para el detalle del comprobante
                string itemActual = cant + "x " + diccionarios.nomMat[indexMat];
                if (primer_item)
                {
                    detalle_items = itemActual;
                    primer_item = false;
                }
                else
                {
                    detalle_items = detalle_items + ", " + itemActual;
                }

                Console.WriteLine("  -> Añadido: Subtotal S/ " + subtotal);
            }

            // Si compró al menos un producto, generamos el comprobante
            if (monto_total > 0)
            {
                string fecha = ObtenerFechaActual();
                string hora = ObtenerHoraActual();  

                // Número de comprobante con formato D5
                string nro_comp = $"B001-{diccionarios.total_registros + 1:D5}";
                string detalle = "Suministro: " + detalle_items;

                // Verificamos espacio disponible
                if (diccionarios.total_registros >= diccionarios.max_registros)
                {
                    Console.WriteLine("  [Error] Se ha alcanzado el límite máximo de registros.");
                    return;
                }

                // Guardado directo en el diccionario y en el archivo
                diccionarios.Registrar(nro_comp, tipo_comp, cliente, documento, telefono, detalle, monto_total, fecha, hora);
                GuardarDatosArchivo();

                Console.WriteLine("\n[Éxito] Comprobante " + nro_comp + " generado el " + fecha + " a las " + hora);
                Console.WriteLine("        Total a pagar: S/ " + monto_total);
            }
            else
            {
                Console.WriteLine("\n[Aviso] No se seleccionó ningún producto. Operación cancelada.");
            }

        }
        // 5. CONFIGURACIÓN DE PRECIOS Y CATÁLOGOS
        public static void ConfigurarCatalogos()
        {
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("       GESTOR DE PRECIOS DEL SISTEMA              ");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("1. Cambiar precio acabado Básico");
            Console.WriteLine("2. Cambiar tarifa por hora de mantenimiento");
            Console.WriteLine("3. Cambiar precio Kit de Químicos");
            Console.WriteLine("4. Regresar");
            Console.Write("Seleccione (1-4): ");
            string opc = Console.ReadLine()?.Trim() ?? "";
            while (opc != "1" && opc != "2" && opc != "3" && opc != "4")
            {
                Console.WriteLine("  [Error] Opción no válida. Ingrese un número del 1 al 4.");
                Console.Write("Seleccione (1-4): ");
                opc = Console.ReadLine()?.Trim() ?? "";
            }
            if (opc == "1")
            {
                double nuevo_p = PedirPrecioValido("Nuevo precio x m² para acabado Básico: S/ ");
                diccionarios.precioBasico = nuevo_p;
                Console.WriteLine("  [Éxito] Precio actualizado.");
            }
            else if (opc == "2")
            {
                double nuevo_p = PedirPrecioValido("Nueva tarifa por hora: S/ ");
                diccionarios.tarifaHora = nuevo_p;
                Console.WriteLine("  [Éxito] Tarifa actualizada.");
            }
            else if (opc == "3")
            {
                double nuevo_p = PedirPrecioValido("Nuevo precio Kit de Químicos: S/ ");
                diccionarios.precioKitQuimicos = nuevo_p;
                Console.WriteLine("  [Éxito] Precio actualizado.");
            }
            else if (opc == "4")
            {
                Console.WriteLine("  Regresando al menú principal...");
                return;
            }
        }
        public static double PedirPrecioValido(string mensaje)
        {
            double precio = 0.0;
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine()?.Trim() ?? "";
                if (double.TryParse(entrada, out precio) && precio > 0)
                {
                    return precio;
                }

                Console.WriteLine("  [Error] Entrada no válida. Ingrese solo números mayores a 0 (ejemplo: 15.50).");
            }
        }
        // 6. BUSCADOR DE COMPROBANTES//
        public static void BuscarComprobantes()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\tBUSCADOR DE COMPROBANTES EMITIDOS");
            Console.WriteLine(new string('=', 60));
            if (diccionarios.total_registros == 0)
            {
                Console.WriteLine("No existen registros...!!!");
                return;
            }
            Console.Write("Ingrese DNI o RUC a buscar: ");
            string doc_buscar = Console.ReadLine()?.Trim() ?? "";
            bool encontrados = false;
            for (int i = 0; i < diccionarios.total_registros; i++)
            {
                if (diccionarios.documento[i] == doc_buscar)
                {
                    encontrados = true;
                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"Nro Comprobante : {diccionarios.nroComp[i]} ({diccionarios.tipoComp[i]})");
                    Console.WriteLine($"Fecha y Hora    : {diccionarios.fecha[i]} - {diccionarios.hora[i]}");
                    Console.WriteLine($"Cliente         : {diccionarios.cliente[i]}");
                    Console.WriteLine($"Documento       : {diccionarios.documento[i]}");
                    Console.WriteLine($"Detalle Servicio: {diccionarios.servicio[i]}");
                    Console.WriteLine($"Monto Total     : S/ {diccionarios.monto[i]:F2}");
                    Console.WriteLine(new string('-', 60));
                }
            }
            if (!encontrados)
            {
                Console.WriteLine($"No se encontraron registros para el documento: {doc_buscar}");
            }
        }
        // 7 REPORTE DE VENTAS//
        public static void ReporteVentas()
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("\t\t\tREPORTE DE VENTAS");
            Console.WriteLine(new string('-', 60));

            // Validación por si no hay registros
            if (diccionarios.total_registros == 0)
            {
                Console.WriteLine("\tNo hay ventas registradas.");
                return;
            }
            Console.WriteLine("1. Ver todas las ventas");
            Console.WriteLine("2. Filtrar ventas por fecha (AAAA-MM-DD)");
            Console.Write("Seleccione (1-2): ");
            string opc = Console.ReadLine()?.Trim() ?? "";
            while (opc != "1" && opc != "2")
            {
                Console.Write("  [Error] Opción no válida. Por favor, ingrese 1 o 2: ");
                opc = Console.ReadLine()?.Trim() ?? "";
            }

            string fecha_filtro = "";
            if (opc == "2")
            {
                Console.Write("Ingrese fecha (Ejemplo: 2026-10-29): ");
                fecha_filtro = Console.ReadLine()?.Trim() ?? "";
            }

            double total_recaudado = 0.0;
            Console.WriteLine("\n" + new string('-', 65));
            Console.WriteLine($"{"NRO",-12} | {"FECHA",-10} | {"CLIENTE",-15} | {"MONTO",-10}");
            Console.WriteLine(new string('-', 65));

            bool hay_resultados = false;

            // Recorremos los arreglos paralelos hasta total_registros
            for (int i = 0; i < diccionarios.total_registros; i++)
            {
                // Si la opción es 1 (muestra todo) o si la fecha coincide con el filtro
                if (opc == "1" || diccionarios.fecha[i] == fecha_filtro)
                {
                    hay_resultados = true;
                    // Cortamos el nombre del cliente a máximo 15 caracteres para no romper  el formato de la tabla
                    string clienteCorto = diccionarios.cliente[i].Length > 15 ? diccionarios.cliente[i].Substring(0, 15) : diccionarios.cliente[i];   
                    // Impresión formateada con los anchos de columna correspondientes
                    Console.WriteLine($"{diccionarios.nroComp[i],-12} | {diccionarios.fecha[i],-10} | {clienteCorto,-15} | S/ {diccionarios.monto[i],7:F2}");

                    total_recaudado += diccionarios.monto[i];
                }
            }
            if (!hay_resultados && opc == "2")
            {
                Console.WriteLine($"\n[Aviso] No se encontraron ventas para la fecha: {fecha_filtro}");
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine($"TOTAL RECAUDADO: S/ {total_recaudado:F2}");
        }
        static void Main(string[] args)
        {
        }
    }
}