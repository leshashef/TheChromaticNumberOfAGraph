using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TOIKTapp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            PositionComplete = false;
            IsSelected = false;
            Grafs = new List<Graf>();
            InitializeComponent();
            MatrixIns.IsReadOnly = true;
            MatrixSm.IsReadOnly = true;
            ChromeDigit.IsReadOnly = true;
        }
        private double x = 70.0;
        private double y = 50.0;

        private List<Graf> Grafs { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!PositionComplete)
            {
                if (Graf.count < 20)
                {
                    Graf graf = new Graf();
                    Graf.count++;
                    graf.CircleGraf.Fill = Brushes.White;
                    graf.CircleGraf.Stroke = Brushes.Red;
                    graf.CircleGraf.Width = 40;
                    graf.CircleGraf.Height = 40;
                    graf.CircleGraf.Name = "Name" + Graf.count;
                    graf.Name = "Name" + Graf.count;
                    graf.Id = Graf.count;
                    graf.CircleGraf.MouseMove += Mouse_Down_Drag;
                    graf.CircleGraf.MouseDown += Mouse_Down;
                    graf.CircleGraf.SetValue(Canvas.LeftProperty, x);
                    graf.CircleGraf.SetValue(Canvas.TopProperty, y);
                    canvas.Children.Add(graf.CircleGraf);
                    Grafs.Add(graf);
                    x += 70;
                    if (Graf.count % 10 == 0)
                    {
                        x = 70;
                        y += 100;
                    }
                }
            }
        }

        private bool IsSelected;
        private bool PositionComplete;
        private void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (PositionComplete)
            {
                if (!IsSelected)
                {
                    Ellipse ellipse = (Ellipse)sender;

                    Graf graf = Grafs.FirstOrDefault(x => x.Name == ellipse.Name);
                    graf.IsActiv = true;

                    IsSelected = true;
                }
                else
                {
                    Ellipse ellipse = (Ellipse)sender;

                    Graf graf = Grafs.FirstOrDefault(x => x.Name == ellipse.Name);
                    if (graf.IsActiv == true)
                    {
                        graf.IsActiv = false;
                        IsSelected = false;
                        return;
                    }
                    graf.IsActiv = true;

                    List<Graf> grafs = Grafs.Where(x => x.IsActiv == true).ToList();

                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = 2;
                    line.X1 = (double)grafs[0].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[0].CircleGraf.Width / 2 - GetLineX(grafs);
                    line.Y1 = (double)grafs[0].CircleGraf.GetValue(Canvas.TopProperty) + grafs[0].CircleGraf.Height / 2 - GetLineY(grafs);
                    line.X2 = (double)grafs[1].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[1].CircleGraf.Width / 2 + GetLineX(grafs);
                    line.Y2 = (double)grafs[1].CircleGraf.GetValue(Canvas.TopProperty) + grafs[1].CircleGraf.Height / 2 + GetLineY(grafs);
                    canvas.Children.Add(line);
                    grafs[0].AdjacentGraphs.Add(grafs[1]);
                    grafs[1].AdjacentGraphs.Add(grafs[0]);
                    grafs[0].IsActiv = false;
                    grafs[1].IsActiv = false;
                    IsSelected = false;
                }
            }
        }

        private void Mouse_Down_Drag(object sender, MouseEventArgs e)
        {
            if (!PositionComplete)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Ellipse ellipse = (Ellipse)sender;

                    Graf graf = Grafs.FirstOrDefault(x => x.Name == ellipse.Name);
                    graf.CircleGraf.SetValue(Canvas.LeftProperty, e.GetPosition(canvas).X - 20);
                    graf.CircleGraf.SetValue(Canvas.TopProperty, e.GetPosition(canvas).Y - 20);
                }
            }
        }
        private double GetLineX(List<Graf> grafs)
        {
            double X1 = (double)grafs[0].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[0].CircleGraf.Width / 2;
            double y1 = (double)grafs[0].CircleGraf.GetValue(Canvas.TopProperty) + grafs[0].CircleGraf.Height / 2;
            double x2 = (double)grafs[1].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[1].CircleGraf.Width / 2;
            double y2 = (double)grafs[1].CircleGraf.GetValue(Canvas.TopProperty) + grafs[1].CircleGraf.Height / 2;
            double AC = Math.Sqrt(Math.Pow(X1 - x2, 2) + Math.Pow(y1 - y2, 2));
            double AD = grafs[0].CircleGraf.Width / 2;
            return AD * (X1 - x2) / AC;
        }
        private double GetLineY(List<Graf> grafs)
        {
            double X1 = (double)grafs[0].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[0].CircleGraf.Width / 2;
            double y1 = (double)grafs[0].CircleGraf.GetValue(Canvas.TopProperty) + grafs[0].CircleGraf.Height / 2;
            double x2 = (double)grafs[1].CircleGraf.GetValue(Canvas.LeftProperty) + grafs[1].CircleGraf.Width / 2;
            double y2 = (double)grafs[1].CircleGraf.GetValue(Canvas.TopProperty) + grafs[1].CircleGraf.Height / 2;
            double AC = Math.Sqrt(Math.Pow(X1 - x2, 2) + Math.Pow(y1 - y2, 2));
            double AD = grafs[0].CircleGraf.Width / 2;
            return AD * (y1 - y2) / AC;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {

            if (checkBox1.IsChecked.Value)
            {
                PositionComplete = true;
            }

        }

        private void checkBox1_UnChecked(object sender, RoutedEventArgs e)
        {
            if (PositionComplete)
            {
                checkBox1.IsChecked = true;
            }
        }

        private void ColorPick(List<Graf> grafs)
        {
            List<int> color = new List<int>();
            foreach (var graf in grafs)
            {
                foreach (var grafIn in graf.AdjacentGraphs)
                {
                    List<int> col = color.Where(x => x == grafIn.Color).ToList();
                    if (col.Count == 0)
                        color.Add(grafIn.Color);
                }
                for (int x = 0; x < Graf.count+1; x++)
                {
                    List<int> col = color.Where(y => y == x).ToList();
                    if (col.Count == 0)
                    {
                        if (x == 0) { continue; }

                        graf.Color = x;
                        break;
                    }
                }
                color = new List<int>();
            }
            foreach (var graf in grafs)
            {
                switch (graf.Color)
                {
                    case 1:
                        graf.CircleGraf.Fill = Brushes.Red;
                        break;
                    case 2:
                        graf.CircleGraf.Fill = Brushes.Black;
                        break;
                    case 3:
                        graf.CircleGraf.Fill = Brushes.Gray;
                        break;
                    case 4:
                        graf.CircleGraf.Fill = Brushes.Bisque;
                        break;
                    case 5:
                        graf.CircleGraf.Fill = Brushes.Blue;
                        break;
                    case 6:
                        graf.CircleGraf.Fill = Brushes.DarkBlue;
                        break;
                    case 7:
                        graf.CircleGraf.Fill = Brushes.Gold;
                        break;
                    case 8:
                        graf.CircleGraf.Fill = Brushes.AliceBlue;
                        break;
                    case 9:
                        graf.CircleGraf.Fill = Brushes.Beige;
                        break;
                    case 10:
                        graf.CircleGraf.Fill = Brushes.Cornsilk;
                        break;
                    case 11:
                        graf.CircleGraf.Fill = Brushes.Coral;
                        break;
                    case 12:
                        graf.CircleGraf.Fill = Brushes.AntiqueWhite;
                        break;
                    case 13:
                        graf.CircleGraf.Fill = Brushes.Aqua;
                        break;
                    case 14:
                        graf.CircleGraf.Fill = Brushes.DarkOrange;
                        break;
                    case 15:
                        graf.CircleGraf.Fill = Brushes.ForestGreen;
                        break;
                    case 16:
                        graf.CircleGraf.Fill = Brushes.Chartreuse;
                        break;
                    case 17:
                        graf.CircleGraf.Fill = Brushes.LightCyan;
                        break;
                    case 18:
                        graf.CircleGraf.Fill = Brushes.GreenYellow;
                        break;
                    case 19:
                        graf.CircleGraf.Fill = Brushes.Pink;
                        break;
                    case 20:
                        graf.CircleGraf.Fill = Brushes.Purple;
                        break;
                    default:
                        graf.CircleGraf.Fill = Brushes.Green;
                        break;

                }
            }
        }

        private void GrafSmej()
        {
            foreach (var graf in Grafs)
            {
                for (int x = 1; x <= Graf.count; x++)
                {
                    List<Graf> gr = graf.AdjacentGraphs.Where(y => y.Id == x).ToList();
                    if (gr.Count == 0)
                        MatrixSm.Text += 0 + " ";
                    else
                        MatrixSm.Text += 1 + " ";
                }
                MatrixSm.Text += "\n";
            }
        }
        private void GrafIns()
        {
            string reb;
            int x = 1;
            Dictionary<int, string> rebrs = new Dictionary<int, string>();
            foreach (var graf in Grafs)
            {
                foreach (var gr in graf.AdjacentGraphs)
                {
                    if (graf.Id < gr.Id)
                        reb = graf.Id.ToString() + gr.Id.ToString();
                    else
                        reb = gr.Id.ToString() + graf.Id.ToString();

                    if (!rebrs.ContainsValue(reb))
                    {
                        rebrs.Add(x, reb);
                        x++;
                    }
                }
            }
            foreach (var rebs in rebrs)
            {
                foreach (var graf in Grafs)
                {
                    if (rebs.Value.Length == 2)
                    {
                        string str1 = rebs.Value[0].ToString();
                        string str2 = rebs.Value[1].ToString();
                        if (graf.Id.ToString() == str1 || graf.Id.ToString() == str2)
                        {
                            MatrixIns.Text += 1 + " ";
                        }
                        else
                        {
                            MatrixIns.Text += 0 + " ";
                        }

                    }
                    else if(rebs.Value.Length == 3)
                    {
                        string str1 = rebs.Value[0].ToString();
                        string str2 = rebs.Value[1].ToString() + rebs.Value[2].ToString();
                        if(graf.Id.ToString() == str1 || graf.Id.ToString() == str2)
                        {
                            MatrixIns.Text += 1 + " ";
                        }
                        else
                        {
                            MatrixIns.Text += 0 + " ";
                        }
                    }
                    else if (rebs.Value.Length == 4)
                    {
                        string str1 = rebs.Value[0].ToString()+ rebs.Value[1].ToString();
                        string str2 = rebs.Value[2].ToString() + rebs.Value[3].ToString();
                        if (graf.Id.ToString() == str1 || graf.Id.ToString() == str2)
                        {
                            MatrixIns.Text += 1 + " ";
                        }
                        else
                        {
                            MatrixIns.Text += 0 + " ";
                        }
                    }
                }
                MatrixIns.Text += "\n";
            }
        }
        private void ChromeDigitResult()
        {
            ChromeDigit.Text = Grafs.Max(x => x.Color).ToString();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked.Value && checkBox1.IsChecked.Value)
            {
                ColorPick(Grafs);
                GrafSmej();
                GrafIns();
                ChromeDigitResult();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PositionComplete = false;
            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            MatrixIns.Text = "";
            MatrixSm.Text = "";
            ChromeDigit.Text = "";
            Grafs = new List<Graf>();
            canvas.Children.RemoveRange(0, canvas.Children.Count);
            x = 70;
            y = 50;
            Graf.count = 0;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DescriptionWindow description = new DescriptionWindow();
            description.Owner = this;
            description.ShowDialog();

        }
    }
}
