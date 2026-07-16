namespace waScrapperAN;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var dataGridViewCellStyle1 = new DataGridViewCellStyle();
        var dataGridViewCellStyle2 = new DataGridViewCellStyle();
        var dataGridViewCellStyle3 = new DataGridViewCellStyle();

        // Main layout
        tableLayoutPrincipal = new TableLayoutPanel();
        panelHeader = new Panel();
        lblTitulo = new Label();
        lblDescripcion = new Label();
        groupBoxDatos = new GroupBox();
        tableLayoutDatos = new TableLayoutPanel();
        lblGestion = new Label();
        txtGestion = new TextBox();
        lblAduana = new Label();
        txtAduana = new TextBox();
        lblNumero = new Label();
        txtNumero = new TextBox();
        lblSerie = new Label();
        txtSerie = new TextBox();
        flowLayoutBotones = new FlowLayoutPanel();
        btnConsultar = new Button();
        btnLimpiar = new Button();
        progressConsulta = new ProgressBar();
        lblEstadoConsulta = new Label();
        panelResultado = new Panel();
        tableLayoutResultado = new TableLayoutPanel();
        lblDimResultado = new Label();
        lblCantidadEventos = new Label();
        dgvSeguimiento = new DataGridView();
        seguimientoBindingSource = new BindingSource(components);
        colEtapa = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colHora = new DataGridViewTextBoxColumn();
        colCanal = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        colObservacion = new DataGridViewTextBoxColumn();
        colUsuario = new DataGridViewTextBoxColumn();
        groupBoxArchivos = new GroupBox();
        tableLayoutArchivos = new TableLayoutPanel();
        lblRutaSolicitud = new Label();
        lblRutaRespuesta = new Label();
        lblRutaInterno = new Label();
        lblRutaCsv = new Label();
        lblRutaJson = new Label();
        btnAbrirCarpeta = new Button();
        errorProvider = new ErrorProvider(components);

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 750);
        MinimumSize = new Size(1000, 650);
        Text = "Seguimiento DIM-DUI";
        StartPosition = FormStartPosition.CenterScreen;

        // tableLayoutPrincipal
        tableLayoutPrincipal.Dock = DockStyle.Fill;
        tableLayoutPrincipal.ColumnCount = 1;
        tableLayoutPrincipal.RowCount = 6;
        tableLayoutPrincipal.Padding = new Padding(10);
        tableLayoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        // panelHeader
        panelHeader.Dock = DockStyle.Fill;
        panelHeader.Padding = new Padding(5);

        lblTitulo.Text = "Seguimiento DIM-DUI";
        lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitulo.AutoSize = true;
        lblTitulo.Location = new Point(5, 5);

        lblDescripcion.Text = "Consulte el historial completo de una Declaración de Importación mediante el servicio de la Aduana Nacional.";
        lblDescripcion.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblDescripcion.AutoSize = true;
        lblDescripcion.Location = new Point(5, 40);
        lblDescripcion.MaximumSize = new Size(1100, 0);

        panelHeader.Controls.Add(lblTitulo);
        panelHeader.Controls.Add(lblDescripcion);

        // groupBoxDatos
        groupBoxDatos.Text = "Datos de la declaración";
        groupBoxDatos.Dock = DockStyle.Fill;
        groupBoxDatos.Padding = new Padding(10);

        tableLayoutDatos.Dock = DockStyle.Fill;
        tableLayoutDatos.ColumnCount = 8;
        tableLayoutDatos.RowCount = 2;
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tableLayoutDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
        tableLayoutDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tableLayoutDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        // Row 0: Labels
        lblGestion.Text = "Gestión:";
        lblGestion.TextAlign = ContentAlignment.MiddleRight;
        lblGestion.Anchor = AnchorStyles.Right;
        lblGestion.Location = new Point(3, 6);

        lblAduana.Text = "Aduana:";
        lblAduana.TextAlign = ContentAlignment.MiddleRight;
        lblAduana.Anchor = AnchorStyles.Right;

        lblNumero.Text = "Número:";
        lblNumero.TextAlign = ContentAlignment.MiddleRight;
        lblNumero.Anchor = AnchorStyles.Right;

        lblSerie.Text = "Serie:";
        lblSerie.TextAlign = ContentAlignment.MiddleRight;
        lblSerie.Anchor = AnchorStyles.Right;

        // Row 0: TextBoxes
        txtGestion.MaxLength = 4;
        txtGestion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtGestion.Location = new Point(83, 3);

        txtAduana.MaxLength = 10;
        txtAduana.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtAduana.Location = new Point(303, 3);

        txtNumero.MaxLength = 15;
        txtNumero.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtNumero.Location = new Point(523, 3);

        txtSerie.MaxLength = 5;
        txtSerie.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtSerie.Location = new Point(703, 3);

        // Row 1: Botones
        flowLayoutBotones.Dock = DockStyle.Fill;
        flowLayoutBotones.FlowDirection = FlowDirection.LeftToRight;
        tableLayoutDatos.SetColumnSpan(flowLayoutBotones, 8);

        btnConsultar.Text = "Consultar seguimiento";
        btnConsultar.AutoSize = true;
        btnConsultar.Padding = new Padding(10, 5, 10, 5);
        btnConsultar.Click += btnConsultar_Click;

        btnLimpiar.Text = "Limpiar";
        btnLimpiar.AutoSize = true;
        btnLimpiar.Padding = new Padding(10, 5, 10, 5);
        btnLimpiar.Click += btnLimpiar_Click;

        flowLayoutBotones.Controls.Add(btnConsultar);
        flowLayoutBotones.Controls.Add(btnLimpiar);

        // Add controls to tableLayoutDatos
        tableLayoutDatos.Controls.Add(lblGestion, 0, 0);
        tableLayoutDatos.Controls.Add(txtGestion, 1, 0);
        tableLayoutDatos.Controls.Add(lblAduana, 3, 0);
        tableLayoutDatos.Controls.Add(txtAduana, 4, 0);
        tableLayoutDatos.Controls.Add(lblNumero, 6, 0);
        tableLayoutDatos.Controls.Add(txtNumero, 7, 0);
        tableLayoutDatos.Controls.Add(lblSerie, 0, 1);
        tableLayoutDatos.Controls.Add(txtSerie, 1, 1);
        tableLayoutDatos.Controls.Add(flowLayoutBotones, 0, 2);

        groupBoxDatos.Controls.Add(tableLayoutDatos);

        // progressConsulta
        progressConsulta.Style = ProgressBarStyle.Marquee;
        progressConsulta.Visible = false;
        progressConsulta.Dock = DockStyle.Fill;
        progressConsulta.MarqueeAnimationSpeed = 30;

        // lblEstadoConsulta
        lblEstadoConsulta.Dock = DockStyle.Fill;
        lblEstadoConsulta.Text = "Listo para consultar.";
        lblEstadoConsulta.TextAlign = ContentAlignment.MiddleLeft;
        lblEstadoConsulta.Padding = new Padding(5, 0, 0, 0);

        // panelResultado
        panelResultado.Dock = DockStyle.Fill;
        panelResultado.Padding = new Padding(5);

        // tableLayoutResultado
        tableLayoutResultado.Dock = DockStyle.Fill;
        tableLayoutResultado.ColumnCount = 2;
        tableLayoutResultado.RowCount = 2;
        tableLayoutResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutResultado.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
        tableLayoutResultado.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        lblDimResultado.Text = string.Empty;
        lblDimResultado.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        lblDimResultado.AutoSize = true;

        lblCantidadEventos.Text = string.Empty;
        lblCantidadEventos.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblCantidadEventos.AutoSize = true;
        lblCantidadEventos.TextAlign = ContentAlignment.MiddleRight;

        // dgvSeguimiento
        dgvSeguimiento.ReadOnly = true;
        dgvSeguimiento.AllowUserToAddRows = false;
        dgvSeguimiento.AllowUserToDeleteRows = false;
        dgvSeguimiento.AutoGenerateColumns = false;
        dgvSeguimiento.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSeguimiento.MultiSelect = false;
        dgvSeguimiento.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSeguimiento.RowHeadersVisible = false;
        dgvSeguimiento.Dock = DockStyle.Fill;
        dgvSeguimiento.DataSource = seguimientoBindingSource;
        dgvSeguimiento.AllowUserToResizeRows = false;
        dgvSeguimiento.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvSeguimiento.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvSeguimiento.BorderStyle = BorderStyle.Fixed3D;

        // Columns
        colEtapa.HeaderText = "Etapa";
        colEtapa.DataPropertyName = "Etapa";
        colEtapa.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colEtapa.FillWeight = 15F;

        colFecha.HeaderText = "Fecha";
        colFecha.DataPropertyName = "Fecha";
        colFecha.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colFecha.FillWeight = 12F;

        colHora.HeaderText = "Hora";
        colHora.DataPropertyName = "Hora";
        colHora.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colHora.FillWeight = 10F;

        colCanal.HeaderText = "Canal";
        colCanal.DataPropertyName = "Canal";
        colCanal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colCanal.FillWeight = 10F;

        colEstado.HeaderText = "Estado";
        colEstado.DataPropertyName = "Estado";
        colEstado.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colEstado.FillWeight = 13F;

        colObservacion.HeaderText = "Observación";
        colObservacion.DataPropertyName = "Observacion";
        colObservacion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colObservacion.FillWeight = 25F;

        colUsuario.HeaderText = "Usuario";
        colUsuario.DataPropertyName = "Usuario";
        colUsuario.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colUsuario.FillWeight = 15F;

        dgvSeguimiento.Columns.AddRange(new DataGridViewColumn[]
        {
            colEtapa,
            colFecha,
            colHora,
            colCanal,
            colEstado,
            colObservacion,
            colUsuario
        });

        tableLayoutResultado.Controls.Add(lblDimResultado, 0, 0);
        tableLayoutResultado.Controls.Add(lblCantidadEventos, 1, 0);
        tableLayoutResultado.Controls.Add(dgvSeguimiento, 0, 1);
        tableLayoutResultado.SetColumnSpan(dgvSeguimiento, 2);

        panelResultado.Controls.Add(tableLayoutResultado);

        // groupBoxArchivos
        groupBoxArchivos.Text = "Archivos generados";
        groupBoxArchivos.Dock = DockStyle.Fill;
        groupBoxArchivos.Padding = new Padding(10);

        tableLayoutArchivos.Dock = DockStyle.Fill;
        tableLayoutArchivos.ColumnCount = 2;
        tableLayoutArchivos.RowCount = 6;
        tableLayoutArchivos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutArchivos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        tableLayoutArchivos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        lblRutaSolicitud.AutoSize = true;
        lblRutaSolicitud.Text = string.Empty;
        lblRutaSolicitud.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        lblRutaRespuesta.AutoSize = true;
        lblRutaRespuesta.Text = string.Empty;
        lblRutaRespuesta.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        lblRutaInterno.AutoSize = true;
        lblRutaInterno.Text = string.Empty;
        lblRutaInterno.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        lblRutaCsv.AutoSize = true;
        lblRutaCsv.Text = string.Empty;
        lblRutaCsv.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        lblRutaJson.AutoSize = true;
        lblRutaJson.Text = string.Empty;
        lblRutaJson.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        btnAbrirCarpeta.Text = "Abrir carpeta";
        btnAbrirCarpeta.AutoSize = true;
        btnAbrirCarpeta.Visible = false;
        btnAbrirCarpeta.Click += btnAbrirCarpeta_Click;

        tableLayoutArchivos.Controls.Add(lblRutaSolicitud, 0, 0);
        tableLayoutArchivos.Controls.Add(lblRutaRespuesta, 0, 1);
        tableLayoutArchivos.Controls.Add(lblRutaInterno, 0, 2);
        tableLayoutArchivos.Controls.Add(lblRutaCsv, 0, 3);
        tableLayoutArchivos.Controls.Add(lblRutaJson, 0, 4);
        tableLayoutArchivos.Controls.Add(btnAbrirCarpeta, 1, 5);

        groupBoxArchivos.Controls.Add(tableLayoutArchivos);

        // errorProvider
        errorProvider.ContainerControl = this;
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // Add controls to main layout
        tableLayoutPrincipal.Controls.Add(panelHeader, 0, 0);
        tableLayoutPrincipal.Controls.Add(groupBoxDatos, 0, 1);
        tableLayoutPrincipal.Controls.Add(progressConsulta, 0, 2);
        tableLayoutPrincipal.Controls.Add(panelResultado, 0, 3);
        tableLayoutPrincipal.Controls.Add(groupBoxArchivos, 0, 4);
        tableLayoutPrincipal.Controls.Add(lblEstadoConsulta, 0, 5);

        Controls.Add(tableLayoutPrincipal);

        // Event handlers
        txtGestion.KeyPress += txtGestion_KeyPress;
        txtAduana.KeyPress += txtAduana_KeyPress;
        txtNumero.KeyPress += txtNumero_KeyPress;
        txtSerie.TextChanged += txtSerie_TextChanged;

        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // Controls
    private TableLayoutPanel tableLayoutPrincipal;
    private Panel panelHeader;
    private Label lblTitulo;
    private Label lblDescripcion;
    private GroupBox groupBoxDatos;
    private TableLayoutPanel tableLayoutDatos;
    private Label lblGestion;
    private TextBox txtGestion;
    private Label lblAduana;
    private TextBox txtAduana;
    private Label lblNumero;
    private TextBox txtNumero;
    private Label lblSerie;
    private TextBox txtSerie;
    private FlowLayoutPanel flowLayoutBotones;
    private Button btnConsultar;
    private Button btnLimpiar;
    private ProgressBar progressConsulta;
    private Label lblEstadoConsulta;
    private Panel panelResultado;
    private TableLayoutPanel tableLayoutResultado;
    private Label lblDimResultado;
    private Label lblCantidadEventos;
    private DataGridView dgvSeguimiento;
    private BindingSource seguimientoBindingSource;
    private DataGridViewTextBoxColumn colEtapa;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colHora;
    private DataGridViewTextBoxColumn colCanal;
    private DataGridViewTextBoxColumn colEstado;
    private DataGridViewTextBoxColumn colObservacion;
    private DataGridViewTextBoxColumn colUsuario;
    private GroupBox groupBoxArchivos;
    private TableLayoutPanel tableLayoutArchivos;
    private Label lblRutaSolicitud;
    private Label lblRutaRespuesta;
    private Label lblRutaInterno;
    private Label lblRutaCsv;
    private Label lblRutaJson;
    private Button btnAbrirCarpeta;
    private ErrorProvider errorProvider;
}