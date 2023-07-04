using DGIIFacturaElectronica;
using DGIIFacturaElectronica.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturadorElectronicoDemo
{
    internal class Program
    {
        private static readonly string usuario = "NombreDeUsuario";
        private static readonly string clave = "ClaveSecreta";
        private static readonly string url = "https://api.aslan.com.do/TesteCF/";
        private static readonly string uRlAutenticacion = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Autenticacion";
        private static readonly string uRlRecepcion = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Recepcion/api/ecf";
        private static readonly string uRlAprobacionComercial = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/AprobacionComercial/api/ecf";
        private static readonly string rncEmisor = "TU_RNC";
        private static readonly string rncComprador = "RNC_COMPRADOR";
        private static readonly string ncf = "E-COMPROBANTE";
        private static readonly string fechaEmision = "DD-MM-YYYY";
        private static DocumentosElectronicos documentosElectronicos;
        static void Main(string[] args)
        {
            documentosElectronicos = new DocumentosElectronicos(usuario, clave, Ambientes.TesteCF);

            //Para el manejo de las excepciones, nos suscribimos en el evento `Excepcion` para capturar los errores de forma centralizada.
            documentosElectronicos.Excepcion += DocumentosElectronicos_Excepcion;

            List<Detalle> detalles = new List<Detalle>
            {
                new Detalle(1, 100, "Producto test 1", "11", 10),
                new Detalle(1, 200, "Producto test 2", "12", 10)
            };

            try
            {
                FacturaElectronica facturaElectronica = new FacturaElectronica(detalles.ToArray(), ncf, "31-12-2025", TipoDeIngresos.Ingresos_por_operaciones_No_financieros, rncEmisor, "TU RAZON SOCIAL", "TU DIRECCION", fechaEmision, rncComprador, "NOMBRE DEL CLIENTE", TipoDePago.Contado, IndicadorMontoGravado.Con_ITBIS_INCLUIDO, ProvinciaMunicipio.MUNICIPIO_SANTO_DOMINGO_DE_GUZMAN_010100, ProvinciaMunicipio.MUNICIPIO_SANTO_DOMINGO_DE_GUZMAN_010100, ProvinciaMunicipio.DISTRITO_NACIONAL_010000, ProvinciaMunicipio.DISTRITO_NACIONAL_010000);

                _ = SubirCertificado();
                _ = EnviarFactura(facturaElectronica);
                _ = ConsultarTrackId(rncEmisor, ncf);
                _ = ConsultarEstatus("954e3d5c-1cc8-4726-84a7-3ceec1599a2b");
                _ = ConsultarTimbre(fechaEmision, rncEmisor, ncf);

                _ = EnviarFacturaAComprador(facturaElectronica, uRlAutenticacion, uRlRecepcion);
                _ = NCFRecibidos(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
                _ = EnviarAprobacionComercialAComprador(uRlAprobacionComercial, uRlAutenticacion, rncEmisor, rncComprador, ncf, fechaEmision, 3000.01m, true, "Hemos aprobado el comprobante.");
                _ = AprobacionesComerciales(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Al conluir el uso del componente, eliminamos la suscripción al evento.
            documentosElectronicos.Excepcion -= DocumentosElectronicos_Excepcion;

            Console.WriteLine("Listo!");
            Console.ReadLine();
        }

        private static void DocumentosElectronicos_Excepcion(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private static async Task<string> SubirCertificado()
        {
            string resultado = await documentosElectronicos.UploadCertificate("RUTA COMPLETA DE TU CERTIFICADO DIGITAL", "CLAVE DEL CERTIFICADO");

            Console.WriteLine(resultado);
            return resultado;
        }

        private static async Task<string> EnviarFactura(FacturaElectronica facturaElectronica)
        {
            string resultado = await documentosElectronicos.EnviarFactura(facturaElectronica);

            Console.WriteLine(resultado);
            return resultado;
        }

        private static async Task<List<TrackId>> ConsultarTrackId(string rncEmisor, string eNcf)
        {
            List<TrackId> resultado = await documentosElectronicos.ConsultarTrackId(rncEmisor, eNcf);

            foreach (var item in resultado)
                Console.WriteLine($"TrackId: {item.trackId}\nMensaje: {item.mensaje}\nEstado: {item.estado}\nFecha Recepcion: {item.fechaRecepcion}");

            return resultado;
        }

        private static async Task<string> ConsultarEstatus(string trackId)
        {
            string resultado = await documentosElectronicos.ConsultarEstatus(trackId);

            Console.WriteLine(resultado);

            return resultado;
        }

        private static async Task<Timbre> ConsultarTimbre(string fecha, string rnc, string ncf)
        {
            Timbre resultado = await documentosElectronicos.ConsultarTimbre(fecha, rnc, ncf);

            Console.WriteLine($"Fecha de Firma: {resultado.fechaFirma}\nfechaEmision: {resultado.fechaEmision}\nencf: {resultado.encf}\nRNC Comprador: {resultado.rncComprador}\nCódigo de Seguridad: {resultado.codigoSeguridad}\nMonto Total: {resultado.montoTotal}\nRrl Image: {resultado.urlImage}\nRNC Emisor: {resultado.rncEmisor}");

            return resultado;
        }

        private static async Task<ARECF> EnviarFacturaAComprador(FacturaElectronica facturaElectronica, string uRlAutenticacion, string uRlRecepcion)
        {
            ARECF resultado = await documentosElectronicos.EnviarNCFAComprador(facturaElectronica, uRlAutenticacion, uRlRecepcion);

            Console.WriteLine($"Detalle de Acuse de Recibo: {resultado.detalleAcusedeRecibo}\nAny: {resultado.any}");
            return resultado;
        }

        private static async Task<List<eNCFRecibido>> NCFRecibidos(DateTime fecha1, DateTime fecha2)
        {
            List<eNCFRecibido> resultado = await documentosElectronicos.NCFRecibidos(fecha1, fecha2);

            foreach (var item in resultado)
            {
                Console.WriteLine($"Cédula: {item.cedula}\n{item.nombre_Completo}\nFecha: {item.fecha}\nTotal: {item.total}\nSubtotal: {item.subtotal}\nSecuencia_Gobierno: {item.secuencia_Gobierno}\nNombre_Completo: {item.nombre_Completo}\nImpuestos: {item.impuestos}");
                Console.WriteLine("\n");
            }

            return resultado;
        }

        private static async Task<string> EnviarAprobacionComercialAComprador(string uRlAprobacionComercial, string uRlAutenticacion, string rncEmisor, string rncComprador, string ncf, string fechaEmision, decimal montoTotal, bool aprobado, string comentarios)
        {
            string resultado = await documentosElectronicos.EnviarAprobacionComercialAComprador(uRlAprobacionComercial, uRlAutenticacion, rncEmisor, rncComprador, ncf, fechaEmision, montoTotal, aprobado, comentarios);

            Console.WriteLine(resultado);
            return resultado;
        }

        private static async Task<List<AprobacionComercial>> AprobacionesComerciales(DateTime fecha1, DateTime fecha2)
        {
            List<AprobacionComercial> resultado = await documentosElectronicos.AprobacionesComerciales(fecha1, fecha2);

            foreach (var item in resultado)
                Console.WriteLine(item.rncComprador);
            return resultado;
        }
    }
}
