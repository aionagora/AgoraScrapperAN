using waScrapperAN.Models;
using waScrapperAN.Utilities;

namespace waScrapperAN.Tests;

public sealed class DimDuiSoapResponseParserTests
{
    private readonly DimDuiSoapResponseParser _parser = new();
    private readonly DimDuiTrackingRequest _request = new()
    {
        Gestion = "2026",
        Aduana = "701",
        Numero = "2225718",
        Serie = "C"
    };
    private readonly GeneratedTrackingFiles _archivos = new();

    [Fact]
    public void Parse_MultipleEstados_ReturnsTwoEvents()
    {
        // Arrange
        var soapResponse = CreateSoapResponse(@"
            <click>
                <dim>DI-2026-701-C-2225718</dim>
                <estado>
                    <etapa>VALIDACION</etapa>
                    <fecha>16/07/2026</fecha>
                    <hora>10:20:00</hora>
                    <canal></canal>
                    <estado>VALIDADO</estado>
                    <obs>Proceso correcto</obs>
                    <usuario>ANDROID</usuario>
                </estado>
                <estado>
                    <etapa>PAGO</etapa>
                    <fecha>16/07/2026</fecha>
                    <hora>10:30:00</hora>
                    <estado>PAGADO</estado>
                </estado>
            </click>");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Eventos.Count);
        Assert.Equal("DI-2026-701-C-2225718", result.Dim);
        Assert.Equal("VALIDACION", result.Eventos[0].Etapa);
        Assert.Equal("PAGO", result.Eventos[1].Etapa);
        Assert.Equal("VALIDADO", result.Eventos[0].Estado);
        Assert.Equal("PAGADO", result.Eventos[1].Estado);
        Assert.Equal("ANDROID", result.Eventos[0].Usuario);
        Assert.Equal(string.Empty, result.Eventos[1].Usuario);
    }

    [Fact]
    public void Parse_ConObservacion_LeeObservacion()
    {
        // Arrange
        var soapResponse = CreateSoapResponse(@"
            <click>
                <dim>DI-2026-701-C-2225718</dim>
                <estado>
                    <etapa>VALIDACION</etapa>
                    <fecha>16/07/2026</fecha>
                    <hora>10:20:00</hora>
                    <observacion>Documentos en revisión</observacion>
                    <estado>EN REVISION</estado>
                    <usuario>ANDROID</usuario>
                </estado>
            </click>");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
        Assert.Equal("Documentos en revisión", result.Eventos[0].Observacion);
    }

    [Fact]
    public void Parse_SinObservacionUsaObs()
    {
        // Arrange
        var soapResponse = CreateSoapResponse(@"
            <click>
                <dim>DI-2026-701-C-2225718</dim>
                <estado>
                    <etapa>VALIDACION</etapa>
                    <fecha>16/07/2026</fecha>
                    <hora>10:20:00</hora>
                    <obs>Proceso correcto</obs>
                    <estado>VALIDADO</estado>
                    <usuario>ANDROID</usuario>
                </estado>
            </click>");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
        Assert.Equal("Proceso correcto", result.Eventos[0].Observacion);
    }

    [Fact]
    public void Parse_SinUsuario_DevuelveUsuarioVacio()
    {
        // Arrange
        var soapResponse = CreateSoapResponse(@"
            <click>
                <dim>DI-2026-701-C-2225718</dim>
                <estado>
                    <etapa>VALIDACION</etapa>
                    <fecha>16/07/2026</fecha>
                    <hora>10:20:00</hora>
                    <estado>VALIDADO</estado>
                </estado>
            </click>");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
        Assert.Equal(string.Empty, result.Eventos[0].Usuario);
    }

    [Fact]
    public void Parse_CodigoInicial1_RetiraCodigoYProcesa()
    {
        // Arrange
        var innerXml = @"<click><dim>DI-2026-701-C-2225718</dim><estado><etapa>VALIDACION</etapa><fecha>16/07/2026</fecha><hora>10:20:00</hora><estado>VALIDADO</estado></estado></click>";
        var soapResponse = CreateSoapResponseWithCode("1" + innerXml);

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
        Assert.Equal("VALIDACION", result.Eventos[0].Etapa);
    }

    [Fact]
    public void Parse_CodigoInicial0_DevuelveError()
    {
        // Arrange
        var soapResponse = CreateSoapResponseWithCode("0Error del servicio: DUI no encontrado");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error del servicio: DUI no encontrado", result.Message);
        Assert.Empty(result.Eventos);
    }

    [Fact]
    public void Parse_SinEstados_DevuelveResultadoControlado()
    {
        // Arrange
        var soapResponse = CreateSoapResponse(@"
            <click>
                <dim>DI-2026-701-C-2225718</dim>
            </click>");

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Eventos);
        Assert.Equal("No se encontraron eventos.", result.Message);
    }

    [Fact]
    public void Parse_ReporteClickResult_EncuentraResultado()
    {
        // Arrange
        var innerXml = "<click><dim>DI-2026-701-C-2225718</dim><estado><etapa>VALIDACION</etapa><fecha>16/07/2026</fecha><hora>10:20:00</hora><estado>VALIDADO</estado></estado></click>";
        var soapResponse = CreateSoapResponseWithElementName("reporteClickResult", innerXml);

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
    }

    [Fact]
    public void Parse_Return_UsaReturnCuandoNoHayReporteClickResult()
    {
        // Arrange
        var innerXml = "<click><dim>DI-2026-701-C-2225718</dim><estado><etapa>VALIDACION</etapa><fecha>16/07/2026</fecha><hora>10:20:00</hora><estado>VALIDADO</estado></estado></click>";
        var soapResponse = CreateSoapResponseWithElementName("return", innerXml);

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
    }

    [Fact]
    public void Parse_SoapFault_DetectaYDevuelveError()
    {
        // Arrange
        var soapFault = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<v:Envelope xmlns:v=""http://schemas.xmlsoap.org/soap/envelope/"">
  <v:Body>
    <v:Fault>
      <faultcode>v:Server</faultcode>
      <faultstring>Error interno del servidor</faultstring>
      <detail>Detalle técnico del error</detail>
    </v:Fault>
  </v:Body>
</v:Envelope>";

        // Act
        var result = _parser.Parse(soapFault, _request, _archivos);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Error interno del servidor", result.Message);
        Assert.Empty(result.Eventos);
    }

    [Fact]
    public void Parse_CaracteresEscapados_InterpretaCorrectamente()
    {
        // Arrange
        var escapedContent = "<click><dim>DI-2026-701-C-2225718</dim><estado><etapa>VALIDACION</etapa><fecha>16/07/2026</fecha><hora>10:20:00</hora><estado>VALIDADO</estado></estado></click>";
        var soapResponse = CreateSoapResponseWithCode("1" + escapedContent);

        // Act
        var result = _parser.Parse(soapResponse, _request, _archivos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Eventos);
        Assert.Equal("VALIDACION", result.Eventos[0].Etapa);
        Assert.Equal("VALIDADO", result.Eventos[0].Estado);
    }

    #region Helpers

    private static string CreateSoapResponse(string innerXml)
    {
        var encoded = System.Net.WebUtility.HtmlEncode(innerXml);
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<v:Envelope xmlns:v=""http://schemas.xmlsoap.org/soap/envelope/"">
  <v:Header />
  <v:Body>
    <n0:reporteClickResponse xmlns:n0=""http://wsgerencialanb.aduana.gob.bo/"">
      <reporteClickResult>1{encoded}</reporteClickResult>
    </n0:reporteClickResponse>
  </v:Body>
</v:Envelope>";
    }

    private static string CreateSoapResponseWithCode(string rawContent)
    {
        var encoded = System.Net.WebUtility.HtmlEncode(rawContent);
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<v:Envelope xmlns:v=""http://schemas.xmlsoap.org/soap/envelope/"">
  <v:Header />
  <v:Body>
    <n0:reporteClickResponse xmlns:n0=""http://wsgerencialanb.aduana.gob.bo/"">
      <reporteClickResult>{encoded}</reporteClickResult>
    </n0:reporteClickResponse>
  </v:Body>
</v:Envelope>";
    }

    private static string CreateSoapResponseWithElementName(string elementName, string innerXml)
    {
        var encoded = System.Net.WebUtility.HtmlEncode(innerXml);
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<v:Envelope xmlns:v=""http://schemas.xmlsoap.org/soap/envelope/"">
  <v:Header />
  <v:Body>
    <n0:reporteClickResponse xmlns:n0=""http://wsgerencialanb.aduana.gob.bo/"">
      <{elementName}>1{encoded}</{elementName}>
    </n0:reporteClickResponse>
  </v:Body>
</v:Envelope>";
    }

    #endregion
}