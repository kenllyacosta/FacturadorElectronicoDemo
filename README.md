# Facturador Electronico Demo
Proyecto para consumo de los servicios de facturación electrónica usando el nuget DGIIFacturaElectronica.

## Nuget usado para este proyecto
https://www.nuget.org/packages/DGIIFacturaElectronica/


# DGII Factura Electronica - Comprobantes Fiscales Eelctronicos - DGII

Librería de uso común para hacer que los sistemas actuales se conviertan muy facilmente en emisores electrónicos de DGII consumiendo los servicios en la nube para esos fines.

## Requisitos

Para usar esta librería es necesario contar con un certificado digital personal para fines tributarios, nombre de usuario y una contraseña que puedes obtener gratis en el sitio web del sistema, 
estos pasos son necesarios para obtener las URL's disponibles como son:

1. Autenticación con DGII https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Autenticacion
2. Recepción de comprobantes https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/Recepcion/api/ecf
3. Aprobación comercial https://api.aslan.com.do/ecf/TU_RNC_SIN_ESPACIOS_NI_GUIONES/fe/AprobacionComercial/api/ecf

*Debes sustituir 'TU_RNC_SIN_ESPACIOS_NI_GUIONES por tu RNC o cédula y configurarlo también en el sistema Menú Inicio/Empresa* [Aquí](https://app.aslan.com.do/empresa)

- [Obtener certificado digital (Viafirma)](https://www.viafirma.do/).
- [Crear nuevo usuario](https://app.aslan.com.do/login/register).

## ¿Cómo uso la librería?

Luego de instalar el paquete en un proyecto .NET pasamoa a comentarte que esta librería se compone de varios métodos que usaremos para consumir los servicios de facturación Electrónica, estos métodos son:
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

Este ejemplo requiere la versión 4.8 de .NET Framework y su respectivo Visual Studio.

```csharp
//Declaramos las variables
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
```

```csharp
//Instanciamos la clase para consumir los métodos
DocumentosElectronicos documentosElectronicos = new DocumentosElectronicos(usuario, clave, url);
```

```csharp
//Subimos el certificado digital
documentosElectronicos.UploadCertificate("RUTA COMPLETA DE TU CERTIFICADO DIGITAL", "CLAVE DEL CERTIFICADO");
```

```csharp
//Preparamos el documento a enviar
List<Detalle> detalles = new List<Detalle>
{
    new Detalle(1, 100, "Producto test 1", "11", 10),
    new Detalle(1, 200, "Producto test 2", "12", 10)
};
FacturaElectronica facturaElectronica = new FacturaElectronica(detalles.ToArray(), ncf, "31-12-2025", "01", rncEmisor, "TU RAZON SOCIAL", "TU DIRECCION", fechaEmision, rncComprador, "NOMBRE DEL CLIENTE", 1, 1, "010100", "010100", "010000", "010000");

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
| - | - | - |
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
| - | - | - |
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
| - | - | - |
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
| - | - | - |
|100200|MUNICIPIO DUVERGÉ||
|100201|DUVERGÉ (D. M.).||
|100202|VENGAN A VER (D. M.).||
|100300|MUNICIPIO LA DESCUBIERTA||
|100301|LA DESCUBIERTA (D. M.).||
|100400|MUNICIPIO POSTRER RÍO||
|100401|POSTRER RÍO (D. M.).||
|100402|GUAYABAL (D. M.).||
|100500|MUNICIPIO CRISTÓBAL||
|100501|CRISTÓBAL (D. M.).||
|100502|BATEY 8 (D. M.).||
|100600|MUNICIPIO MELLA||
|100601|MELLA (D. M.).||
|100602|LA COLONIA (D. M.).||
|110000|PROVINCIA LA ALTAGRACIA||
|110100|MUNICIPIO HIGÜEY||
|110101|HIGÜEY (D. M.).||
|110102|LAS LAGUNAS DE NISIBÓN (D. M.).||
|110103|LA OTRA BANDA (D. M.).||
|110104|VERÓN PUNTA CANA (D. M.) (Incluye Bávaro)||
|110200|MUNICIPIO SAN RAFAEL DEL YUMA||
|110201|SAN RAFAEL DEL YUMA (D. M.).||
|110202|BOCA DE YUMA (D. M.).||
|110203|BAYAHÍBE (D. M.).||
|120000|PROVINCIA LA ROMANA||
|120100|MUNICIPIO LA ROMANA||
|120101|LA ROMANA (D. M.).||
|120102|CALETA (D. M.).||
|120200|MUNICIPIO GUAYMATE||
|120201|GUAYMATE (D. M.).||
|120300|MUNICIPIO VILLA HERMOSA||
|120301|VILLA HERMOSA (D. M.).||
|120302|CUMAYASA (D. M.).||
|130000|PROVINCIA LA VEGA||
|130100|MUNICIPIO LA VEGA||
|130101|LA VEGA (D. M.).||
|130102|RÍO VERDE ARRIBA (D. M.).||
|130103|EL RANCHITO (D. M.).||
|130104|TAVERAS (D. M.).||
|130105|DON JUAN RODRÍGUEZ (D.M.)||
|130200|MUNICIPIO CONSTANZA||
|130201|CONSTANZA (D. M.).||
|130202|TIREO (D. M.).||
|130203|LA SABINA (D. M.).||
|130300|MUNICIPIO JARABACOA||
|130301|JARABACOA (D. M.).||
|130302|BUENA VISTA (D. M.).||
|130303|MANABAO (D. M.).||


|130400|MUNICIPIO JIMA ABAJO||
| - | - | - |
|130401|JIMA ABAJO (D. M.).||
|130402|RINCÓN (D. M.).||
|140000|PROVINCIA MARÍA TRINIDAD SÁNCHEZ||
|140100|MUNICIPIO NAGUA||
|140101|NAGUA (D. M.).||
|140102|SAN JOSÉ DE MATANZAS (D. M.).||
|140103|LAS GORDAS (D. M.).||
|140104|ARROYO AL MEDIO (D. M.).||
|140200|MUNICIPIO CABRERA||
|140201|CABRERA (D. M.).||
|140202|ARROYO SALADO (D. M.).||
|140203|LA ENTRADA (D. M.).||
|140300|MUNICIPIO EL FACTOR||
|140301|EL FACTOR (D. M.).||
|140302|EL POZO (D. M.).||
|140400|MUNICIPIO RÍO SAN JUAN||
|140401|RÍO SAN JUAN (D. M.).||
|150000|PROVINCIA MONTE CRISTI||
|150100|MUNICIPIO MONTE CRISTI||
|150101|MONTE CRISTI (D. M.).||
|150200|MUNICIPIO CASTAÑUELAS||
|150201|CASTAÑUELAS (D. M.).||
|150202|PALO VERDE (D. M.).||
|150300|MUNICIPIO GUAYUBÍN||
|150301|GUAYUBÍN (D. M.).||
|150302|VILLA ELISA (D. M.).||
|150303|HATILLO PALMA (D. M.).||
|150304|CANA CHAPETÓN (D. M.).||
|150400|MUNICIPIO LAS MATAS DE SANTA CRUZ||
|150401|LAS MATAS DE SANTA CRUZ (D. M.).||
|150500|MUNICIPIO PEPILLO SALCEDO||
|150501|PEPILLO SALCEDO (MANZANILLO)||
|150502|SANTA MARÍA (D. M.)||
|150600|MUNICIPIO VILLA VÁSQUEZ||
|150601|VILLA VÁSQUEZ||
|160000|PROVINCIA PEDERNALES||
|160100|MUNICIPIO PEDERNALES||
|160101|PEDERNALES||
|160102|JOSÉ FRANCISCO PEÑA GÓMEZ (D. M.).||
|160200|MUNICIPIO OVIEDO||
|160201|OVIEDO||
|160202|JUANCHO (D. M.).||
|170000|PROVINCIA PERAVIA||
|170100|MUNICIPIO BANÍ||
|170101|BANÍ (D. M.).||
|170102|MATANZAS (D. M.).||
|170103|VILLA FUNDACIÓN (D. M.).||
|170104|SABANA BUEY (D. M.).||


|170105|PAYA (D. M.).||
| - | - | - |
|170106|VILLA SOMBRERO (D. M.).||
|170107|EL CARRETÓN (D. M.).||
|170108|CATALINA (D. M.).||
|170109|EL LIMONAL (D. M.).||
|170110|LAS BARÍAS (D. M.).||
|170200|MUNICIPIO NIZAO||
|170201|NIZAO||
|170202|PIZARRETE (D. M.).||
|170203|SANTANA (D. M.).||
|170300|MATANZAS||
|170301|MATANZAS||
|180000|PROVINCIA PUERTO PLATA||
|180100|MUNICIPIO PUERTO PLATA||
|180101|PUERTO PLATA (D. M.).||
|180102|YÁSICA ARRIBA (D. M.).||
|180103|MAIMÓN (D. M.).||
|180200|MUNICIPIO ALTAMIRA||
|180201|ALTAMIRA||
|180202|RÍO GRANDE (D. M.).||
|180300|MUNICIPIO GUANANICO||
|180301|GUANANICO||
|180400|MUNICIPIO IMBERT||
|180401|IMBERT||
|180500|MUNICIPIO LOS HIDALGOS||
|180501|LOS HIDALGOS||
|180502|NAVAS (D. M.).||
|180600|MUNICIPIO LUPERÓN||
|180601|LUPERÓN||
|180602|LA ISABELA (D. M.).||
|180603|BELLOSO (D. M.).||
|180604|EL ESTRECHO DE LUPERÓN OMAR BROSS (D. M.).||
|180700|MUNICIPIO SOSÚA||
|180701|SOSÚA||
|180702|CABARETE (D. M.).||
|180703|SABANETA DE YÁSICA (D. M.).||
|180800|MUNICIPIO VILLA ISABELA||
|180801|VILLA ISABELA||
|180802|ESTERO HONDO (D. M.).||
|180803|LA JAIBA (D. M.).||
|180804|GUALETE (D. M.).||
|180900|MUNICIPIO VILLA MONTELLANO||
|180901|VILLA MONTELLANO||
|190000|PROVINCIA HERMANAS MIRABAL||
|190100|MUNICIPIO SALCEDO||
|190101|SALCEDO||
|190102|JAMAO AFUERA (D. M.).||
|190200|MUNICIPIO TENARES||
|190201|TENARES||


|190202|BLANCO (D. M.).||
| - | - | - |
|190300|MUNICIPIO VILLA TAPIA||
|190301|VILLA TAPIA||
|200000|PROVINCIA SAMANÁ||
|200100|MUNICIPIO SAMANÁ||
|200101|SAMANÁ||
|200102|EL LIMÓN  (D. M.).||
|200103|ARROYO BARRIL (D. M.).||
|200104|LAS GALERAS (D. M.).||
|200200|MUNICIPIO SÁNCHEZ||
|200201|SÁNCHEZ (D. M.).||
|200300|MUNICIPIO LAS TERRENAS||
|200301|LAS TERRENAS||
|210000|PROVINCIA SAN CRISTÓBAL||
|210100|MUNICIPIO SAN CRISTÓBAL||
|210101|SAN CRISTÓBAL (D. M.).||
|210102|HATO DAMAS (D. M.).||
|210103|HATILLO (D. M.).||
|210200|MUNICIPIO SABANA GRANDE DE PALENQUE||
|210201|SABANA GRANDE DE PALENQUE (D. M.).||
|210300|MUNICIPIO BAJOS DE HAINA||
|210301|BAJOS DE HAINA||
|210302|EL CARRIL (D. M.).||
|210303|QUITA SUEÑO (D. M.).||
|210400|MUNICIPIO CAMBITA GARABITOS||
|210401|CAMBITA GARABITOS||
|210402|CAMBITA EL PUEBLECITO (D. M.).||
|210500|MUNICIPIO VILLA ALTAGRACIA||
|210501|VILLA ALTAGRACIA||
|210502|SAN JOSÉ DEL PUERTO (D. M.).||
|210503|MEDINA (D. M.).||
|210504|LA CUCHILLA (D. M.).||
|210600|MUNICIPIO YAGUATE||
|210601|YAGUATE (D. M.).||
|210602|DOÑA ANA (D. M.)||
|210700|MUNICIPIO SAN GREGORIO DE NIGUA||
|210701|SAN GREGORIO DE NIGUA||
|210800|MUNICIPIO LOS CACAOS||
|210801|LOS CACAOS (D. M.).||
|220000|PROVINCIA SAN JUAN||
|220100|MUNICIPIO SAN JUAN||
|220101|SAN JUAN||
|220102|PEDRO CORTO (D. M.).||
|220103|SABANETA (D. M.).||
|220104|SABANA ALTA (D. M.).||
|220105|EL ROSARIO (D. M.).||
|220106|HATO DEL PADRE (D. M.).||
|220107|GUANITO (D. M.).||
|220108|LA JAGUA (D. M.).||


|220109|LAS MAGUANAS-HATO NUEVO (D. M.).||
| - | - | - |
|220110|LAS CHARCAS DE MARÍA NOVA (D. M.).||
|220111|LAS ZANJAS (D. M.)||
|220200|MUNICIPIO BOHECHÍO||
|220201|BOHECHÍO||
|220202|ARROYO CANO (D. M.).||
|220203|YAQUE (D. M.).||
|220300|MUNICIPIO EL CERCADO||
|220301|EL CERCADO||
|220302|DERRUMBADERO (D. M.)||
|220303|BATISTA (D. M.)||
|220400|MUNICIPIO JUAN DE HERRERA||
|220401|JUAN DE HERRERA||
|220402|JÍNOVA (D. M.).||
|220500|MUNICIPIO LAS MATAS DE FARFÁN||
|220501|LAS MATAS DE FARFÁN||
|220502|MATAYAYA (D. M.).||
|220503|CARRERA DE YEGUAS (D. M.).||
|220600|MUNICIPIO VALLEJUELO||
|220601|VALLEJUELO||
|220602|JORJILLO (D. M.).||
|230000|PROVINCIA SAN PEDRO DE MACORÍS||
|230100|MUNICIPIO SAN PEDRO DE MACORÍS||
|230101|SAN PEDRO DE MACORÍS||
|230200|MUNICIPIO LOS LLANOS||
|230201|LOS LLANOS||
|230202|EL PUERTO (D. M.).||
|230203|GAUTIER (D. M.).||
|230300|MUNICIPIO RAMÓN SANTANA||
|230301|RAMÓN SANTANA||
|230400|MUNICIPIO CONSUELO||
|230401|CONSUELO||
|230500|MUNICIPIO QUISQUEYA||
|230501|QUISQUEYA||
|230600|MUNICIPIO GUAYACANES||
|230601|GUAYACANES||
|240000|PROVINCIA SANCHEZ RAMÍREZ||
|240100|MUNICIPIO COTUÍ||
|240101|COTUÍ||
|240102|QUITA SUEÑO (D. M.).||
|240103|CABALLERO (D. M.).||
|240104|COMEDERO ARRIBA (D. M.).||
|240105|PLATANAL  (D. M.).||
|240106|ZAMBRANA ABAJO||
|240200|MUNICIPIO CEVICOS||
|240201|CEVICOS||
|240202|LA CUEVA (D. M.).||
|240300|MUNICIPIO FANTINO||
|240301|FANTINO||


|240400|MUNICIPIO LA MATA||
| - | - | - |
|240401|LA MATA||
|240402|LA BIJA (D. M.).||
|240403|ANGELINA (D. M.).||
|240404|HERNANDO ALONZO (D. M.).||
|250000|PROVINCIA SANTIAGO||
|250100|MUNICIPIO SANTIAGO||
|250101|SANTIAGO||
|250102|PEDRO GARCÍA (D. M.).||
|250104|BAITOA (D. M.).||
|250105|LA CANELA (D. M.).||
|250106|SAN FRANCISCO DE JACAGUA (D. M.).||
|250107|HATO DEL YAQUE (D. M.).||
|250200|MUNICIPIO BISONÓ||
|250201|VILLA BISONÓ (NAVARRETE) (D. M.).||
|250300|MUNICIPIO JÁNICO||
|250301|JÁNICO||
|250302|JUNCALITO (D. M.).||
|250303|EL CAIMITO (D. M.).||
|250400|MUNICIPIO LICEY AL MEDIO||
|250401|LICEY AL MEDIO||
|250402|LAS PALOMAS (D. M.).||
|250500|MUNICIPIO SAN JOSÉ DE LAS MATAS||
|250501|SAN JOSÉ DE LAS MATAS||
|250502|EL RUBIO (D. M.).||
|250503|LA CUESTA (D. M.).||
|250504|LAS PLACETAS (D. M.).||
|250600|MUNICIPIO TAMBORIL||
|250601|TAMBORIL||
|250602|CANCA LA PIEDRA (D. M.).||
|250700|MUNICIPIO VILLA GONZÁLEZ||
|250701|VILLA GONZÁLEZ||
|250702|PALMAR ARRIBA (D. M.).||
|250703|EL LIMÓN (D. M.).||
|250800|MUNICIPIO PUÑAL||
|250801|PUÑAL||
|250802|GUAYABAL (D. M.).||
|250803|CANABACOA (D. M.).||
|250900|MUNICIPIO SABANA IGLESIA||
|250901|SABANA IGLESIA||
|251000|BAITOA||
|251001|BAITOA||
|260000|PROVINCIA SANTIAGO RODRÍGUEZ||
|260100|MUNICIPIO SAN IGNACIO DE SABANETA||
|260101|SAN IGNACIO DE SABANETA (D. M.).||
|260200|MUNICIPIO VILLA LOS ALMÁCIGOS||
|260201|VILLA LOS ALMÁCIGOS (D. M.).||
|260300|MUNICIPIO MONCIÓN||
|260301|MONCIÓN (D. M.).||


|270000|PROVINCIA VALVERDE||
| - | - | - |
|270100|MUNICIPIO MAO||
|270101|MAO (D. M.).||
|270102|AMINA (D. M.).||
|270103|JAIBÓN (PUEBLO NUEVO) (D. M.).||
|270104|GUATAPANAL (D. M.).||
|270200|MUNICIPIO ESPERANZA||
|270201|ESPERANZA||
|270202|MAIZAL (D. M.).||
|270203|JICOMÉ (D. M.).||
|270204|BOCA DE MAO (D. M.).||
|270205|PARADERO (D. M.).||
|270300|MUNICIPIO LAGUNA SALADA||
|270301|LAGUNA SALADA (D. M.).||
|270302|JAIBÓN (D. M.).||
|270303|LA CAYA (D. M.).||
|270304|CRUCE DE GUAYACANES (D. M.).||
|280000|PROVINCIA MONSEÑOR NOUEL||
|280100|MUNICIPIO BONAO||
|280101|BONAO (D. M.).||
|280102|SABANA DEL PUERTO (D. M.).||
|280103|JUMA BEJUCAL (D. M.).||
|280104|ARROYO  TORO - MASIPEDRO (D. M.).||
|280105|JAYACO (D. M.).||
|280106|LA SALVIA - LOS QUEMADOS (D. M.).||
|280200|MUNICIPIO MAIMÓN||
|280201|MAIMÓN (D. M.).||
|280300|MUNICIPIO PIEDRA BLANCA||
|280301|PIEDRA BLANCA (D. M.).||
|280302|VILLA DE SONADOR (D. M.).||
|280303|JUAN ADRIÁN (D. M.).||
|290000|PROVINCIA MONTE PLATA||
|290100|MUNICIPIO MONTE PLATA||
|290101|MONTE PLATA (D. M.).||
|290102|DON JUAN (D. M.).||
|290103|CHIRINO (D. M.).||
|290104|BOYÁ (D. M.).||
|290200|MUNICIPIO BAYAGUANA||
|290201|BAYAGUANA (D. M.).||
|290300|MUNICIPIO SABANA GRANDE DE BOYÁ||
|290301|SABANA GRANDE DE BOYÁ (D. M.).||
|290302|GONZALO (D. M.).||
|290303|MAJAGUAL (D. M.).||
|290400|MUNICIPIO YAMASÁ||
|290402|LOS BOTADOS (D. M.).||
|290403|MAMÁ TINGÓ (D. M.).||
|290500|MUNICIPIO PERALVILLO||
|290501|PERALVILLO (D. M.).||
|300000|PROVINCIA HATO MAYOR||


|300100|MUNICIPIO HATO MAYOR||
| - | - | - |
|300101|HATO MAYOR (D. M.).||
|300102|YERBA BUENA (D. M.).||
|300103|MATA PALACIO (D. M.).||
|300104|GUAYABO DULCE (D. M.).||
|300200|MUNICIPIO SABANA DE LA MAR||
|300201|SABANA DE LA MAR (D. M.).||
|300202|ELUPINA CORDERO DE LAS CAÑITAS (D. M.).||
|300300|MUNICIPIO EL VALLE||
|300301|EL VALLE (D. M.).||
|310000|PROVINCIA SAN JOSÉ DE OCOA||
|310100|MUNICIPIO SAN JOSÉ DE OCOA||
|310101|SAN JOSÉ DE OCOA (D. M.).||
|310102|LA CIÉNAGA (D. M.).||
|310103|NIZAO - LAS AUYAMAS (D. M.).||
|310104|EL PINAR (D. M.).||
|310105|EL NARANJAL (D. M.).||
|310200|MUNICIPIO SABANA LARGA||
|310201|SABANA LARGA (D. M.).||
|310300|MUNICIPIO RANCHO ARRIBA||
|310301|RANCHO ARRIBA (D. M.).||
|320000|PROVINCIA SANTO DOMINGO||
|320100|MUNICIPIO SANTO DOMINGO ESTE||
|320101|SANTO DOMINGO ESTE (D. M.).||
|320102|SAN LUIS (D. M.).||
|320200|MUNICIPIO SANTO DOMINGO OESTE||
|320201|SANTO DOMINGO OESTE (D. M.).||
|320300|MUNICIPIO SANTO DOMINGO NORTE||
|320301|SANTO DOMINGO NORTE (D. M.).||
|320302|LA VICTORIA (D. M.).||
|320400|MUNICIPIO BOCA CHICA||
|320401|BOCA CHICA (D. M.).||
|320402|LA CALETA (D. M.).||
|320500|MUNICIPIO SAN ANTONIO DE GUERRA||
|320501|SAN ANTONIO DE GUERRA (D. M.).||
|320502|HATO VIEJO (D. M.).||
|320600|MUNICIPIO LOS ALCARRIZOS||
|320601|LOS ALCARRIZOS (D. M.).||
|320602|PALMAREJO-VILLA LINDA (D. M.).||
|320603|PANTOJA (D. M.).||
|320700|MUNICIPIO PEDRO BRAND||
|320701|PEDRO BRAND (D. M.).||
|320702|LA GUÁYIGA (D. M.).||
|320703|LA CUABA (D. M.).||