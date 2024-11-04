﻿using Clinic_Management_System.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Service.DataAccess
{
    public class SqlServerDao : IDao
    {
		public (int, string) Authentication(string username, string password)
		{
			if (username == null || password == null)
			{
				return (0, "");
			}

			var connectionString = GetConnectionString();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				string query = $"SELECT id, role FROM EndUser WHERE username = @Username AND password = @Password";
				var command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Username", username);
				command.Parameters.AddWithValue("@Password", password);

				var reader = command.ExecuteReader();
				int id = 0;
				string role = "";

				if(reader.Read())
				{
					id = (int) reader["id"];
					role = reader["role"].ToString();
					return (id, role);
				}


			}

			return (0, "");
		}
		private static string GetConnectionString()
        {
            var connectionString = """
                 Server = localhost,1433;
                    Database = ClinicManagementSystemDatabase;
                    User Id = sa;
                    Password = SqlServer@123;
                    TrustServerCertificate = True;
                """;
            return connectionString;
        }



		private void AddParameters(SqlCommand command, params (string ParameterName, object Value)[] parameters)
		{
			foreach (var (parameterName, value) in parameters)
			{
				command.Parameters.AddWithValue(parameterName, value);
			}
		}


		public (bool, int) AddPatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

			

            var command = new SqlCommand("INSERT INTO Patient (Name , Email , ResidentId, Address , Birthday , Gender) " +
				"VALUES (@Name , @Email , @ResidentId , @Address , @DoB , @Gender)" +
				"SELECT CAST(SCOPE_IDENTITY() AS int);", connection);
            AddParameters(command,
                ("@Name", patient.Name),
                ("@Email", patient.Email),
                ("@ResidentId", patient.ResidentId),
                ("@Address", patient.Address),
                ("@DoB", patient.DoB),
                ("@Gender", patient.Gender));

			var result = command.ExecuteScalar();
			int id = result != null ? Convert.ToInt32(result) : 0;

			connection.Close();
			return (id > 0, id);
        }

        public bool AddMedicalExaminationForm(int patientId ,MedicalExaminationForm medicalExaminationForm)
		{
			var connectionString = GetConnectionString();
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();

			DateTime currentDate = DateTime.Now;
            string formatDate = currentDate.ToString("yyyy-MM-dd");
			int id = UserSessionService.Instance.LoggedInUserId;

			var command = new SqlCommand("INSERT INTO MedicalExaminationForm (PatientId, StaffId, DoctorId, Time, Symptom) VALUES (@PatientId, @StaffId, @DoctorId, @Time, @Symptom)", connection);


		
			AddParameters(command,
				("@PatientId", patientId),
				("@StaffId", id),
				("@DoctorId", medicalExaminationForm.DoctorId),
                ("@Time", formatDate),
                ("@Symptom", medicalExaminationForm.Symptoms));
			
			int result = command.ExecuteNonQuery();

			connection.Close();
			return result > 0;
		}

		public List<Doctor> GetInforDoctor()
		{
			var connectionString = GetConnectionString();
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();

			string query1 = $"SELECT id, role FROM EndUser WHERE username = @Username AND password = @Password";
			var command1 = new SqlCommand(query1, connection);

			var query = $"""
							SELECT e.id, e.name as doctorName, s.id as specialtyId, s.name as specialtyName, d.room 
							FROM EndUser e JOIN Doctor d ON e.id = d.userId
								JOIN Specialty s ON d.specialtyId = s.id
							WHERE e.role = @Role;  
						""";
			var command = new SqlCommand(query, connection);
			AddParameters(command,
				("@Role", "doctor"));

			var reader = command.ExecuteReader();
			var result = new List<Doctor>();

			while (reader.Read())
			{
				var doctor = new Doctor
				{
					Id = (int)reader["id"],
					name = reader["doctorName"].ToString(),
					SpecialtyId = (int)reader["specialtyId"],
					SpecialtyName = reader["specialtyName"].ToString(),
					Room = reader["room"].ToString()
				};
				result.Add(doctor);
			}
			int count = result.Count;

			connection.Close();
			return result;
		}
	}
}
