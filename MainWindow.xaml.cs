using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PizzaModel;
using System.Data.Entity;
using System.Data;

namespace Proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        PizzaEntitiesModel ctx = new PizzaEntitiesModel();
        CollectionViewSource chelnerVSource;
        PizzaEntitiesModel ptx = new PizzaEntitiesModel();
        CollectionViewSource pizzaVSource;
        CollectionViewSource chelnerComandasVSource;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            chelnerVSource =  ((System.Windows.Data.CollectionViewSource)(this.FindResource("chelnerViewSource")));
            chelnerVSource.Source = ctx.Chelners.Local;
            ctx.Chelners.Load();
            pizzaVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pizzaViewSource")));
            pizzaVSource.Source = ptx.Pizzas.Local;
            ptx.Pizzas.Load();

            System.Windows.Data.CollectionViewSource pizzaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pizzaViewSource")));

            System.Windows.Data.CollectionViewSource comandaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("comandaViewSource")));

            chelnerComandasVSource = (System.Windows.Data.CollectionViewSource)(this.FindResource("chelnerViewSource"));

            chelnerComandasVSource.Source = ctx.Comandas.Local;
            ctx.Comandas.Load();
            ctx.Pizzas.Load();
            cmbChelneri.ItemsSource = ctx.Chelners.Local;
           // cmbChelneri.DisplayMemberPath = "Prenume";
            cmbChelneri.SelectedValuePath = "Chelner_Id";
            cmbPizza.ItemsSource = ctx.Pizzas.Local;
            //cmbPizza.DisplayMemberPath = "Sortiment";
            cmbPizza.SelectedValuePath = "Pizza_id";
            BindDataGrid();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }
        private void btnEditO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }
        private void btnDeleteO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            chelnerVSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            chelnerVSource.View.MoveCurrentToPrevious();
        }
        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }
        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }
       
        private void SaveChelners()
        {
            Chelner chelner = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    chelner = new Chelner()
                    {
                        Prenume = prenumeTextBox.Text.Trim(),
                        Nume = numeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Chelners.Add(chelner);
                    chelnerVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    chelner = (Chelner)chelnerDataGrid.SelectedItem;
                    chelner.Prenume = prenumeTextBox.Text.Trim();
                    chelner.Nume = numeTextBox.Text.Trim();

                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    chelner = (Chelner)chelnerDataGrid.SelectedItem;
                    ctx.Chelners.Remove(chelner);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                chelnerVSource.View.Refresh();
            }

        }
        private void SavePizzas()
        {
            Pizza pizza = null;
            if (action == ActionState.New)
            {
                try
                {
                 
                    pizza = new Pizza()
                    {
                        Sortiment = sortimentTextBox.Text.Trim(),
                        Pret = pretTextBox.Text.Trim(),
                        Gramaj = gramajTextBox.Text.Trim(),

                    };
                  
                    ptx.Pizzas.Add(pizza);
                    pizzaVSource.View.Refresh();
                    ptx.SaveChanges();
                }
                
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    pizza = (Pizza)pizzaDataGrid.SelectedItem;
                    pizza.Sortiment = sortimentTextBox.Text.Trim();
                    pizza.Pret = pretTextBox.Text.Trim();
                    //salvam modificarile
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    pizza = (Pizza)pizzaDataGrid.SelectedItem;
                    ptx.Pizzas.Remove(pizza);
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                pizzaVSource.View.Refresh();
            }

        }
        private void SaveOrders()
        {
            Comanda comanda = null;
            if (action == ActionState.New)
            {
                try
                {
                    Chelner chelner = (Chelner)cmbChelneri.SelectedItem;
                    Pizza pizza = (Pizza)cmbPizza.SelectedItem;
                    //instantiem Order entity
                    comanda = new Comanda()
                    {

                        Chelner_id = chelner.Chelner_Id,
                        Pizza_id = pizza.Pizza_id
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Comandas.Add(comanda);
                    //salvam modificarile
                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                dynamic selectedOrder = comenziDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Comandas.FirstOrDefault(s => s.Comanda_id == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.Chelner_id = Int32.Parse(cmbChelneri.SelectedValue.ToString());
                        editedOrder.Pizza_id = Convert.ToInt32(cmbPizza.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                chelnerComandasVSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = comenziDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Comandas.FirstOrDefault(s => s.Comanda_id == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Comandas.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlPizza.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Chelneri":
                    SaveChelners();
                    break;
                case "Pizza":
                    SavePizzas();
                    break;
                case "Comenzi":
                    break;
            }
            ReInitialize();
        }
        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Comandas
                             join cust in ctx.Chelners on ord.Chelner_id equals cust.Chelner_Id
                             join inv in ctx.Pizzas on ord.Pizza_id equals inv.Pizza_id
                             select new
                             {
                                 ord.Comanda_id,
                                 ord.Pizza_id,
                                 ord.Chelner_id,
                                 cust.Prenume,
                                 cust.Nume,
                                 inv.Sortiment,
                                 inv.Pret,
                                 inv.Gramaj
                             };
             chelnerComandasVSource.Source = queryOrder.ToList();
        }
        private void SetValidationBinding()
        {
            Binding prenumeValidationBinding = new Binding();
            prenumeValidationBinding.Source = chelnerVSource;
            prenumeValidationBinding.Path = new PropertyPath("Prenume");
            prenumeValidationBinding.NotifyOnValidationError = true;
            prenumeValidationBinding.Mode = BindingMode.TwoWay;
            prenumeValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            prenumeValidationBinding.ValidationRules.Add(new StringNotEmpty());
            prenumeTextBox.SetBinding(TextBox.TextProperty, prenumeValidationBinding);
            Binding numeValidationBinding = new Binding();
            numeValidationBinding.Source = chelnerVSource;
            numeValidationBinding.Path = new PropertyPath("Nume");
            numeValidationBinding.NotifyOnValidationError = true;
            numeValidationBinding.Mode = BindingMode.TwoWay;
            numeValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            numeValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            numeTextBox.SetBinding(TextBox.TextProperty, numeValidationBinding); //setare binding nou
        }



    }
}
