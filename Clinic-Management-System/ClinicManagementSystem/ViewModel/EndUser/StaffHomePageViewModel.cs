﻿using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.EndUser
{
	public class StaffHomePageViewModel : INotifyPropertyChanged
	{
		private readonly IDao _dao;

		public int TodayFormsCount { get; set; }
		public int TotalPatientCount { get; set; }
		public int TodayPatientCount { get; set; }
		public int PendingFormsCount { get; set; }

		public StaffHomePageViewModel()
		{
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadData();
		}

		public void LoadData()
		{
			TodayFormsCount = _dao.GetTodayMedicalExaminationFormsCount();
			TotalPatientCount = _dao.GetTotalPatientsCount();
			TodayPatientCount = _dao.GetTodayNewPatientsCount();
			PendingFormsCount = _dao.GetPendingFormsCount();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
