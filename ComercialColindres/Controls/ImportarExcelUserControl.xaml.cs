using ComercialColindres.Helpers;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace ComercialColindres.Controls
{
    /// <summary>
    /// Interaction logic for ImportarExcelUserControl.xaml
    /// </summary>
    public partial class ImportarExcelUserControl : UserControl
    {
        public ImportarExcelUserControl()
        {
            InitializeComponent();
        }


        //#region Propiedades
        public static readonly DependencyProperty PropiedadColeccionDatosImportados =
            DependencyProperty.Register("ColeccionDatosImportados", typeof(IList),
            typeof(ImportarExcelUserControl), new PropertyMetadata(OnSelectedColeccionDatosImportadosCambio));

        public IList ColeccionDatosImportados
        {
            get
            {
                return (IList)GetValue(PropiedadColeccionDatosImportados);
            }
            set
            {
                SetValue(PropiedadColeccionDatosImportados, value);
            }
        }

        private static void OnSelectedColeccionDatosImportadosCambio(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as ImportarExcelUserControl;
            if (me != null)
            {
                if (e.NewValue is ObservableCollection<object>)
                {
                    me.dgDatosXLS.ItemsSource = (ObservableCollection<object>)e.NewValue;
                }
                else
                {
                    me.dgDatosXLS.ItemsSource = null;
                }
            }
        }

        #region Eventos del View

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Excel Files|*.xlsx;";
            var browsefile = openfile.ShowDialog();
            var listaDatosImportados = new ObservableCollection<object>();

            if (browsefile == true)
            {
                FileInfo existingFile = new FileInfo(openfile.FileName);
                using (ExcelPackage xlPackage = new ExcelPackage(existingFile))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet excelSheet = xlPackage.Workbook.Worksheets[1];
                    var claseDinamica = new ClaseDinamica();

                    TypeBuilder builder = claseDinamica.GetTypeBuilder(0);

                    ConstructorBuilder constructor = builder.DefineDefaultConstructor(
                         MethodAttributes.Public |
                         MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName);

                    for (int i = 1; i <= excelSheet.Dimension.Columns; i++)
                    {
                        if (excelSheet.Cells[1, i] != null && excelSheet.Cells[1, i].Value != null)
                        {
                            if (!string.IsNullOrEmpty(excelSheet.Cells[1, i].Value.ToString()))
                            {
                                claseDinamica.CreateProperty(builder,
                                    excelSheet.Cells[1, i].Value.ToString().Trim().Replace(" ", "_"), typeof(string));
                            }
                        }
                    }

                    Type t = builder.CreateType();
                    for (int i = 2; i <= excelSheet.Dimension.Rows; i++)
                    {
                        // crear instancia
                        var celda = Activator.CreateInstance(t);

                        for (int j = 1; j <= excelSheet.Dimension.Columns; j++)
                        {
                            if (excelSheet.Cells[i, j] == null || excelSheet.Cells[i, j].Value == null)
                            {
                                continue;
                            }

                            if (!string.IsNullOrEmpty(excelSheet.Cells[i, j].Value.ToString()))
                            {
                                Type type = celda.GetType();
                                PropertyInfo propiedad =
                                    type.GetProperty(excelSheet.Cells[1, j].Value.ToString().Trim().Replace(" ", "_"));

                                //Asignar valores a las propiedades de la coleccion
                                var valor = excelSheet.Cells[i, j].Value.ToString();
                                propiedad.SetValue(celda, valor, null);
                            }
                        }
                        listaDatosImportados.Add(celda);
                    }
                    excelSheet.Dispose();
                }

                dgDatosXLS.ItemsSource = listaDatosImportados;
                ColeccionDatosImportados = new ObservableCollection<object>(listaDatosImportados);
            }
        }

        #endregion
    }
}
