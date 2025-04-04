﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp User chứa thông tin của người dùng gồm các thuộc tính id, name, password, username, gender, role, birthday, address, phone, status
	/// </summary>
	public class User : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password {  get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string role { get; set; }
        public DateTimeOffset? birthday { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
