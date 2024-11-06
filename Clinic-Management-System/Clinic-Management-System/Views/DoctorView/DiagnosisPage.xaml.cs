﻿using Clinic_Management_System.Model;
using Clinic_Management_System.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Clinic_Management_System.Views.DoctorView
{
    public sealed partial class DiagnosisPage : Page
    {
        public DiagnosisPage()
        {
            this.InitializeComponent();
            this.DataContext = new DiagnosisViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MedicalExaminationForm selectedForm)
            {
                ((DiagnosisViewModel)this.DataContext).LoadData(selectedForm.Id);
            }
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Điều hướng trở về DoctorPage
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
