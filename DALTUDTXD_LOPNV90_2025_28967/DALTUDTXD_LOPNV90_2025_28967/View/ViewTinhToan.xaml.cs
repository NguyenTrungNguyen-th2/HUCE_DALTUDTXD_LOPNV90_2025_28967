using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{
    public partial class ViewTinhToan : Window
    {
        private readonly UIDocument _uiDoc;
        private readonly ObservableCollection<TinhToanViewModel> _ketQuaList;

        public ViewTinhToan(ObservableCollection<TinhToanViewModel> danhSachKetQua, UIDocument uiDoc)
        {
            InitializeComponent();
            _ketQuaList = danhSachKetQua;
            _uiDoc = uiDoc;
            DataContext = new { DanhSachCotHienThi = danhSachKetQua };
        }

        public ViewTinhToan()
        {
#if DEBUG
            InitializeComponent();
            DataContext = new TinhToanViewModel();
#endif
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dg && dg.SelectedItem is TinhToanViewModel selectedCot)
            {
                if (int.TryParse(selectedCot.RevitId, out int idInt))
                {
                    var elementId = new ElementId(idInt);

                    _uiDoc.Selection.SetElementIds(
                        new List<ElementId> { elementId });

                    _uiDoc.ShowElements(elementId);
                }
            }
        }



        private void SelectColumnInRevit(string revitIdStr)
        {
            if (string.IsNullOrEmpty(revitIdStr) || _uiDoc?.Document == null) return;

            if (int.TryParse(revitIdStr, out int idInt))
            {
                try
                {
                    ElementId id = new ElementId(idInt);
                    _uiDoc.Selection.SetElementIds(new[] { id });

                    Element ele = _uiDoc.Document.GetElement(id);
                    if (ele?.get_BoundingBox(null) is BoundingBoxXYZ bb)
                    {
                        var view = _uiDoc.GetOpenUIViews()
                            .FirstOrDefault(v => v.ViewId == _uiDoc.ActiveView.Id);
                        if (view != null)
                        {
                            XYZ center = (bb.Min + bb.Max) * 0.5;
                            XYZ half = (bb.Max - bb.Min) * 0.5 * 1.3;
                            view.ZoomAndCenterRectangle(center - half, center + half);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn cột trong Revit: {ex.Message}");
                }
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_uiDoc == null || _uiDoc.Document == null)
                return;

            var vm = new ColumnRebarViewModel(
                _uiDoc.Document,
                _uiDoc
            );

            var view = new ViewVeThep
            {
                DataContext = vm,
                Owner = this
            };


            //var viewVeThep = new ViewVeThep
            //{
            //    DataContext = vm
            //};
            //this.Owner?.Close();
            //this.Close();
            view.ShowDialog();
        }



    }
}