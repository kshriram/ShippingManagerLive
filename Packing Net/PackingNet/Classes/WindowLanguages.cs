using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Packing_Net.Classes
{
    /// <summary>
    /// Author: Avinash.
    /// Version: Alfa.
    /// Class is to get the Controls on the Forms 
    /// Here it is used to Convert into languages
    /// </summary>
   public static class WindowLanguages
    {
       /// <summary>
       /// Find same type controls from the specified form
       /// </summary>
       /// <typeparam name="T">Control Type</typeparam>
       /// <param name="depObj">Form Object</param>
       /// <returns>Control collection</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
       /// <summary>
       /// Convet the window language to the selected language 
       /// </summary>
       /// <param name="Frmclass">window object(this)</param>
        public static void Convert(DependencyObject Frmclass)
        { 
            //Convert Labels 
            try
            {
               
                foreach (Label lbl in FindVisualChildren<Label>(Frmclass))
                {

                    lbl.Content = Global.controller.ConvetLanguage(lbl.Content.ToString(), Global.LanguageFileName);
                }
            }
            catch (Exception)
            { } 
            //Convert Buttons
            try
            {
                
                foreach (Button btn in FindVisualChildren<Button>(Frmclass))
                {
                    btn.Content = Global.controller.ConvetLanguage(btn.Content.ToString(), Global.LanguageFileName);
                }
            }
            catch (Exception)
            {}
            //Convert DataGridTemplateColumn
            try
            {
                foreach (DataGrid dgv in FindVisualChildren<DataGrid>(Frmclass))
                {
                    int Clm = dgv.Columns.Count();
                    for (int i = 0; i <= Clm; i++)
                    {
                        DataGridTemplateColumn temp = new DataGridTemplateColumn();
                        DataGridTextColumn text = new DataGridTextColumn();
                        if (dgv.Columns[i].GetType() == temp.GetType())
                        {
                            DataGridTemplateColumn templClm = (DataGridTemplateColumn)dgv.Columns[i];
                            templClm.Header = Global.controller.ConvetLanguage(templClm.Header.ToString(), Global.LanguageFileName);
                        }
                        else if (dgv.Columns[i].GetType() == text.GetType())
                        {
                            DataGridTextColumn textclm = (DataGridTextColumn)dgv.Columns[i];
                            textclm.Header = Global.controller.ConvetLanguage(textclm.Header.ToString(), Global.LanguageFileName);
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }
    }
}
