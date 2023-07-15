# Facturador Electronico Demo
Proyecto para consumo de los servicios de facturación electrónica usando el nuget DGIIFacturaElectronica.

## Nuget usado para este proyecto
https://www.nuget.org/packages/DGIIFacturaElectronica/

# DGII Facturación Electrónica - Comprobantes Fiscales Eelctronicos - DGII
Librería de uso común para hacer que los sistemas actuales se conviertan muy facilmente en emisores electrónicos de DGII consumiendo los servicios en la nube para esos fines.
## Requisitos
Para usar esta librería es necesario contar con un certificado digital personal para fines tributarios, nombre de usuario y una contraseña que puedes obtener gratis en el sitio web del sistema, 
estos pasos son necesarios para obtener las URL's disponibles como son:
1. Autenticación con DGII https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Autenticacion
2. Recepción de comprobantes https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Recepcion/api/ecf
3. Aprobación comercial https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/AprobacionComercial/api/ecf
*Debes sustituir 'TU_RNC_SIN_ESPACIOS_NI_GUIONES por tu RNC o cédula y configurarlo también en el sistema Menú Inicio/Empresa* [Aquí](https://app.aslan.com.do/empresa)
- [Obtener certificado digital (Viafirma)](https://www.viafirma.do/).
- [Crear nuevo usuario](https://app.aslan.com.do/login/register?promocode=nuget).
## ¿Cómo uso la librería?
Luego de instalar el paquete en un proyecto .NET pasamos a comentarte que esta librería se compone de varios métodos que usaremos para consumir los servicios de facturación Electrónica, estos métodos son:
- **UploadCertificate**
    - Este método recibe dos parámetros de cadena de texto, uno es la ruta física de tu certificado, y el otro es la clave de tu certificado digital, lo usaremos para cargar el certificado la primera vez unicamente.
      Este método se encarga de subir el certificado necesario para realizar las operaciones. Asegúrate de validar y verificar el certificado antes de continuar con las siguientes operaciones.
- **EnviarFactura**
    - Con este método enviaremos las facturas a DGII, recibe un parámetro de la factura en cuestión a enviar.
      Esta función envía una factura a través del servicio. Utiliza la objeto `FacturaElectronica` que contiene la factura a enviar.       
- **ConsultarTrackId**
    - Realiza la consulta del número de seguimiento o TrackId, recibe como parámetros el RNC del emisor y el número de NCF.
     Este método consulta el estado de una factura utilizando el RNC del emisor y el número de comprobante fiscal (NCF).
     Implementa la lógica necesaria para conectarte al servicio o API de consulta correspondiente y realizar la consulta utilizando los parámetros proporcionados.
     Procesa la respuesta y extrae la información necesaria.
- **ConsultarEstatus**
    - Realiza la consulta del estado del documento en los sevidores electrónicos de DGII, recibe el TrackId o número de seguimiento. 
      Esta función consulta el estado de una factura utilizando un ID de seguimiento proporcionado.
      Procesa la respuesta y extrae la información necesaria.
- **ConsultarTimbre**
    - Realiza la consulta para generar el código QR de consultar el documento en los sevidores de DGII, recibe como parámetros fecha del documento, RNC de comprador y el número de comprobante electrónico.
- **EnviarNCFAComprador**
    - Envia el comprobante al comprador, recibe como parámetros la factura, URl de Autenticación y la URl de Recepción.
- **NCFRecibidos**
    - Consulta los NCF recibidos desde nuestros suplidores en un rango de fecha.
- **EnviarAprobacionComercialAComprador**
    - Envia al suplidor una aprobación o rechazo comercial
- **AprobacionesComerciales**
    - Consulta las aprobaciones comerciales recibidas en un rango de fechas.
- **InformacionDeCertificado**
    - Consulta la información relacionada con tu certificado digital.
- **InformacionDelToken**
    - Retorna el objeto `Token` con los valores que usa para los servicios.
- **ConsultarDirectorio**
    - Responsable de retornar el listado de los contribuyentes con la URLs de sus servicios.
Este ejemplo ha sido probado con la versión 4.8 de .NET Framework y NET 6 con su respectiva versión de Visual Studio.
```csharp
//Declaramos las variables
private static readonly string usuario = "NombreDeUsuario";
private static readonly string clave = "ClaveSecreta";
private static readonly string uRlAutenticacion = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Autenticacion";
private static readonly string uRlRecepcion = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Recepcion/api/ecf";
private static readonly string uRlAprobacionComercial = "https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/AprobacionComercial/api/ecf";
private static readonly string rncEmisor = "TU_RNC";
private static readonly string rncComprador = "RNC_COMPRADOR";
private static readonly string ncf = "E-COMPROBANTE";
private static readonly string fechaEmision = "DD-MM-YYYY";
```
```csharp
//Instanciamos la clase para consumir los métodos
DocumentosElectronicos documentosElectronicos = new DocumentosElectronicos(usuario, clave, Ambientes.TesteCF);
```
```csharp
//Subimos el certificado digital (solo la primera vez, luego ya no es necesario.)
documentosElectronicos.UploadCertificate("RUTA COMPLETA DE TU CERTIFICADO DIGITAL", "CLAVE DEL CERTIFICADO");
```
```csharp
//Preparamos el documento a enviar
List<Detalle> detalles = new List<Detalle>
{
    new Detalle(1, 100, "Producto test 1", "11", 10),
    new Detalle(1, 200, "Producto test 2", "12", 10)
};
FacturaElectronica facturaElectronica = new FacturaElectronica(detalles.ToArray(), ncf, "31-12-2025", TipoDeIngresos.Ingresos_por_operaciones_No_financieros, rncEmisor, "TU RAZON SOCIAL", "Mi dirección", fechaEmision, rncComprador, "NOMBRE DEL CLIENTE", TipoDePago.Contado, IndicadorMontoGravado.Con_ITBIS_INCLUIDO, ProvinciaMunicipio.MUNICIPIO_SANTO_DOMINGO_DE_GUZMAN_010100, ProvinciaMunicipio.MUNICIPIO_SANTO_DOMINGO_DE_GUZMAN_010100, ProvinciaMunicipio.DISTRITO_NACIONAL_010000, ProvinciaMunicipio.DISTRITO_NACIONAL_010000);

//Enviamos el documento o factura
documentosElectronicos.EnviarFactura(facturaElectronica);
```
```csharp
//Consultamos el número de seguimiento de cada en envio del documento
List<TrackId> resultado = await documentosElectronicos.ConsultarTrackId(rncEmisor, eNcf);
foreach (var item in resultado)
    Console.WriteLine($"TrackId: {item.trackId}\nMensaje: {item.mensaje}\nEstado: {item.estado}\nFecha Recepcion: {item.fechaRecepcion}");
```
```csharp
//Consultamos el estatus mediante su número de seguimiento o TrackId
string resultado = await documentosElectronicos.ConsultarEstatus(trackId);
```
```csharp
//Consultamo el Timbre para generar el código QR
Timbre resultado = await documentosElectronicos.ConsultarTimbre(fecha, rnc, ncf);
Console.WriteLine($"Fecha de Firma: {resultado.fechaFirma}\nfechaEmision: {resultado.fechaEmision}\nencf: {resultado.encf}\nRNC Comprador: {resultado.rncComprador}\nCódigo de Seguridad: {resultado.codigoSeguridad}\nMonto Total: {resultado.montoTotal}\nRrl Image: {resultado.urlImage}\nRNC Emisor: {resultado.rncEmisor}");
```
```csharp
//Enviamos la Factura Al Comprador
ARECF resultado = await documentosElectronicos.EnviarNCFAComprador(facturaElectronica, uRlAutenticacion, uRlRecepcion);
Console.WriteLine($"Detalle de Acuse de Recibo: {resultado.detalleAcusedeRecibo}\nAny: {resultado.any}");
```
```csharp
//Consultamos los NCF Recibidos
List<eNCFRecibido> resultado = await documentosElectronicos.NCFRecibidos(fecha1, fecha2);
foreach (var item in resultado)
{
    Console.WriteLine($"Cédula: {item.cedula}\n{item.nombre_Completo}\nFecha: {item.fecha}\nTotal: {item.total}\nSubtotal: {item.subtotal}\nSecuencia_Gobierno: {item.secuencia_Gobierno}\nNombre_Completo: {item.nombre_Completo}\nImpuestos: {item.impuestos}");
    Console.WriteLine("\n");
}
```
```csharp
//Enviar Aprobacion Comercial Al Comprador
string resultado = await documentosElectronicos.EnviarAprobacionComercialAComprador(uRlAprobacionComercial, uRlAutenticacion, rncEmisor, rncComprador, ncf, fechaEmision, montoTotal, aprobado, comentarios);
Console.WriteLine(resultado);
```
```csharp
//Consultamos las Aprobaciones Comerciales recibidas
List<AprobacionComercial> resultado = await documentosElectronicos.AprobacionesComerciales(fecha1, fecha2);
foreach (var item in resultado)
    Console.WriteLine(item.rncComprador);
```
## Todo el código junto a continuación en un proyecto de consola en C#
```csharp
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
    documentosElectronicos = new DocumentosElectronicos(usuario, clave, url);
    
    //Para el manejo de las excepciones, nos suscribimos en el evento `Excepcion` para capturar los errores de forma centralizada.
    documentosElectronicos.Excepcion += DocumentosElectronicos_Excepcion;
    List<Detalle> detalles = new List<Detalle>
    {
        new Detalle(1, 100, "Producto test 1", "11", 10),
        new Detalle(1, 200, "Producto test 2", "12", 10)
    };
    FacturaElectronica facturaElectronica = new FacturaElectronica(detalles.ToArray(), ncf, "31-12-2025", "01", rncEmisor, "TU RAZON SOCIAL", "TU DIRECCION", fechaEmision, rncComprador, "NOMBRE DEL CLIENTE", 1, 1, "010100", "010100", "010000", "010000");
    _ = SubirCertificado();
    _ = EnviarFactura(facturaElectronica);
    _ = ConsultarTrackId(rncEmisor, ncf);
    _ = ConsultarEstatus("954e3d5c-1cc8-4726-84a7-3ceec1599a2b");
    _ = ConsultarTimbre(fechaEmision, rncEmisor, ncf);
    _ = EnviarFacturaAComprador(facturaElectronica, uRlAutenticacion, uRlRecepcion);
    _ = NCFRecibidos(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
    _ = EnviarAprobacionComercialAComprador(uRlAprobacionComercial, uRlAutenticacion, rncEmisor, rncComprador, ncf, fechaEmision, 3000.01m, true, "Hemos aprobado el comprobante.");
    _ = AprobacionesComerciales(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
    _ = InformacionDeCertificado();
    _ = InformacionDelToken();
    _ = ConsultarDirectorio();

    //Al conluir el uso del componente, eliminamos la suscripción al evento `Excepcion`.
    documentosElectronicos.Excepcion -= DocumentosElectronicos_Excepcion;
    Console.WriteLine("Listo!");
    Console.ReadLine();
}
private static async Task<Directorio> ConsultarDirectorio()
{
    Directorio Resultado = await documentosElectronicos.ConsultaDirectorio("999999999");

    return Resultado;
}
private static async Task<Token> InformacionDelToken()
{
    Token Resultado = await documentosElectronicos.InformacionDelToken();

    return Resultado;
}
private static async Task<CertInfo> InformacionDeCertificado()
{
    CertInfo Resultado = await documentosElectronicos.InformacionDeCertificado();

    return Resultado;
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
```
Tabla de provincias y municipios utilizados para enviar el objeto FacturaElectronica
|**Codigo**|**Nombre**||
| - | - | - |
|010000|DISTRITO NACIONAL||
|010100|MUNICIPIO SANTO DOMINGO DE GUZMÁN||
|010101|SANTO DOMINGO DE GUZMÁN (D. M.).||
|020000|PROVINCIA AZUA||
|020100|MUNICIPIO AZUA||
|020101|AZUA (D. M.).||
|020102|BARRO ARRIBA (D. M.).||
|020103|LAS BARÍAS-LA ESTANCIA (D. M.).||
|020104|LOS JOVILLOS (D. M.).||
|020105|PUERTO VIEJO (D. M.).||
|020106|BARRERAS (D. M.).||
|020107|DOÑA EMMA BALAGUER VIUDA VALLEJO (D. M.).||
|020108|CLAVELLINA (D. M.).||
|020109|LAS LOMAS (D. M.).||
|020200|MUNICIPIO LAS CHARCAS||
|020201|LAS CHARCAS (D. M.).||
|020202|PALMAR DE OCOA (D. M.).||
|020300|MUNICIPIO LAS YAYAS DE VIAJAMA||
|020301|LAS YAYAS DE VIAJAMA (D. M.).||
|020302|VILLARPANDO (D. M.).||
|020303|HATO NUEVO CORTÉS (D. M.).||
|020400|MUNICIPIO PADRE LAS CASAS||
|020401|PADRE LAS CASAS (D. M.).||
|020402|LAS LAGUNAS (D. M.).||
|020403|LA SIEMBRA (D. M.).||
|020404|MONTE BONITO (D. M.).||
|020405|LOS FRÍOS (D. M.).||
|020500|MUNICIPIO PERALTA||
|020501|PERALTA (D. M.).||
|020600|MUNICIPIO SABANA YEGUA||
|020601|SABANA YEGUA (D. M.).||
|020602|PROYECTO 4 (D. M.).||
|020603|GANADERO (D. M.).||
|020604|PROYECTO 2-C (D. M.).||
|020700|MUNICIPIO PUEBLO VIEJO||
|020701|PUEBLO VIEJO (D. M.).||
|020702|EL ROSARIO (D. M.).||
|020800|MUNICIPIO TÁBARA ARRIBA||
|020801|TÁBARA ARRIBA (D. M.).||
|020802|TÁBARA ABAJO (D. M.).||
|020803|AMIAMA GÓMEZ (D. M.).||
|020804|LOS TOROS (D. M.).||
|020900|MUNICIPIO GUAYABAL||
|020901|GUAYABAL (D. M.).||
|021000|MUNICIPIO ESTEBANÍA||
|021001|ESTEBANÍA (D. M.).||
|030000|PROVINCIA BAHORUCO||
|030001|MUNICIPIO NEIBA||
|030101|NEIBA (D. M.).||
|030102|EL PALMAR  (D. M.).||
|030200|MUNICIPIO GALVÁN||
|030201|GALVÁN (D. M.).||
|030202|EL SALADO (D. M.).||
|030300|MUNICIPIO TAMAYO||
|030301|TAMAYO (D. M.).||
|030302|UVILLA (D. M.).||
|030303|SANTANA (D. M.).||
|030304|MONSERRATE (MONTSERRAT) (D. M.).||
|030305|CABEZA DE TORO (D. M.).||
|030306|MENA (D. M.).||
|030307|SANTA BÁRBARA EL 6 (D. M.).||
|030400|MUNICIPIO VILLA JARAGUA||
|030401|VILLA JARAGUA (D. M.).||
|030500|MUNICIPIO LOS RÍOS||
|030501|LOS RÍOS (D. M.).||
|030502|LAS CLAVELLINAS (D. M.).||
|040000|PROVINCIA BARAHONA||
|040100|MUNICIPIO BARAHONA||
|040101|BARAHONA (D. M.).||
|040102|EL CACHÓN (D. M.).||
|040103|LA GUÁZARA (D. M.).||
|040104|VILLA CENTRAL (D. M.).||
|040200|MUNICIPIO CABRAL||
|040201|CABRAL (D. M.).||
|040300|MUNICIPIO ENRIQUILLO||
|040301|ENRIQUILLO (D. M.).||
|040302|ARROYO DULCE (D. M.).||
|040400|MUNICIPIO PARAÍSO||
|040401|PARAÍSO (D. M.).||
|040402|LOS PATOS (D. M.).||
|040500|MUNICIPIO VICENTE NOBLE||
|040501|VICENTE NOBLE (D. M.).||
|040502|CANOA (D. M.).||
|040503|QUITA CORAZA (D. M.).||
|040504|FONDO NEGRO (D. M.).||
|040600|MUNICIPIO EL PEÑÓN||
|040601|EL PEÑÓN (D. M.).||
|040700|MUNICIPIO LA CIÉNAGA||
|040701|LA CIÉNAGA (D. M.).||
|040702|BAHORUCO (D. M.).||
|040800|MUNICIPIO FUNDACIÓN||
|040801|FUNDACIÓN (D. M.).||
|040802|PESCADERÍA (D. M.).||
|040900|MUNICIPIO LAS SALINAS||
|040901|LAS SALINAS (D. M.).||
|041000|MUNICIPIO POLO||
|041001|POLO (D. M.).||
|041100|MUNICIPIO JAQUIMEYES||
|041101|JAQUIMEYES (D. M.).||
|041102|PALO ALTO (D. M.).||
|050000|PROVINCIA DAJABÓN||
|050100|MUNICIPIO DAJABÓN||
|050101|DAJABÓN (D. M.).||
|050102|CAÑONGO (D. M.).||
|050200|MUNICIPIO LOMA DE CABRERA||
|050201|LOMA DE CABRERA (D. M.).||
|050202|CAPOTILLO (D. M.).||
|050203|SANTIAGO DE LA CRUZ (D. M.).||
|050300|MUNICIPIO PARTIDO||
|050301|PARTIDO (D. M.).||
|050400|MUNICIPIO RESTAURACIÓN||
|050401|RESTAURACIÓN (D. M.).||
|050500|MUNICIPIO EL PINO||
|050501|EL PINO (D. M.).||
|050502|MANUEL BUENO (D. M.).||
|060000|PROVINCIA DUARTE||
|060100|MUNICIPIO SAN FRANCISCO DE MACORÍS||
|060101|SAN FRANCISCO DE MACORÍS (D. M.).||
|060102|LA PEÑA (D. M.).||
|060103|CENOVÍ (D. M.).||
|060104|JAYA (D. M.).||
|060105|PRESIDENTE DON ANTONIO GUZMÁN FERNÁNDEZ (D. M.).|
|060200|MUNICIPIO ARENOSO||
|060201|ARENOSO (D. M.).||
|060202|LAS COLES (D. M.).||
|060203|EL AGUACATE (D. M.).||
|060300|MUNICIPIO CASTILLO||
|060301|CASTILLO (D. M.).||
|060400|MUNICIPIO PIMENTEL||
|060401|PIMENTEL (D. M.).||
|060500|MUNICIPIO VILLA RIVA||
|060501|VILLA RIVA (D. M.).||
|060502|AGUA SANTA DEL YUNA  (D. M.).||
|060503|CRISTO REY DE GUARAGUAO (D. M.).||
|060504|LAS TARANAS (D. M.).||
|060505|BARRAQUITO (D. M.).||
|060600|MUNICIPIO LAS GUÁRANAS||
|060601|LAS GUÁRANAS (D. M.).||
|060700|MUNICIPIO EUGENIO MARÍA DE HOSTOS||
|060701|EUGENIO MARÍA DE HOSTOS (D. M.).||
|060702|SABANA GRANDE (D. M.).||
|070000|PROVINCIA ELÍAS PIÑA||
|070100|MUNICIPIO COMENDADOR||
|070101|COMENDADOR (D. M.).||
|070102|SABANA LARGA (D. M.).||
|070103|GUAYABO  (D. M.).||
|070200|MUNICIPIO BÁNICA||
|070201|BÁNICA (D. M.).||
|070202|SABANA CRUZ (D. M.).||
|070203|SABANA HIGÜERO (D. M.).||
|070300|MUNICIPIO EL LLANO||
|070301|EL LLANO (D. M.).||
|070302|GUANITO (D. M.).||
|070400|MUNICIPIO HONDO VALLE||
|070401|HONDO VALLE (D. M.).||
|070402|RANCHO DE LA GUARDIA (D. M.).||
|070500|MUNICIPIO PEDRO SANTANA||
|070501|PEDRO SANTANA (D. M.).||
|070502|RÍO LIMPIO (D. M.).||
|070600|MUNICIPIO JUAN SANTIAGO||
|070601|JUAN SANTIAGO (D. M.).||
|080000|PROVINCIA EL SEIBO||
|080100|MUNICIPIO EL SEIBO||
|080101|EL SEIBO (D. M.).||
|080102|PEDRO SÁNCHEZ (D. M.).||
|080103|SAN FRANCISCO-VICENTILLO (D. M.).||
|080104|SANTA LUCÍA (D. M.).||
|080200|MUNICIPIO MICHES||
|080201|MICHES (D. M.).||
|080202|EL CEDRO (D. M.).||
|080203|LA GINA (D. M.).||
|090000|PROVINCIA ESPAILLAT||
|090100|MUNICIPIO MOCA||
|090101|MOCA (D. M.).||
|090102|JOSÉ CONTRERAS (D. M.).||
|090103|SAN VÍCTOR (D. M.).||
|090104|JUAN LÓPEZ (D. M.).||
|090105|LAS LAGUNAS (D. M.).||
|090106|CANCA LA REYNA  (D. M.).||
|090107|EL HIGÜERITO (D. M.).||
|090108|MONTE DE LA JAGUA (D. M.).||
|090109|LA ORTEGA (D. M.).||
|090200|MUNICIPIO CAYETANO GERMOSÉN||
|090201|CAYETANO GERMOSÉN (D. M.).||
|090300|MUNICIPIO GASPAR HERNÁNDEZ||
|090301|GASPAR HERNÁNDEZ (D. M.).||
|090302|JOBA ARRIBA (D. M.).||
|090303|VERAGUA (D. M.).||
|090304|VILLA MAGANTE (D. M.).||
|090400|MUNICIPIO JAMAO AL NORTE||
|090401|JAMAO AL NORTE (D. M.).||
|100000|PROVINCIA INDEPENDENCIA||
|100100|MUNICIPIO JIMANÍ||
|100101|JIMANÍ (D. M.).||
|100102|EL LIMÓN (D. M.).||
|100103|BOCA DE CACHÓN (D. M.).||
|100200|MUNICIPIO DUVERGÉ||
|100201|DUVERGÉ (D. M.).||
|100202|VENGAN A VER (D. M.).||
|100300|MUNICIPIO LA DESCUBIERTA||
|100301|LA DESCUBIERTA (D. M.).||
|100400|MUNICIPIO POSTRER RÍO||
|100401|POSTRER RÍO (D. M.).||
