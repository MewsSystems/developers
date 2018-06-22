using ExchangeRate.Entities;
using ExchangeRate.Exceptions;
using ExchangeRate.Managers;
using System;
//using log4net;
using System.Windows;
using System.Windows.Controls;

namespace ExchangeRate
{
  /// <summary>
  /// class for main Currency windows 
  /// </summary>
  public partial class MainWindow : Window
  {
    //instance for CourseManager
    private CourseManager courseManager;

    private string czk="CZK";

    public MainWindow()
    {
      courseManager = new CourseManager();
      InitializeComponent();
    }


    /// <summary>
    /// load Code to ComboBox
    /// </summary>
    private void ComboBoxCurrency_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        string CodeName = comboBoxCode.SelectedItem as string;
        var combo = sender as ComboBox;
        combo.ItemsSource = courseManager.ListCode();//list of all Code from CNB file with exchange
        combo.SelectedIndex = 0; 
      }
      catch (CourseException ext)
      {
        MessageBox.Show("Failed to load courses");
        //log.Info(ext.ToString());
      }
      catch (FileException ext)
      {
        MessageBox.Show("Failed to connect CNB");
        //log.Info(ext.ToString());
      }
      catch (SumException ext)
      {
        MessageBox.Show("Wrong format. It must be the only number.");
        //log.Info(ext.ToString());
      }
      catch (Exception ext)
      {
        MessageBox.Show(ext.ToString());
        //log.Info(ext.ToString());
      }
    }


    /// <summary>
    /// Rate add TextBox by SelectionChange 
    /// </summary>
    private void ComboBoxCurrency_SelectionChange(object sender, RoutedEventArgs e)
    {
      try
      {
        var selectedComboItem = sender as ComboBox;
        string name = selectedComboItem.SelectedItem as string;
        Currency currency = courseManager.GetCurrency(name);//Currency entity by Code
        TextBlockRate.Text = currency.Rate.ToString()+" "+czk;//TextBox add Rate
        TextBlockAmount.Text = currency.Amount.ToString();//TextBox add Amount
      }
      catch (CourseException ext)
      {
        MessageBox.Show("Failed to load courses");
        //log.Info(ext.ToString());
      }
      catch (FileException ext)
      {
        MessageBox.Show("Failed to connect CNB");
        //log.Info(ext.ToString());
      }
      catch (SumException ext)
      {
        MessageBox.Show("Wrong format. It must be the only number.");
        //log.Info(ext.ToString());
      }
      catch (Exception ext)
      {
        MessageBox.Show(ext.ToString());
        //log.Info(ext.ToString());
      }
    }

    /// <summary>
    /// Transfer sum Currency to total
    /// </summary>
    private void Transfer_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int sumItem = courseManager.convertSumToInt(TextBoxSum.Text);
        string name = comboBoxCode.SelectedItem as string;

        if( name == null )
        {
          MessageBox.Show("You must exchange");
        }
        else
        {
          TextBlockTotal.Text = courseManager.GetCurrencyTotal(name, sumItem).ToString() + " " + czk;
        }
      }
      catch (CourseException ext)
      {
        MessageBox.Show("Failed to load courses");
        //log.Info(ext.ToString());
      }
      catch (FileException ext)
      {
        MessageBox.Show("Failed to connect CNB");
        //log.Info(ext.ToString());
      }
      catch (SumException ext)
      {
        MessageBox.Show("Wrong format. It must be the only number.");
        //log.Info(ext.ToString());
      }
      catch (Exception ext)
      {
        MessageBox.Show(ext.ToString());
        //log.Info(ext.ToString());
      }
    }
  }
}
