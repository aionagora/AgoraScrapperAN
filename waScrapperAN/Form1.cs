using System.Diagnostics;
using waScrapperAN.Models;
using waScrapperAN.Services;

namespace waScrapperAN
{
    public partial class Form1 : Form
    {
        private readonly IDimDuiTrackingService _trackingService;
        private CancellationTokenSource? _cts;

        public Form1(IDimDuiTrackingService trackingService)
        {
            InitializeComponent();
            _trackingService = trackingService;

            // Set initial values
            txtGestion.Text = DateTime.Now.Year.ToString();
            txtAduana.Text = "701";
            txtSerie.Text = "C";
        }

        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            _ = ConsultarAsync();
        }

        private async Task ConsultarAsync()
        {
            // Clear previous errors
            errorProvider.Clear();
            lblEstadoConsulta.Text = string.Empty;

            // Validate
            if (!ValidarFormulario())
                return;

            // Build request
            var request = new DimDuiTrackingRequest
            {
                Gestion = txtGestion.Text.Trim(),
                Aduana = txtAduana.Text.Trim(),
                Numero = txtNumero.Text.Trim(),
                Serie = txtSerie.Text.Trim().ToUpperInvariant()
            };

            // Activar estado de carga
            SetLoadingState(true);
            lblEstadoConsulta.Text = "Consultando Aduana Nacional...";
            lblEstadoConsulta.ForeColor = Color.DodgerBlue;
            dgvSeguimiento.DataSource = null;
            lblDimResultado.Text = string.Empty;
            lblCantidadEventos.Text = string.Empty;
            LimpiarRutasArchivos();
            btnAbrirCarpeta.Visible = false;

            _cts = new CancellationTokenSource();

            try
            {
                lblEstadoConsulta.Text = "Consultando Aduana Nacional...";
                var result = await _trackingService.ConsultarAsync(request, _cts.Token);

                if (result.IsSuccess)
                {
                    MostrarResultado(result);
                }
                else
                {
                    lblEstadoConsulta.Text = "Ocurrió un error durante la consulta.";
                    lblEstadoConsulta.ForeColor = Color.Red;
                    MessageBox.Show(result.Message, "Resultado de la consulta",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                lblEstadoConsulta.Text = "Consulta cancelada.";
                lblEstadoConsulta.ForeColor = Color.Gray;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inesperado: {ex}");
                lblEstadoConsulta.Text = "Ocurrió un error durante la consulta.";
                lblEstadoConsulta.ForeColor = Color.Red;
                MessageBox.Show("Ocurrió un error inesperado. Intente nuevamente.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                _cts?.Dispose();
                _cts = null;
            }
        }

        private bool ValidarFormulario()
        {
            bool isValid = true;

            // Gestion
            if (string.IsNullOrWhiteSpace(txtGestion.Text))
            {
                errorProvider.SetError(txtGestion, "La gestión es obligatoria.");
                isValid = false;
            }
            else if (txtGestion.Text.Trim().Length != 4 || !txtGestion.Text.Trim().All(char.IsDigit))
            {
                errorProvider.SetError(txtGestion, "La gestión debe contener exactamente 4 dígitos.");
                isValid = false;
            }

            // Aduana
            if (string.IsNullOrWhiteSpace(txtAduana.Text))
            {
                errorProvider.SetError(txtAduana, "Debe introducir un código de aduana válido.");
                isValid = false;
            }
            else if (!txtAduana.Text.Trim().All(char.IsDigit))
            {
                errorProvider.SetError(txtAduana, "La aduana debe contener solo números.");
                isValid = false;
            }

            // Numero
            if (string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                errorProvider.SetError(txtNumero, "Debe introducir el número de registro.");
                isValid = false;
            }
            else if (!txtNumero.Text.Trim().All(char.IsDigit))
            {
                errorProvider.SetError(txtNumero, "El número debe contener solo dígitos.");
                isValid = false;
            }

            // Serie
            if (string.IsNullOrWhiteSpace(txtSerie.Text))
            {
                errorProvider.SetError(txtSerie, "Debe introducir la serie.");
                isValid = false;
            }

            return isValid;
        }

        private void MostrarResultado(DimDuiTrackingResult result)
        {
            lblDimResultado.Text = $"DIM: {result.Dim}";
            lblCantidadEventos.Text = $"Eventos encontrados: {result.Eventos.Count}";

            // Bind to DataGridView
            seguimientoBindingSource.DataSource = result.Eventos.ToList();
            dgvSeguimiento.DataSource = seguimientoBindingSource;

            // Show file paths
            MostrarRutasArchivos(result.Archivos);

            if (result.Eventos.Count > 0)
            {
                lblEstadoConsulta.Text = "Consulta completada correctamente.";
                lblEstadoConsulta.ForeColor = Color.Green;
            }
            else
            {
                lblEstadoConsulta.Text = "No se encontraron eventos.";
                lblEstadoConsulta.ForeColor = Color.Orange;
            }
        }

        private void MostrarRutasArchivos(GeneratedTrackingFiles archivos)
        {
            lblRutaSolicitud.Text = !string.IsNullOrEmpty(archivos.SolicitudSoapPath)
                ? $"Solicitud SOAP: {archivos.SolicitudSoapPath}" : string.Empty;
            lblRutaRespuesta.Text = !string.IsNullOrEmpty(archivos.RespuestaSoapPath)
                ? $"Respuesta SOAP: {archivos.RespuestaSoapPath}" : string.Empty;
            lblRutaInterno.Text = !string.IsNullOrEmpty(archivos.ResultadoInternoPath)
                ? $"XML interno: {archivos.ResultadoInternoPath}" : string.Empty;
            lblRutaCsv.Text = !string.IsNullOrEmpty(archivos.CsvPath)
                ? $"CSV: {archivos.CsvPath}" : string.Empty;
            lblRutaJson.Text = !string.IsNullOrEmpty(archivos.JsonPath)
                ? $"JSON: {archivos.JsonPath}" : string.Empty;

            bool hasFiles = !string.IsNullOrEmpty(archivos.CsvPath) ||
                           !string.IsNullOrEmpty(archivos.JsonPath);
            btnAbrirCarpeta.Visible = hasFiles;
        }

        private void LimpiarRutasArchivos()
        {
            lblRutaSolicitud.Text = string.Empty;
            lblRutaRespuesta.Text = string.Empty;
            lblRutaInterno.Text = string.Empty;
            lblRutaCsv.Text = string.Empty;
            lblRutaJson.Text = string.Empty;
            btnAbrirCarpeta.Visible = false;
        }

        private void SetLoadingState(bool isLoading)
        {
            btnConsultar.Enabled = !isLoading;
            btnLimpiar.Enabled = !isLoading;
            txtGestion.Enabled = !isLoading;
            txtAduana.Enabled = !isLoading;
            txtNumero.Enabled = !isLoading;
            txtSerie.Enabled = !isLoading;
            progressConsulta.Visible = isLoading;
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtGestion.Text = DateTime.Now.Year.ToString();
            txtAduana.Text = "701";
            txtNumero.Text = string.Empty;
            txtSerie.Text = "C";
            errorProvider.Clear();
            dgvSeguimiento.DataSource = null;
            seguimientoBindingSource.Clear();
            lblDimResultado.Text = string.Empty;
            lblCantidadEventos.Text = string.Empty;
            lblEstadoConsulta.Text = "Listo para consultar.";
            lblEstadoConsulta.ForeColor = SystemColors.ControlText;
            LimpiarRutasArchivos();
        }

        private void btnAbrirCarpeta_Click(object? sender, EventArgs e)
        {
            var outputDir = @"C:\Temp\SeguimientoDIM";
            if (Directory.Exists(outputDir))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputDir,
                    UseShellExecute = true
                });
            }
        }

        private void txtGestion_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtAduana_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtNumero_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtSerie_TextChanged(object? sender, EventArgs e)
        {
            // Convert to uppercase
            var selectionStart = txtSerie.SelectionStart;
            txtSerie.Text = txtSerie.Text.ToUpperInvariant();
            txtSerie.SelectionStart = selectionStart;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            base.OnFormClosing(e);
        }
    }
}