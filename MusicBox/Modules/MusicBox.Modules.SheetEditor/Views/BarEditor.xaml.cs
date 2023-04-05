using MusicBox.Modules.SheetEditor.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MusicBox.Modules.SheetEditor.Views
{
    /// <summary>
    /// Interaction logic for BarEditor.xaml
    /// </summary>
    public partial class BarEditor : UserControl
    {
        private BarEditorViewModel _viewModel;

        public BarEditor()
        {
            InitializeComponent();
            DataContextChanged += BarEditor_DataContextChanged;
            PositionRectangle.SizeChanged += PositionRectangle_SizeChanged;
        }

        ~BarEditor()
        {
            Debug.WriteLine("Quitting BarEditor");
        }

        private void BarEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext == null) { return; }

            _viewModel = (BarEditorViewModel)DataContext;
        }

        private void PositionRectangle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_viewModel == null) { return; }

            RemovePreviousLines();
            AddBarLines();
        }

        private void RemovePreviousLines()
        {
            for (int i = MainBarGrid.Children.Count - 1; i >= 0; --i)
            {
                if (MainBarGrid.Children[i].GetType() == typeof(Line))
                {
                    MainBarGrid.Children.RemoveAt(i);
                }
            }
        }

        private void AddBarLines()
        {
            AddHorizontalLines();
            AddVerticalLines();
        }

        private void AddHorizontalLines()
        {
            if (PositionRectangle.ActualWidth == 0) { return; }

            int yTopHorizontalLine, verticalSpacingPerTone;
            _viewModel.GetHorizontalLinesInfo((int)PositionRectangle.ActualHeight, out yTopHorizontalLine, out verticalSpacingPerTone);

            for (int i = 0; i < (5 + 1 + 5); i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 2;
                line.X1 = 0;
                line.Y1 = i * verticalSpacingPerTone + yTopHorizontalLine;
                line.X2 = PositionRectangle.ActualWidth;
                line.Y2 = line.Y1;
                line.Opacity = 0.7;
                if (i != 5) // add space between the 2 staffs
                {
                    MainBarGrid.Children.Add(line);
                }
                Canvas.SetZIndex(line, -1);
            }
        }

        private void AddVerticalLines()
        {
            if (PositionRectangle.ActualHeight == 0) { return; }

            int beatLines, subBeatLines, totalLines, lineHorizontalSpacing;
            _viewModel.GetVerticalLinesInfo((int)PositionRectangle.ActualWidth, out beatLines, out subBeatLines, out totalLines, out lineHorizontalSpacing);

            for (int i = 0; i < totalLines; i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                line.X1 = i * lineHorizontalSpacing;
                line.Y1 = 0;
                line.X2 = line.X1;
                line.Y2 = PositionRectangle.ActualHeight;
                line.Opacity = (i % (subBeatLines / beatLines + 1)) > 0 ? 0.1 : 0.4;
                MainBarGrid.Children.Add(line);
                Canvas.SetZIndex(line, -1);
            }
        }

        private void NoteRectangle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // base.OnMouseWheel(e);  /// required ??? recommanded !!!

            if (sender is Rectangle rect)
            {
                //Mouse.Capture(rect);

                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    _viewModel.RotateNoteAlteration((int)rect.Tag);
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    _viewModel.ConvertToTriplets((int)rect.Tag);
                }
                else
                {
                    _viewModel.ModifyDuration((int)rect.Tag, e.Delta > 0);
                }
                //                _viewModel.RefreshNoteInfoExecute((int)rect.Tag);
                e.Handled = true;
            }
        }

        private void NoteRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var rect = sender as Rectangle;
                if (rect != null)
                {
                    int id = (int)rect.Tag;
                    Point pt = Mouse.GetPosition(rect);
                    DataObject dataObject = new DataObject();
                    dataObject.SetData(typeof(int), id);
                    dataObject.SetData(typeof(Point), pt);
                    _viewModel.StoreTimePixel(id);
                    var dropOperation = DragDrop.DoDragDrop(rect, dataObject, DragDropEffects.Move);
                    if (dropOperation == DragDropEffects.None)
                    {
                        _viewModel.RecallTimePixel(id);
                    }
                }
            }
        }

        private void NoteRectangle_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            MoveNoteRectangle(e);
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void NoteRectangle_DragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);

            MoveNoteRectangle(e);
            e.Handled = true;
        }

        private void PositionRectangle_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            MoveNoteRectangle(e);
            e.Effects = DragDropEffects.Move;
            e.Handled |= true;
        }

        private void PositionRectangle_DragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);

            MoveNoteRectangle(e);
            e.Handled = true;
        }

        private void MoveNoteRectangle(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)) && e.Data.GetDataPresent(typeof(Point)))
            {
                int id = (int)e.Data.GetData(typeof(int));
                var ptRelative = (Point)e.Data.GetData(typeof(Point));

                Point pt = e.GetPosition(PositionRectangle);
                pt = (Point)(pt - ptRelative);

                _viewModel.MoveTimePixel(id, pt);
            }
        }

        private void NoteRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle rect)
            {
            }
        }

        private void NoteRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle rect)
            {
            }
        }
    }
}